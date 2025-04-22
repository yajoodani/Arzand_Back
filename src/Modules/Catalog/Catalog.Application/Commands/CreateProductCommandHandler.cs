using Arzand.Modules.Catalog.Application.DTOs;
using Arzand.Modules.Catalog.Domain.AggregatesModels.BrandAggregate;
using Arzand.Modules.Catalog.Domain.AggregatesModels.CategoryAggregate;
using Arzand.Modules.Catalog.Domain.AggregatesModels.ProductAggregate;
using AutoMapper;
using MediatR;

namespace Arzand.Modules.Catalog.Application.Commands;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IBrandRepository _brandRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(
        IBrandRepository brandRepository,
        ICategoryRepository categoryRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _brandRepository = brandRepository;
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var brand = await _brandRepository.GetByIdAsync(command.BrandId);
        if (brand is null)
            throw new ArgumentException("Invalid Brand ID");

        var category = await _categoryRepository.GetByIdAsync(command.CategoryId);
        if (category is null)
            throw new ArgumentException("Invalid Category ID");

        var product = Product.Create(
            name: command.Name,
            description: command.Description,
            categoryId: category.Id,
            brandId: brand.Id,
            variants: _mapper.Map<List<ProductVariant>>(command.Variants)
        );

        await _productRepository.AddAsync(product);
        await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return product.Id;
    }

}

