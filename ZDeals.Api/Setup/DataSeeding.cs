using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ZDeals.Data;
using ZDeals.Data.Entities.Sales;
using ZDeals.Identity;

namespace ZDeals.Api.Setup
{
    public static class DataSeeding
    {
        public static async Task<IHost> SeedDataAsync(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            using (var dbContext = scope.ServiceProvider.GetRequiredService<ZDealsDbContext>())
            {
                try
                {
                    if(dbContext.Stores.Any() == false)
                    {
                        dbContext.Stores.AddRange(GetStoreSeedData());
                        await dbContext.SaveChangesAsync();
                    }

                    if(dbContext.Categories.Any() == false)
                    {
                        dbContext.Categories.AddRange(GetCategorySeedData());
                        await dbContext.SaveChangesAsync();
                    }
                    if(dbContext.Users.Any() == false)
                    {
                        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                        await userService.CreateUserAsync(new Contract.Requests.CreateUserRequest
                        {
                            Username = "test",
                            Nickname = "Admin TEST",
                            Password = "Test123!",
                            Role = "Admin"
                        });
                    }
                }
                catch(Exception ex)
                {
                    throw;
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
                            Title = "Games & Toys",
                            Code = "games-n-toyes",
                            Children = new List<CategoryEntity>
                            {
                                new CategoryEntity
                                {
                                    Title = "Board Games",
                                    Code = "board-games",
                                },
                                new CategoryEntity
                                {
                                    Title = "Puzzles",
                                    Code = "puzzles",
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

    }
}
