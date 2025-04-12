using System;
using Arzand.Modules.Catalog.Application.DTOs;
using Arzand.Modules.Catalog.Domain.AggregatesModels.ProductAggregate;
using MediatR;

namespace Arzand.Modules.Catalog.Application.Queries;

public record GetAllProductsQuery() : IRequest<List<ProductDto>>;
