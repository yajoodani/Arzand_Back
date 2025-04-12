using System;
using Arzand.Shared.Domain;

namespace Arzand.Modules.Catalog.Domain.AggregatesModels.ProductAggregate;

public class ProductVariant : Entity<Guid>
{
    private List<VariantAttribute> _attributes = new();
    
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; }
    public Money Price { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public IReadOnlyCollection<VariantAttribute> Attributes => _attributes.AsReadOnly();

    private ProductVariant() {} // Required for EF Core //////// WHY???

    public static ProductVariant Create(Guid productId, Money price, string name, 
        string description, List<VariantAttribute> variantAttributes)
    {
        return new ProductVariant
        {
            ProductId = productId,
            Price = price,
            Name = name,
            Description = description,
            _attributes = variantAttributes ?? []
        };
    }

    public void AddAttribute(string name, string value)
    {
        _attributes.Add(VariantAttribute.Create(name, value));
    }
}
