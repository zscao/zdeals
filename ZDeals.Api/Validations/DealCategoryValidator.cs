using FluentValidation;

using ZDeals.Api.Contract.Requests;

namespace ZDeals.Api.Validations
{
    public class SaveDealCategoriesValidator: AbstractValidator<SaveDealCategoriesRequest>
    {
        public SaveDealCategoriesValidator()
        {
            RuleFor(x => x.Categories).NotNull();
        }
    }
}
