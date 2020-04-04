using System;
using System.Collections.Generic;
using System.Text;

namespace ZDeals.Api.Contract.Requests
{
    public class SaveDealCategoriesRequest
    {
        public List<int> Categories { get; set; }
    }
}
