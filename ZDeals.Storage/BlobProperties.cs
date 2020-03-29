using System.Collections.Generic;

namespace ZDeals.Storage
{
    public class BlobProperties
    {
        public static readonly BlobProperties Empty;

        public BlobSecurity Security { get; set; }
        public string ContentType { get; set; }
        public IDictionary<string, string> Metadata { get; set; }
    }
}
