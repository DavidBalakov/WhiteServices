using Diploma.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Data.Seed
{
    class ProductSeed
    {
        public static void Initialize(IServiceProvider serviceProvider, string adminId)
        {

            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))

                
                if (!context.Products.Any())
                {
                    context.Products.AddRange(
                    new Product
                    {
                        SerialNumber = "1",
                        Brand = "HAJ-89",
                        Model = "Toshiba",
                        UserId = adminId
                    },

                    new Product
                    {
                        SerialNumber = "2",
                        Brand = "HAJ-96",
                        Model = "Yamaha",
                        UserId = adminId
                    },

                    new Product
                    {
                        SerialNumber = "3",
                        Brand = "HAJ-140",
                        Model = "Veco",
                        UserId = adminId
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
