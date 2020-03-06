namespace PlanIt.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using PlanIt.Data.Models;

    internal class SubProjectTypesSeeder : ISeeder
    {
        public async Task SeedAsync(PlanItDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.SubProjectTypes.Any())
            {
                return;
            }

            await dbContext.SubProjectTypes.AddRangeAsync(
                new SubProjectType { Name = "Architecture" },
                new SubProjectType { Name = "Contruction" },
                new SubProjectType { Name = "Electrical" },
                new SubProjectType { Name = "Hvac" },
                new SubProjectType { Name = "Water and Sewage" },
                new SubProjectType { Name = "Geodesy" },
                new SubProjectType { Name = "Geology" },
                new SubProjectType { Name = "Fire Safety" },
                new SubProjectType { Name = "Fire instalation" },
                new SubProjectType { Name = "Landscape" });
        }
    }
}
