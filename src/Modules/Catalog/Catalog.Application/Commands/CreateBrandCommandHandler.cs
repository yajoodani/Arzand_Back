using System;
using Arzand.Modules.Catalog.Domain.AggregatesModels.BrandAggregate;
using MediatR;

namespace Arzand.Modules.Catalog.Application.Commands;

public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, Guid>
{
    private readonly IBrandRepository _brandRepository;

    public CreateBrandCommandHandler(IBrandRepository repository)
    {
        _brandRepository = repository;
    }

    public async Task<Guid> Handle(CreateBrandCommand command, CancellationToken cancellationToken)
    {
        var brand = Brand.Create(command.Name);
        await _brandRepository.AddAsync(brand);
        
        await _brandRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return brand.Id;
    }
}
