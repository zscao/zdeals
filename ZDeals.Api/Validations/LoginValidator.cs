﻿using FluentValidation;

using ZDeals.Api.Contract.Requests;

namespace ZDeals.Api.Validations
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
