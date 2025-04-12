using System;
using Arzand.Modules.Catalog.Domain.AggregatesModels.BrandAggregate;
using Arzand.Modules.Catalog.Domain.AggregatesModels.CategoryAggregate;
using Arzand.Shared.Domain;

namespace Arzand.Modules.Catalog.Domain.AggregatesModels.ProductAggregate;

public class Product : Entity<Guid>, IAggregateRoot
{
    private List<ProductVariant> _variants = new();

    public string Name { get; private set; }
    public string Description { get; private set; }
    public int CategoryId { get; private set; }
    public Category Category { get; private set; }
    public Guid BrandId { get; private set; }
    public Brand Brand { get; private set; }
    public IReadOnlyCollection<ProductVariant> Variants => _variants.AsReadOnly();

    private Product() {} // Required for EF Core

    public static Product Create(string name, string description, int categoryId, Guid brandId, List<ProductVariant> variants)
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            CategoryId = categoryId,
            BrandId = brandId,
            _variants = variants ?? []
        };
    }

    public void AddVariants(List<ProductVariant> variants)
    {
        _variants.AddRange(variants);
        
    }

    public void Update(string name, string description, int categoryId, Guid brandId)
    {
        Name = name;
        Description = description;
        CategoryId = categoryId;
        BrandId = brandId;
    }
}