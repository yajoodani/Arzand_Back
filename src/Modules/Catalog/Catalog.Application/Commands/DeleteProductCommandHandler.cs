using System;
using Arzand.Modules.Catalog.Application.Exceptions;
using Arzand.Modules.Catalog.Domain.AggregatesModels.ProductAggregate;
using MediatR;

namespace Arzand.Modules.Catalog.Application.Commands;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.Id)
            ?? throw new NotFoundException(nameof(Product), command.Id);

        _productRepository.Remove(product);
        await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
