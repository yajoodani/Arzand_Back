using System;
using Arzand.Modules.Catalog.Domain.AggregatesModels.CategoryAggregate;
using MediatR;

namespace Arzand.Modules.Catalog.Application.Commands;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<int> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = Category.Create(command.Name);
        await _categoryRepository.AddAsync(category);
        
        await _categoryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return category.Id;
    }
}
