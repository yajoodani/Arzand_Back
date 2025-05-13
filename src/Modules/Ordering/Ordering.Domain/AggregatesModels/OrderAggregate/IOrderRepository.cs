using System;
using Arzand.Shared.Domain;

namespace Arzand.Modules.Ordering.Domain.AggregatesModels.OrderAggregate;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Order>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(Order order, CancellationToken cancellationToken = default);
    void Update(Order order);
}
