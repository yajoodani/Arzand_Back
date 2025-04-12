using System;
using MediatR;

namespace Arzand.Modules.Catalog.Application.Commands;

public record DeleteCategoryCommand(int Id) : IRequest;
