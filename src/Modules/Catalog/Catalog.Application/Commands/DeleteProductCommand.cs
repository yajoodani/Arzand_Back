using System;
using MediatR;

namespace Arzand.Modules.Catalog.Application.Commands;

public record DeleteProductCommand(Guid Id) : IRequest;

