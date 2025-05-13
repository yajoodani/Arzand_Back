using System;
using Arzand.Modules.Ordering.Application.DTOs;
using MediatR;

namespace Arzand.Modules.Ordering.Application.Queries;

public record GetOrderByIdQuery(Guid UserId, Guid OrderId) : IRequest<OrderDto?>;
