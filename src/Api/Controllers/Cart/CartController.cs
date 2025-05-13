using System.Security.Claims;
using Arzand.Modules.Cart.Models;
using Arzand.Modules.Cart.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Arzand.Api.Controllers.Cart;

[ApiController]
[Route("api/cart")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var userId = GetUserId();
        var cart = await _cartService.GetCartAsync(userId);
        if (cart == null) return NotFound();
        return Ok(cart);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateCart(List<CartItem> cartItems)
    {
        var userId = GetUserId();
        var cart = new CartModel { UserId = userId, Items = cartItems };
        var updated = await _cartService.UpdateCartAsync(cart);
        return Ok(updated);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteCart()
    {
        var userId = GetUserId();
        var deleted = await _cartService.DeleteCartAsync(userId);
        if (!deleted) return NotFound();
        return NoContent();
    }

    [Authorize]
    [HttpPost("checkout")]
    public async Task<IActionResult> CheckoutAsync()
    {
        var userId = GetUserId();
        await _cartService.CheckoutAsync(userId);
        return Accepted();
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userIdClaim))
            throw new Exception("User ID claim not found.");
        return Guid.Parse(userIdClaim);
    }
}
