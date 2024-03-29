﻿using System.Collections.Generic;

namespace ZDeals.Common.AspNetCore.Responses
{
    public class ErrorResponse
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public List<ErrorDetail> Errors { get; set; }
    }


    public class ErrorDetail
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
