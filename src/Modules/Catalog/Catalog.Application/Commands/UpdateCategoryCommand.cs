using System;
using MediatR;

namespace Arzand.Modules.Catalog.Application.Commands;

public record UpdateCategoryCommand(int Id, string Name) : IRequest;