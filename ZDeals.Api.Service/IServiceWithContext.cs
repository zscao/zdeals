using System;
using System.Collections.Generic;
using System.Text;

namespace ZDeals.Api.Service
{
    public interface IServiceWithContext
    {
        RequestContext RequestContext { get; set; }
    }

    public class RequestContext
    {
        public string Username { get; set; }
    }
}
