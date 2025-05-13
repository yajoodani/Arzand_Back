using System;
using System.Text.Json;
using Arzand.Modules.Cart.Models;
using Arzand.Shared.Application;
using Arzand.Shared.Contracts;
using Arzand.Shared.Contracts.IntegrationEvents;
using DotNetCore.CAP;
using StackExchange.Redis;

namespace Arzand.Modules.Cart.Services;

public class CartService : ICartService
{
    private readonly IDatabase _db;
    private readonly ICatalogService _catalogService;
    private readonly ICapPublisher _capPublisher;

    public CartService(IConnectionMultiplexer redis, ICatalogService catalogService, ICapPublisher capPublisher)
    {
        _db = redis.GetDatabase();
        _catalogService = catalogService;
        _capPublisher = capPublisher;
    }

    public async Task<CartModel?> GetCartAsync(Guid userId)
    {
        var data = await _db.StringGetAsync(userId.ToString());
        if (data.IsNullOrEmpty) return null;

        return JsonSerializer.Deserialize<CartModel>(data!);
    }

    public async Task<CartModel> UpdateCartAsync(CartModel cart)
    {
        ValidateCartItems(cart);

        return await UpdateCartInternalAsync(cart);
    }

    public async Task<bool> DeleteCartAsync(Guid userId)
    {
        var dummyCart = new CartModel{ UserId = userId, Items = [] };
        await UpdateCartInternalAsync(dummyCart);
        return await _db.KeyDeleteAsync(userId.ToString());
    }

    public async Task CheckoutAsync(Guid userId)
    {
        var cart = await GetCartAsync(userId)
            ?? throw new Exception("Cart is empty.");
        if (cart!.Items.Count == 0) throw new Exception("Cart is empty.");

        var integrationEvent = new CartCheckedOutIntegrationEvent
        {
            UserId = userId,
            CheckedOutAt = DateTime.UtcNow,
            TotalPrice = new MoneyDto
            {
                Amount = cart.Items.Sum(i => i.Price.Amount * i.Quantity),
                Currency = cart.Items.First().Price.Currency,
            },
            Items = cart.Items.Select(i => new CartCheckedOutIntegrationEvent.CartItemDto
            {
                ProductId = i.ProductId,
                VariantId = i.VariantId,
                ProductName = i.ProductName,
                VariantName = i.VariantName,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        };

        await _capPublisher.PublishAsync("cart.checkout", integrationEvent);

        // Cart is cleared but the the stock is still reserved.
        await _db.KeyDeleteAsync(userId.ToString());
    }


    private async Task<CartModel> UpdateCartInternalAsync(CartModel cart)
    {
       var existingCart = await GetCartAsync(cart.UserId)
            ?? new CartModel { UserId = cart.UserId, Items = []};

        var cartDiff = GetCartDiff(cart, existingCart);
        var reserveStockDtos = cartDiff.Items.Select(i => new ReserveStockDto {
            ProductId = i.ProductId,
            VariantId = i.VariantId,
            Quantity = i.Quantity,
        }).ToList();
        var productVariantPriceDtos = await _catalogService.HandleReserveStockAsync(reserveStockDtos);

        // Replace the price retrieved from the client with the one from the DB
        foreach (var item in cart.Items)
        {
            item.Price = productVariantPriceDtos
                .First(d => d.ProductId == item.ProductId && d.VariantId == item.VariantId).Price;
        }

        var serialized = JsonSerializer.Serialize(cart);
        var created = await _db.StringSetAsync(cart.UserId.ToString(), serialized);
        if (!created) throw new Exception("Failed to update cart in Redis.");

        return await GetCartAsync(cart.UserId) ?? cart;
    }

    private CartModel GetCartDiff(CartModel inputCart, CartModel existingCart)
    {
        var cartDiff = new CartModel { UserId = inputCart.UserId, Items = [] };

        foreach (var inputItem in inputCart.Items)
        {
            var corrExistingItemQuantity = existingCart.Items
                .FirstOrDefault(i => i.ProductId == inputItem.ProductId && i.VariantId == inputItem.VariantId)
                ?.Quantity ?? 0;
            
            var diffItem = new CartItem {
                Quantity = inputItem.Quantity - corrExistingItemQuantity,
                ProductId = inputItem.ProductId,
                VariantId = inputItem.VariantId,
                ProductName = inputItem.ProductName,
                VariantName = inputItem.VariantName,
                Price = inputItem.Price,
            };
            cartDiff.Items.Add(diffItem);
        }

        var removedItems = existingCart.Items
            .Where(ei => !inputCart.Items.Any(i => i.ProductId == ei.ProductId && i.VariantId == ei.VariantId))
            .ToList();
        foreach (var removedItem in removedItems)
        {
            var diffItem = new CartItem {
                Quantity = -removedItem.Quantity,
                ProductId = removedItem.ProductId,
                VariantId = removedItem.VariantId,
                ProductName = removedItem.ProductName,
                VariantName = removedItem.VariantName,
                Price = removedItem.Price,
            };
            cartDiff.Items.Add(diffItem);
        }

        return cartDiff;
    }

    private void ValidateCartItems(CartModel cart)
    {
        var distinctItemsCount = cart.Items.Select(i => Tuple.Create(i.ProductId, i.VariantId)).Distinct().Count();
        if (cart.Items.Count > distinctItemsCount)
        {
            throw new Exception("Cart contains duplicate items.");
        }

        if (cart.Items.Exists(i => i.Quantity <= 0))
        {
            throw new Exception("Cart items must have positive values.");
        }
    }
}
