using System;
using Arzand.Shared.Application;

namespace Arzand.Modules.Catalog.Application.DTOs;

public class ProductVariantDto
{
    public Guid Id { get; set;}
    public string Name { get; set; }
    public string Description { get; set; }
    public MoneyDto Price { get; set; }
    public int Stock { get; set; }
    public List<VariantAttributeDto> Attributes { get; set; }
}

