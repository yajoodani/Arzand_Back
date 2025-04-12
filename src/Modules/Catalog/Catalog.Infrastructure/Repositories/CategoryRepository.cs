using System;
using Arzand.Modules.Catalog.Domain.AggregatesModels.BrandAggregate;
using Arzand.Modules.Catalog.Domain.AggregatesModels.CategoryAggregate;
using Arzand.Modules.Catalog.Infrastructure.Data;
using Arzand.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Arzand.Modules.Catalog.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly CatalogDbContext _context;

    public IUnitOfWork UnitOfWork {
        get { return _context; }
    } 

    public CategoryRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllAsync() => await _context.Categories.ToListAsync();

    public async Task<Category?> GetByIdAsync(int id) => await _context.Categories.FindAsync(id);

    public async Task<Category> AddAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
        return category;
    }

    public void Remove(Category category)
    {
        _context.Categories.Remove(category);
    }
}
