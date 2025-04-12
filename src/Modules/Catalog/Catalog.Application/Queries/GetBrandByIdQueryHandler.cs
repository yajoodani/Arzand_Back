using System;
using Arzand.Modules.Catalog.Application.DTOs;
using Arzand.Modules.Catalog.Application.Exceptions;
using Arzand.Modules.Catalog.Domain.AggregatesModels.BrandAggregate;
using AutoMapper;
using MediatR;

namespace Arzand.Modules.Catalog.Application.Queries;

public class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, BrandDto>
{
    private readonly IBrandRepository _brandRepository;
    private readonly IMapper _mapper;

    public GetBrandByIdQueryHandler(IBrandRepository brandRepository, IMapper mapper)
    {
        _brandRepository = brandRepository;
        _mapper = mapper;
    }

    public async Task<BrandDto> Handle(GetBrandByIdQuery query, CancellationToken cancellationToken)
    {
        var brand = await _brandRepository.GetByIdAsync(query.Id);

        if (brand == null)
        {
            throw new NotFoundException(nameof(Brand), query.Id);
        }

        return _mapper.Map<BrandDto>(brand);
    }
}
