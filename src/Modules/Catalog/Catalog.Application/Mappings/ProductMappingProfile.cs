using System;
using Arzand.Modules.Catalog.Application.DTOs;
using Arzand.Modules.Catalog.Domain.AggregatesModels.ProductAggregate;
using Arzand.Shared.Application;
using Arzand.Shared.Domain;
using AutoMapper;

namespace Arzand.Modules.Catalog.Application.Mappings;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        // CreateMap<Product, ProductDto>()
        //     .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.Name))
        //     .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));

        // Create -> Domain
        CreateMap<CreateProductDto, Product>()
            .ConstructUsing(src =>
                Product.Create(
                    src.Name,
                    src.Description,
                    src.CategoryId,
                    src.BrandId,
                    new List<ProductVariant>() // variants mapped below manually
                )
            );

        CreateMap<CreateProductVariantDto, ProductVariant>()
            .ConstructUsing((src, ctx) =>
                Product.CreateVariant(
                    Guid.Empty, // will be assigned in aggregate after root is created
                    ctx.Mapper.Map<Money>(src.Price),
                    // new Money(src.Price.Amount, src.Price.Currency),
                    src.Stock,
                    src.Name,
                    src.Description,
                    // src.Attributes.Select(a => Product.CreateVariantAttribute(a.Name, a.Value)).ToList()
                    ctx.Mapper.Map<List<VariantAttribute>>(src.Attributes)
                )
            )
            .ForMember(dest => dest.Attributes, opt => opt.Ignore());;

        CreateMap<VariantAttributeDto, VariantAttribute>()
            .ConstructUsing(src => Product.CreateVariantAttribute(src.Name, src.Value));

        CreateMap<MoneyDto, Money>();

        // Domain -> DTO
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.Name))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants));

        CreateMap<ProductVariant, ProductVariantDto>()
            .ForMember(dest => dest.Attributes, opt => opt.MapFrom(src => src.Attributes));

        CreateMap<VariantAttribute, VariantAttributeDto>();

        CreateMap<Money, MoneyDto>();
    }
}