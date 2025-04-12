using System;
using Arzand.Modules.Catalog.Domain.AggregatesModels.ProductAggregate;
using Arzand.Shared.Domain;

namespace Arzand.Modules.Catalog.Domain.AggregatesModels.CategoryAggregate;

public class Category : Entity<int>, IAggregateRoot
{
    public string Name { get; private set; }
    public ICollection<Product> Products{ get; private set; } = new List<Product>();

    private Category() {} // Required for EF Core

    public static Category Create(string name)
    {
        return new Category
        {
            Id = new Random().Next(1,100000),
            Name = name
        };
    }

    public void Update(string name)
    {
        Name = name;
    }
}
