using System;
using Arzand.Modules.Catalog.Domain.AggregatesModels.ProductAggregate;
using Arzand.Shared.Application;
using Arzand.Shared.Contracts;

namespace Arzand.Modules.Catalog.Application.Services;

public class CatalogService : ICatalogService
{
    private readonly IProductRepository _productRepository;

    public CatalogService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<ProductVariantPriceDto>> HandleReserveStockAsync(List<ReserveStockDto> cartItems, CancellationToken cancellationToken = default)
    {
        // The Dtos are assumed to contain only the updated quantity diffs,
        // which can be non-positive

        using var transaction = await _productRepository.UnitOfWork.BeginTransactionAsync();
        var ret = new List<ProductVariantPriceDto>();

        try
        {
            var productIds = cartItems.Select(i => i.ProductId).ToList();
            var products = await _productRepository.GetByIdsAsync(productIds);

            foreach (var item in cartItems)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product == null)
                {
                    throw new Exception($"Item with product id {item.ProductId} and variant id {item.VariantId} not found.");
                }

                product.ReserveVariantStock(item.VariantId, item.Quantity);
                var variant = product.Variants.First(v => v.Id == item.VariantId);
                ret.Add(new ProductVariantPriceDto 
                { 
                    ProductId = variant.ProductId, 
                    VariantId = variant.Id, 
                    Price = new MoneyDto { Amount = variant.Price.Amount, Currency = variant.Price.Currency } 
                });
            }

            await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return ret;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }

    }

}
