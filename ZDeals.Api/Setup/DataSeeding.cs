using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ZDeals.Data;
using ZDeals.Data.Entities;

namespace ZDeals.Api.Setup
{
    public static class DataSeeding
    {
        public static async Task<IHost> SeedDataAsync(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            using (var dbContext = scope.ServiceProvider.GetRequiredService<ZDealsDbContext>())
            {
                if (dbContext.Stores.Any() == false)
                {
                    dbContext.Stores.AddRange(GetStoreSeedData());
                    await dbContext.SaveChangesAsync();
                }

                if (dbContext.Categories.Any() == false)
                {
                    dbContext.Categories.AddRange(GetCategorySeedData());
                    await dbContext.SaveChangesAsync();
                }

                if(dbContext.Brands.Any() == false)
                {                    
                    var brands = new List<BrandEntity>
                    {
                        new BrandEntity
                        {
                            Code = "other",
                            Name = "Other",
                            DisplayOrder = 0
                        }
                    };

                    dbContext.Brands.AddRange(brands);
                    await dbContext.SaveChangesAsync();
                }
            }
            return host;
        }

        private static List<CategoryEntity> GetCategorySeedData()
        {
            return new List<CategoryEntity>()
            { 
                new CategoryEntity
                {
                    Title = "Deals",
                    Code = Common.Constants.DefaultValues.DealsCategoryRoot,

                    Children = new List<CategoryEntity>
                    {
                        new CategoryEntity
                        {
                            Title = "Computers",
                            Code = "computers",
                            Children = new List<CategoryEntity>
                            {
                                new CategoryEntity
                                {
                                    Title = "Laptops",
                                    Code = "laptos",
                                },
                                new CategoryEntity
                                {
                                    Title = "Desktops",
                                    Code = "desktops",
                                },
                                new CategoryEntity
                                {
                                    Title = "Tablets",
                                    Code = "tablets",
                                },
                                new CategoryEntity
                                {
                                    Title = "Accessories",
                                    Code = "accessories"
                                }
                            }
                        },
                        new CategoryEntity
                        {
                            Title = "Mobile & Broadband",
                            Code = "mobile-n-broadband",
                            Children = new List<CategoryEntity>
                            {
                                new CategoryEntity
                                {
                                    Title = "Mobile Plans",
                                    Code = "mobile-plans",
                                },
                                new CategoryEntity
                                {
                                    Title = "Broadband",
                                    Code = "broadband",
                                }

                            }
                        },
                        new CategoryEntity
                        {
                            Title = "Electronics",
                            Code = "electronics",
                            Children = new List<CategoryEntity>
                            {
                                new CategoryEntity
                                {
                                    Title = "Cameras",
                                    Code = "cameras"
                                },
                                new CategoryEntity
                                {
                                    Title = "Cell Phones",
                                    Code = "cell-phones"
                                },
                                new CategoryEntity
                                {
                                    Title = "Accessories & Suppliers",
                                    Code = "accessories-suppliers"
                                }
                            }
                        }
                    }
                }
            };
        }

        private static List<StoreEntity> GetStoreSeedData()
        {
            return new List<StoreEntity>
            {
                new StoreEntity
                {
                    Name = "eBay",
                    Website = "https://www.ebay.com.au",
                    Domain = "ebay.com.au"
                },
                new StoreEntity
                {
                    Name = "Amazon",
                    Website = "https://www.amazon.com.au",
                    Domain = "amazon.com.au"
                },
                new StoreEntity
                {
                    Name = "NewEgg",
                    Website = "https://www.newegg.com",
                    Domain = "newegg.com",
                }
            };
        }

        private static List<BrandEntity> GetBrandSeedData()
        {
            return new List<BrandEntity>
            {
                new BrandEntity
                {
                    Name = "Other",
                    DisplayOrder = 0
                }
            };
        }

    }
}
