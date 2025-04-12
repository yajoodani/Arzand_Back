using System;
using Arzand.Modules.Catalog.Domain.AggregatesModels.ProductAggregate;
using Arzand.Modules.Catalog.Infrastructure.Data;
using Arzand.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Arzand.Modules.Catalog.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly CatalogDbContext _context;

    public IUnitOfWork UnitOfWork {
        get { return _context; }
    }

    public ProductRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync() => await _context.Products.ToListAsync();

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products
            .Include(p => p.Variants)
                .ThenInclude(v => v.Attributes)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product> AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        return product;
    }

    public void Remove(Product product)
    {
        _context.Products.Remove(product);
    }
}

