using System;
using Arzand.Modules.Ordering.Application.DTOs;
using Arzand.Modules.Ordering.Domain.AggregatesModels.OrderAggregate;
using AutoMapper;
using MediatR;

namespace Arzand.Modules.Ordering.Application.Queries;

public class GetOrdersForUserQueryHandler : IRequestHandler<GetOrdersForUserQuery, List<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrdersForUserQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<List<OrderDto>> Handle(GetOrdersForUserQuery query, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetByUserIdAsync(query.UserId, cancellationToken);
        return _mapper.Map<List<OrderDto>>(orders);
    }
}
