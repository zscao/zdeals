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

    public struct RequestContext
    {
        public string SessionToken { get; private set; }
        public string SessionId { get; private set; }

        public RequestContext(string sessionToken, string sessionId)
        {
            SessionToken = sessionToken;
            SessionId = sessionId;
        }
    }
}
