using System;
using Arzand.Modules.Catalog.Application.Exceptions;
using Arzand.Modules.Catalog.Domain.AggregatesModels.CategoryAggregate;
using MediatR;

namespace Arzand.Modules.Catalog.Application.Commands;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(command.Id) 
            ?? throw new NotFoundException(nameof(Category), command.Id);
        
        category.Update(command.Name);
        await _categoryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
