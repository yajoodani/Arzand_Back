using System;
using Arzand.Modules.Catalog.Domain.AggregatesModels.ProductAggregate;
using Arzand.Shared.Domain;

namespace Arzand.Modules.Catalog.Domain.AggregatesModels.BrandAggregate;

public class Brand : Entity<Guid>, IAggregateRoot
{
    public string Name { get; private set; }

    private Brand() {} // Required for EF Core

    public ICollection<Product> Products{ get; private set; } = new List<Product>();

    public static Brand Create(string name)
    {
        return new Brand 
        {
            Id = Guid.NewGuid(),
            Name = name
        };
    }

    public void Update(string name)
    {
        Name = name;
    }
}
