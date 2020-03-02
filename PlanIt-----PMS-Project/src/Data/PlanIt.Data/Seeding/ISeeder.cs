namespace PlanIt.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    public interface ISeeder
    {
        Task SeedAsync(PlanItDbContext dbContext, IServiceProvider serviceProvider);
    }
}
