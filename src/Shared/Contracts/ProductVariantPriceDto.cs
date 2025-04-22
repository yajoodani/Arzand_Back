using System;
using Arzand.Shared.Application;

namespace Arzand.Shared.Contracts;

public class ProductVariantPriceDto
{
    public Guid ProductId { get; set;}
    public Guid VariantId { get; set;}
    public MoneyDto Price { get; set;}
}
