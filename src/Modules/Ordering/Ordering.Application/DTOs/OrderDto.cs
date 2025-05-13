using System;
using Arzand.Modules.Ordering.Domain.AggregatesModels.OrderAggregate;
using Arzand.Shared.Application;

namespace Arzand.Modules.Ordering.Application.DTOs;

public record OrderDto(
    Guid Id,
    Guid BuyerId,
    OrderStatus Status,
    Address ShippingAddress,
    List<OrderItemDto> Items,
    MoneyDto TotalAmount
);
