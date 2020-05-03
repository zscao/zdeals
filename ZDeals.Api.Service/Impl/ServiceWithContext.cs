namespace ZDeals.Api.Service.Impl
{
    public class ServiceWithContext : IServiceWithContext
    {
        private RequestContext _context = null;
        public RequestContext RequestContext { get => _context; set => _context = value; }
    }
}
