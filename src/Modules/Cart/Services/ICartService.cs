using System;
using Arzand.Modules.Cart.Models;

namespace Arzand.Modules.Cart.Services;

public interface ICartService
{
    Task<CartModel?> GetCartAsync(Guid userId);
    Task<CartModel> UpdateCartAsync(CartModel cart);
    Task<bool> DeleteCartAsync(Guid userId);
}
