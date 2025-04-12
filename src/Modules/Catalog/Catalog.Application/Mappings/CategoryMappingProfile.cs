using System;
using Arzand.Modules.Catalog.Application.DTOs;
using Arzand.Modules.Catalog.Domain.AggregatesModels.CategoryAggregate;
using AutoMapper;

namespace Arzand.Modules.Catalog.Application.Mappings;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, CategoryDto>();
    }
}
