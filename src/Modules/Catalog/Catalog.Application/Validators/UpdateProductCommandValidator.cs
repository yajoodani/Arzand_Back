using System;
using Arzand.Modules.Catalog.Application.Commands;
using FluentValidation;

namespace Arzand.Modules.Catalog.Application.Validators;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty().MaximumLength(100);
        RuleFor(p => p.Description).MaximumLength(1000);
        RuleFor(p => p.CategoryId).NotEmpty();
        RuleFor(p => p.BrandId).NotEmpty();

        // TODO Add later
        // RuleForEach(p => p.Variants)
        //     .ChildRules(variant => 
        //     {
        //         variant.RuleFor(v => v.Stock)
        //             .GreaterThanOrEqualTo(0);
        //     });
    }
}
