using FluentValidation;

using ZDeals.Identity.Contract.Requests;

namespace ZDeals.Identity.Validations
{
    public class LoginRequestValidator: AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty().MaximumLength(50);

            RuleFor(x => x.Password).NotEmpty().MaximumLength(50);
        }
    }
}
