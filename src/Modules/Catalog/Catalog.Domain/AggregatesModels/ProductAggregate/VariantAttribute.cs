using System;
using Arzand.Shared.Domain;

namespace Arzand.Modules.Catalog.Domain.AggregatesModels.ProductAggregate;

public class VariantAttribute : ValueObject
{
    public string Name { get; private set; }
    public string Value { get; private set; }

    private VariantAttribute() { } // Required by EF Core

    internal static VariantAttribute Create(string name, string value)
    {
        return new VariantAttribute
        {
            Name = name,
            Value = value
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Value;
    }
}

