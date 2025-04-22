using System;

namespace Arzand.Modules.Cart.Models;

public class CartModel
{
    public Guid UserId { get; set; }
    public List<CartItem> Items { get; set; } = new();
}