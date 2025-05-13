using System;
using MediatR;

namespace Arzand.Modules.Ordering.Application.Commands;

public record CancelOrderCommand(Guid UserId, Guid Id) : IRequest;
