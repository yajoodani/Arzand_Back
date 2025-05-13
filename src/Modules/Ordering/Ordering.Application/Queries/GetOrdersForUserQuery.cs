using System;
using Arzand.Modules.Ordering.Application.DTOs;
using MediatR;

namespace Arzand.Modules.Ordering.Application.Queries;

public record GetOrdersForUserQuery(Guid UserId) : IRequest<List<OrderDto>>;
