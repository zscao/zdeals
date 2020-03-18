using System.Collections.Generic;

namespace ZDeals.Api.Contract.Responses
{
    public class ErrorResponse
    {
        public List<ErrorDetail> Errors { get; set; }
    }


    public class ErrorDetail
    {
        public int Status { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
