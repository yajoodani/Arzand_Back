using System;
using Arzand.Modules.Catalog.Application.Exceptions;
using Arzand.Modules.Catalog.Domain.AggregatesModels.CategoryAggregate;
using MediatR;

namespace Arzand.Modules.Catalog.Application.Commands;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(command.Id)
            ?? throw new NotFoundException(nameof(Category), command.Id);

        _categoryRepository.Remove(category);
        await _categoryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
