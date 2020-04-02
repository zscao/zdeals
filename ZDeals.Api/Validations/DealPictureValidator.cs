using FluentValidation;

using ZDeals.Api.Contract.Requests;

namespace ZDeals.Api.Validations
{
    public class SaveDealPictureValidator: AbstractValidator<SaveDealPictureRequest>
    {
        public SaveDealPictureValidator()
        {
            RuleFor(x => x.FileName).NotEmpty();
        }
    }
}
