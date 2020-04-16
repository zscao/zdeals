using FluentValidation;

using ZDeals.Identity.Contract.Requests;

namespace ZDeals.Identity.Validations
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
