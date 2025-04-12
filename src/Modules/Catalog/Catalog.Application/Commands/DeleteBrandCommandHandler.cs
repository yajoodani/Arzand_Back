using System;
using Arzand.Modules.Catalog.Application.Exceptions;
using Arzand.Modules.Catalog.Domain.AggregatesModels.BrandAggregate;
using MediatR;

namespace Arzand.Modules.Catalog.Application.Commands;

public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand>
{
    private readonly IBrandRepository _brandRepository;

    public DeleteBrandCommandHandler(IBrandRepository repository)
    {
        _brandRepository = repository;
    }

    public async Task Handle(DeleteBrandCommand command, CancellationToken cancellationToken)
    {
        var brand = await _brandRepository.GetByIdAsync(command.Id);
        if (brand == null)
        {
            throw new NotFoundException(nameof(Brand), command.Id);
        }

        _brandRepository.Remove(brand);
        await _brandRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
