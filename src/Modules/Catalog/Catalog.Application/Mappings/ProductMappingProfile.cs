using System;
using Arzand.Modules.Catalog.Application.DTOs;
using Arzand.Modules.Catalog.Domain.AggregatesModels.ProductAggregate;
using AutoMapper;

namespace Arzand.Modules.Catalog.Application.Mappings;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.Name))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));
    }
}