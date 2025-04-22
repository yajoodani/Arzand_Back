using System;
using Arzand.Modules.Catalog.Application.DTOs;
using Arzand.Modules.Catalog.Domain.AggregatesModels.ProductAggregate;
using MediatR;

namespace Arzand.Modules.Catalog.Application.Commands;


public record CreateProductCommand(
    string Name,
    string Description,
    int CategoryId,
    Guid BrandId,
    List<CreateProductVariantDto> Variants
) : IRequest<Guid>;
