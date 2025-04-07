using Diploma.Entities;
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
                        Brand = "Toshiba",
                        Model = "HAJ-96",
                        UserId = adminId
                    },

                    new Product
                    {
                        SerialNumber = "2",
                        Brand = "Yamaha",
                        Model = "HAJ-96",
                        UserId = adminId
                    },

                    new Product
                    {
                        SerialNumber = "3",
                        Brand = "Veco",
                        Model = "HAJ-140",
                        UserId = adminId
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
