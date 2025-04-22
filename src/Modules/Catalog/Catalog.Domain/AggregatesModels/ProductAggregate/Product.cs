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

    public void Update(string name, string description, int categoryId, Guid brandId)
    {
        Name = name;
        Description = description;
        CategoryId = categoryId;
        BrandId = brandId;
    }

    public static ProductVariant CreateVariant(Guid productId, Money price, int stock, string name, 
        string description, List<VariantAttribute> variantAttributes)
    {
        var variant = ProductVariant.Create(productId, price, stock, name, 
            description, variantAttributes);
        
        return variant;
    }

    public void UpdateVariant(Guid variantId, Money price, int stock, string name, string description)
    {
        var variant = GetVariant(variantId);
        variant.Update(price, stock, name, description);
    }

    public void SetVariantStock(Guid variantId, int stock)
    {
        var variant = GetVariant(variantId);
        variant.SetStock(stock);
    }

    public void ReserveVariantStock(Guid variantId, int quantity)
    {
        /**
            With negative quantity values can be used to free reserved stock.
        */
        
        var variant = GetVariant(variantId);
        variant.SetStock(variant.Stock - quantity);
    }

    public void AddAttributeToVariant(Guid variantId, string name, string value)
    {
        var variant = GetVariant(variantId);
        variant.AddAttribute(name, value);
    }

    public void AddVariants(List<ProductVariant> variants)
    {
        _variants.AddRange(variants);
        
    }

    private ProductVariant GetVariant(Guid variantId)
    {
        var variant = _variants.FirstOrDefault(v => v.Id == variantId) 
            ?? throw new Exception("Variant not found.");
        return variant;
    }

    public static VariantAttribute CreateVariantAttribute(string name, string description)
    {
        var attribute = VariantAttribute.Create(name, description);
        return attribute;
    }
}