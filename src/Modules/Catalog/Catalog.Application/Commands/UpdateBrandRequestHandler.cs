using System;
using Arzand.Modules.Catalog.Application.Exceptions;
using Arzand.Modules.Catalog.Domain.AggregatesModels.BrandAggregate;
using MediatR;

namespace Arzand.Modules.Catalog.Application.Commands;

public class UpdateBrandRequestHandler : IRequestHandler<UpdateBrandCommand>
{
    private readonly IBrandRepository _brandRepository;

    public UpdateBrandRequestHandler(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public async Task Handle(UpdateBrandCommand command, CancellationToken cancellationToken)
    {
        var brand = await _brandRepository.GetByIdAsync(command.Id);
        if (brand == null)
        {
            throw new NotFoundException(nameof(Brand), command.Id);
        }

        brand.Update(command.Name);
        await _brandRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
