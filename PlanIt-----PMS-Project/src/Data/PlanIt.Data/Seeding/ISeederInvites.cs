namespace PlanIt.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    public interface ISeederInvites
    {
        Task SeedAsync(InvitesDbContext dbContext, IServiceProvider serviceProvider);
    }
}
