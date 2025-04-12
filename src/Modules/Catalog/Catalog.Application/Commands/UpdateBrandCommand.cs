using System;
using MediatR;

namespace Arzand.Modules.Catalog.Application.Commands;

public record UpdateBrandCommand(
    Guid Id,
    string Name
) : IRequest;
