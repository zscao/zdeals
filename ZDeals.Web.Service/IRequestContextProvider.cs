namespace ZDeals.Web.Service
{

    /// <summary>
    /// provide the request context for the services.
    /// this provider is supposed to be implemented in the web layer
    /// </summary>

    public interface IRequestContextProvider
    {
        RequestContext Context { get;  }
    }

    public class RequestContext
    {
        public string? SessionToken { get; set; }
        public string? SessionId { get; set; }

        public string? ClientIP { get; set; }
    }
}
