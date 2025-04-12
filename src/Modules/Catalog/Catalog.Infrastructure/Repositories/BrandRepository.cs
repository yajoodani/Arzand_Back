using System;
using Arzand.Modules.Catalog.Domain.AggregatesModels.BrandAggregate;
using Arzand.Modules.Catalog.Infrastructure.Data;
using Arzand.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Arzand.Modules.Catalog.Infrastructure.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly CatalogDbContext _context;

    public IUnitOfWork UnitOfWork 
    {
        get { return _context; }
    }

    public BrandRepository (CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<List<Brand>> GetAllAsync() => await _context.Brands.ToListAsync();
    
    public async Task<Brand?> GetByIdAsync(Guid id) => await _context.Brands.FindAsync(id);
    
    public async Task<Brand> AddAsync(Brand brand)
    {
        await _context.Brands.AddAsync(brand);
        return brand;
    }

    public void Remove(Brand brand)
    {
        _context.Brands.Remove(brand);
    }
}
