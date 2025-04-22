using System;
using Arzand.Modules.Catalog.Application.Commands;
using FluentValidation;

namespace Arzand.Modules.Catalog.Application.Validators;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty().MaximumLength(100);
        RuleFor(p => p.Description).MaximumLength(1000);
        RuleFor(p => p.CategoryId).NotEmpty();
        RuleFor(p => p.BrandId).NotEmpty();

        RuleForEach(p => p.Variants)
            .ChildRules(variant => 
            {
                variant.RuleFor(v => v.Stock)
                    .NotEmpty()
                    .GreaterThanOrEqualTo(0);
            });
    }
}
