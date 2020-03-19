namespace ZDeals.Api.Contract
{
    public static class ApiRoutes
    {
        public const string BaseUrl = "api/";

        public static class Error
        {
            public const string Production = BaseUrl + "error";
            public const string Development = BaseUrl + "error-development";
        }

        public static class Deals
        {
            public const string SearchDeals = BaseUrl + "deals";
            public const string GetDealById = BaseUrl + "deals/{dealId}";
            public const string CreateDeal = BaseUrl + "deals";
            public const string UpdateDeal = BaseUrl + "deals/{dealId}";
            public const string DeleteDeal = BaseUrl + "deals/{dealId}";

            public const string GetDealStore = BaseUrl + "deals/{dealId}/store";
            public const string UpdateDealStore = BaseUrl + "deals/{dealId}/store/{storeId}";
        }

        public static class Stores
        {
            public const string SearchStores = BaseUrl + "stores";
            public const string GetStoreById = BaseUrl + "stores/{storeId}";
            public const string CreateStore = BaseUrl + "stores";

        }

        public static class Categories
        {
            public const string SearchCategories = BaseUrl + "categories";
            public const string GetCategoryById = BaseUrl + "categories/{categoryId}";
            public const string CreateCategory = BaseUrl + "categories";

        }
    }
}
