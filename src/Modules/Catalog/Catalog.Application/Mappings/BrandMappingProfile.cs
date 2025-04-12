using System;
using Arzand.Modules.Catalog.Application.DTOs;
using Arzand.Modules.Catalog.Domain.AggregatesModels.BrandAggregate;
using AutoMapper;

namespace Arzand.Modules.Catalog.Application.Mappings;

public class BrandMappingProfile : Profile
{
    public BrandMappingProfile()
    {
        CreateMap<Brand, BrandDto>();
    }
}
