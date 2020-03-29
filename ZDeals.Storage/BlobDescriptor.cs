using System;
using System.Collections.Generic;

namespace ZDeals.Storage
{
    public class BlobDescriptor
    {
        public string ContentType { get; set; }
        public long Length { get; set; }
        public DateTimeOffset? LastModified { get; set; }
        public BlobSecurity Security { get; set; }
        public string Name { get; set; }
        public string Container { get; set; }
        public string Url { get; set; }
        public IDictionary<string, string> Metadata { get; set; }
    }
}
