using FluentValidation;

using ZDeals.Api.Contract.Requests;

namespace ZDeals.Api.Validations
{
    public class CreateDealValidator: AbstractValidator<CreateDealRequest>
    {
        public CreateDealValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);

            RuleFor(x => x.HighLight).MaximumLength(50);

            RuleFor(x => x.Description).MaximumLength(400);

            RuleFor(x => x.Discount).MaximumLength(20);

            RuleFor(x => x.Source).MaximumLength(400);
        }
    }


    public class UpdateDealValidator : AbstractValidator<UpdateDealRequest>
    {
        public UpdateDealValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);

            RuleFor(x => x.HighLight).MaximumLength(200);

            RuleFor(x => x.Description).MaximumLength(2000);

            RuleFor(x => x.Discount).MaximumLength(100);
        }
    }
}
