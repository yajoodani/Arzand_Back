using System;
using Arzand.Shared.Application;

namespace Arzand.Modules.Catalog.Application.DTOs;

public class CreateProductVariantDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Stock { get; set; }
    public MoneyDto Price { get; set; }
    public List<VariantAttributeDto> Attributes { get; set; }
}

