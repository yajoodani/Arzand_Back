using System;
using Arzand.Shared.Application;

namespace Arzand.Modules.Cart.Models;

public class CartItem
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public Guid VariantId { get; set; }
    public string VariantName { get; set; }
    public MoneyDto Price { get; set; } 
    public int Quantity { get; set; }
}
