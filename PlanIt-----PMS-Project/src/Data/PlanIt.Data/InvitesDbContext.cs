namespace PlanIt.Data
{
    using Microsoft.EntityFrameworkCore;
    using PlanIt.Data.Models;

    public class InvitesDbContext : DbContext
    {
        public InvitesDbContext(DbContextOptions<InvitesDbContext> options)
            : base(options)
        {
        }

        public DbSet<Invite> Invites { get; set; }
    }
}
