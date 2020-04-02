using FluentValidation;

using ZDeals.Api.Contract.Requests;

namespace ZDeals.Api.Validations
{
    public class CreateUserValidator: AbstractValidator<CreateUserRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Username).NotEmpty().MaximumLength(50);

            RuleFor(x => x.Nickname).NotEmpty().MaximumLength(50);

            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(50);
        }
    }
}
