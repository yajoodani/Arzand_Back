using System;
using Arzand.Modules.Catalog.Application.DTOs;
using Arzand.Modules.Catalog.Application.Exceptions;
using Arzand.Modules.Catalog.Domain.AggregatesModels.ProductAggregate;
using AutoMapper;
using MediatR;

namespace Arzand.Modules.Catalog.Application.Commands;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public UpdateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.Id);

        if (product == null)
            throw new NotFoundException(nameof(Product), command.Id);

        product.Update(command.Name, command.Description, command.CategoryId, command.BrandId);
        await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
