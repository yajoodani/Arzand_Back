using System;
using Arzand.Shared.Domain;

namespace Arzand.Modules.Catalog.Domain.AggregatesModels.CategoryAggregate;

public interface ICategoryRepository : IRepository<Category>
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<Category> AddAsync(Category category);
    void Remove(Category category);
}
