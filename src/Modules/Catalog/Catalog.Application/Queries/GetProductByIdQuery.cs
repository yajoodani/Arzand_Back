using System;
using Arzand.Modules.Catalog.Application.DTOs;
using MediatR;

namespace Arzand.Modules.Catalog.Application.Queries;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto>;
