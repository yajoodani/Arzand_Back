using System;
using MediatR;

namespace Arzand.Modules.Catalog.Application.Commands;

public record CreateBrandCommand(
    string Name
) : IRequest<Guid>;