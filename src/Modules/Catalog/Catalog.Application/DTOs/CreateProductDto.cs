using System;

namespace Arzand.Modules.Catalog.Application.DTOs;

public class CreateProductDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int CategoryId { get; set; }
    public Guid BrandId { get; set; }
    public List<CreateProductVariantDto> Variants { get; set; }
}

