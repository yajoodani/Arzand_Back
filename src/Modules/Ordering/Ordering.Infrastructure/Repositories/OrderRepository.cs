using System;
using Arzand.Modules.Ordering.Domain.AggregatesModels.OrderAggregate;
using Arzand.Modules.Ordering.Infrastructure.Data;
using Arzand.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Arzand.Modules.Ordering.Infrastructure.Repositories;


public class OrderRepository : IOrderRepository
{
    private readonly OrderingDbContext _context;

    public IUnitOfWork UnitOfWork {
        get { return _context; }
    }

    public OrderRepository(OrderingDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<List<Order>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .Where(o => o.BuyerId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _context.Orders.AddAsync(order, cancellationToken);
    }

    public void Update(Order order)
    {
        _context.Orders.Update(order);
    }
}
