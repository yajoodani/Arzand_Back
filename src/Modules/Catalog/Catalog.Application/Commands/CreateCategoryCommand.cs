using MediatR;

namespace Arzand.Modules.Catalog.Application.Commands;

public record CreateCategoryCommand(string Name) : IRequest<int>;
