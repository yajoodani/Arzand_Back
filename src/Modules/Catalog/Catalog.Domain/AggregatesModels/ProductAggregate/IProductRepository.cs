using System;
using Arzand.Shared.Domain;

namespace Arzand.Modules.Catalog.Domain.AggregatesModels.ProductAggregate;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(Guid id);
    Task<List<Product>> GetByIdsAsync(IEnumerable<Guid> ids);
    Task<Product> AddAsync(Product product);
    void Remove(Product product);
}

