﻿using System.Collections.Generic;

namespace ZDeals.Api.Contract
{
    public class DataList<T>
    {
        public IEnumerable<T> Items { get; set; }
    }
}
