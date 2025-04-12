using System;

namespace Arzand.Modules.Catalog.Application.Exceptions;


public class NotFoundException : Exception
{
    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" with key ({key}) was not found.") { }
}

