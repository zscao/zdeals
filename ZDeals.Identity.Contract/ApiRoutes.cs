namespace ZDeals.Identity.Contract
{
    public static class ApiRoutes
    {
        public const string BaseUrl = "api/";

        public static class Error
        {
            public const string Production = BaseUrl + "error";
            public const string Development = BaseUrl + "error-development";
        }

        public static class Users
        {
            public const string Base = BaseUrl + "users";
        }
    }
}
