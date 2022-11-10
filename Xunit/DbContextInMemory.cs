using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RENT.Data.Context;

namespace Rent.Xunit
{
    public static class DbContextInMemory
    {
        public static AppDbContext GetApplicationDbContext<T>(string dbName, T t) where T : class
        {
            var serviceProvider = new ServiceCollection()
                                  .AddEntityFrameworkInMemoryDatabase()
                                  .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                           .UseInMemoryDatabase(dbName)
                           .UseInternalServiceProvider(serviceProvider)
                           .Options;

            var dbContext = new AppDbContext(options);
            dbContext.SeedData(t);
            return dbContext;
        }

        private static void SeedData<T>(this AppDbContext context, T t) where T : class
        {
            context.Set<T>().AddRange(t);
            context.SaveChanges();
        }
    }
}
