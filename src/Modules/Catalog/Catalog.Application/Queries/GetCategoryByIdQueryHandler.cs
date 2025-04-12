using System;
using Arzand.Modules.Catalog.Application.DTOs;
using Arzand.Modules.Catalog.Application.Exceptions;
using Arzand.Modules.Catalog.Domain.AggregatesModels.CategoryAggregate;
using AutoMapper;
using MediatR;

namespace Arzand.Modules.Catalog.Application.Queries;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
{
    public readonly ICategoryRepository _categoryRepository;
    public readonly IMapper _mapper;

    public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(query.Id);
        if (category == null)
        {
            throw new NotFoundException(nameof(Category), query.Id);
        }

        return _mapper.Map<CategoryDto>(category);
    }
}
