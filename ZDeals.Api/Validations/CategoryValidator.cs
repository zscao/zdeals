﻿using FluentValidation;

using ZDeals.Api.Contract.Requests;

namespace ZDeals.Api.Validations
{
    public class CreateCategoryValidator: AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Code).NotEmpty().MaximumLength(50);

            RuleFor(x => x.Title).NotEmpty().MaximumLength(50);

            RuleFor(x => x.ParentId).NotNull().GreaterThan(0);
        }
    }

    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryRequest>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(50);

            RuleFor(x => x.ParentId).GreaterThan(0);
        }
    }
}
