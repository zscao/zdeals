using System.Collections.Generic;
using System.Linq;
using ZDeals.Api.Contract.Responses;
using ZDeals.Common;

namespace ZDeals.Api.Mapping
{
    public static class ErrorMapping
    {
        public static ErrorResponse ToErrorResponse(this IEnumerable<Error> errors, int status = 400)
        {
            var response = new ErrorResponse() { Status = status };

            if (errors == null) return response;

            response.Errors = errors.Select(error => new ErrorDetail
            {
                Code = error.Code,
                Message = error.Message
            }).ToList();

            return response;
        }
    }
}
