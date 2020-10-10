namespace ZDeals.Api.Service
{
    public interface IRequestContextProvider
    {
        RequestContext Context { get; }
    }

    public struct RequestContext
    {
        public string Username { get; private set; }

        public RequestContext(string username)
        {
            Username = username;
        }
    }
}
