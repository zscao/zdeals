using FluentValidation;

using ZDeals.Api.Contract.Requests;

namespace ZDeals.Api.Validations
{
    public class CreateStoreValidator: AbstractValidator<CreateStoreRequest>
    {
        public CreateStoreValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);

            RuleFor(x => x.Website).NotEmpty().MaximumLength(200).Must(x =>
            {
                return System.Uri.TryCreate(x, System.UriKind.Absolute, out System.Uri uri);

            }).WithMessage("Website URL is invalid.");

            RuleFor(x => x.Domain).NotEmpty().MaximumLength(100);
        }
    }


    public class UpdateStoreValidator : AbstractValidator<UpdateStoreRequest>
    {
        public UpdateStoreValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);

            RuleFor(x => x.Website).NotEmpty().MaximumLength(200).Must(x =>
            {
                return System.Uri.TryCreate(x, System.UriKind.Absolute, out System.Uri uri);

            }).WithMessage("Website URL is invalid.");

            RuleFor(x => x.Domain).NotEmpty().MaximumLength(100);
        }
    }
}
