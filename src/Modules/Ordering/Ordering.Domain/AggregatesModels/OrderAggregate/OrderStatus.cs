namespace Arzand.Modules.Ordering.Domain.AggregatesModels.OrderAggregate;

public enum OrderStatus
{
    Created = 0,
    // Awaiting Payment 
    Pending = 1,
    Paid = 2,
    Shipped = 3,
    Delivered = 4,
    Cancelled = 5,
    Failed = 6
}

