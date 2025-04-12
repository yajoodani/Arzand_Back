using System;

namespace Arzand.Modules.Catalog.Application.DTOs;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid BrandId { get; set; }
    public string Brand { get; set; }
    public int CategoryId { get; set; }
    public string Category { get; set; }
}
