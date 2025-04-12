using System;
using Arzand.Shared.Domain;

namespace Arzand.Modules.Catalog.Domain.AggregatesModels.BrandAggregate;

public interface IBrandRepository : IRepository<Brand>
{
    Task<List<Brand>> GetAllAsync();
    Task<Brand?> GetByIdAsync(Guid id);
    Task<Brand> AddAsync(Brand brand);
    void Remove(Brand brand);
}
