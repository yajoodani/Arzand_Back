using System;
using Arzand.Modules.Ordering.Application.DTOs;
using Arzand.Modules.Ordering.Domain.AggregatesModels.OrderAggregate;
using AutoMapper;
using MediatR;

namespace Arzand.Modules.Ordering.Application.Queries;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrderByIdQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<OrderDto?> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(query.OrderId, cancellationToken);
        if (order is null || order.BuyerId != query.UserId)
        {
            return null;
        }
        return _mapper.Map<OrderDto>(order);
    }
}
