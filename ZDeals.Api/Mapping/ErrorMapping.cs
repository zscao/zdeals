﻿using System.Collections.Generic;
using System.Linq;
using ZDeals.Api.Contract.Responses;
using ZDeals.Common;

namespace ZDeals.Api.Mapping
{
    public static class ErrorMapping
    {
        public static ErrorResponse ToErrorResponse(this IEnumerable<Error> errors, int status = 400, string message = null)
        {
            var response = new ErrorResponse() { Status = status, Message = message };

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
