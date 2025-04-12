using System;
using MediatR;

namespace Arzand.Modules.Catalog.Application.Commands;

public record DeleteBrandCommand(Guid Id) : IRequest;
