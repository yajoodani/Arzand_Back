using System;
using Arzand.Modules.Ordering.Application.DTOs;
using Arzand.Modules.Ordering.Domain.AggregatesModels.OrderAggregate;
using Arzand.Shared.Application;
using Arzand.Shared.Domain;
using AutoMapper;

namespace Arzand.Modules.Ordering.Application.Mappings;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {

        // Domain -> DTO
        CreateMap<Order, OrderDto>();
        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<Money, MoneyDto>();
    }
}
