using Diploma.Data.Seed;

namespace Diploma.Data
{
    class DatabaseInitializer
    {
        public static async Task Initialize(IApplicationBuilder app)
        {
            await RolesSeed.Seed(app);
        }
    }
}