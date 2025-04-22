using System;
using Arzand.Shared.Domain;

namespace Arzand.Modules.Catalog.Domain.AggregatesModels.ProductAggregate;

public class ProductVariant : Entity<Guid>
{
    private List<VariantAttribute> _attributes = new();
    
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; }
    public Money Price { get; private set; }
    public int Stock { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public IReadOnlyCollection<VariantAttribute> Attributes => _attributes.AsReadOnly();

    private ProductVariant() {} // Required for EF Core

    internal static ProductVariant Create(Guid productId, Money price, int stock, string name, 
        string description, List<VariantAttribute> variantAttributes)
    {
        var variant = new ProductVariant
        {
            ProductId = productId,
            Price = price,
            Name = name,
            Description = description,
            _attributes = variantAttributes ?? []
        };
        variant.SetStock(stock);
        return variant;
    }

    internal void Update(Money price, int stock, string name, string description)
    {
        Price = price;
        SetStock(stock);
        Name = name;
        Description = description;
    }

    internal void AddAttribute(string name, string value)
    {
        _attributes.Add(VariantAttribute.Create(name, value));
    }

    internal void SetStock(int stock)
    {
        if (stock < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(stock), "Insufficient stock.");
        }
        Stock = stock;
    }
}
