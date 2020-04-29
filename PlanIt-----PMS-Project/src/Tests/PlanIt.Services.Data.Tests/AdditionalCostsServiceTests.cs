namespace PlanIt.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using PlanIt.Common;
    using PlanIt.Data.Models;
    using PlanIt.Web.ViewModels.AdditionalCosts;
    using Xunit;

    public class AdditionalCostsServiceTests : BaseServiceTests
    {
        private const string DateTimeFormat = "MM/dd/yy H:mm";

        private IAdditionalCostsService AdditionalCostsService => this.ServiceProvider.GetRequiredService<IAdditionalCostsService>();

        [Fact]
        public async Task SumAdditionalCostsCompletedProjectsByManagerIdShouldReturnCorrectNumber()
        {
            // Arrange
            var costs = new List<AdditionalCost>
            {
                new AdditionalCost
                {
                    Project = new Project
                    {
                        ProjectManagerId = "First",
                        ProgressStatus = new ProgressStatus
                        {
                            Name = GlobalConstants.ProgressStatusCompleted,
                        },
                        IsDeleted = false,
                    },
                    TotalCost = 20,
                },
                new AdditionalCost
                {
                    Project = new Project
                    {
                        ProjectManagerId = "First",
                        ProgressStatus = new ProgressStatus
                        {
                            Name = GlobalConstants.ProgressStatusCompleted,
                        },
                        IsDeleted = false,
                    },
                    TotalCost = 20,
                },
                new AdditionalCost
                {
                    Project = new Project
                    {
                        ProjectManagerId = "Second",
                        ProgressStatus = new ProgressStatus
                        {
                            Name = GlobalConstants.ProgressStatusCompleted,
                        },
                        IsDeleted = false,
                    },
                    TotalCost = 20,
                },
            };

            await this.PlanItDbContext.AdditionalCosts.AddRangeAsync(costs);
            await this.PlanItDbContext.SaveChangesAsync();

            var userId = "First";

            // Act
            var expected = 40;

            var actual = this.AdditionalCostsService
                .SumAdditionalCostsCompletedProjectsByManagerId(userId);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task AddAsyncShouldAddCostCorrectly()
        {
            // Arrange
            var inputModel = new AddCostAddToSubProjectInputModel
            {
                Name = "Print",
                Description = "Drawings",
                PricePerQuantity = 5.50M,
                Quantity = 20,
                ProjectId = 1,
                SubProjectId = 1,
            };

            // Act
            var expected = new AdditionalCost
            {
                Name = "Print",
                Description = "Drawings",
                PricePerQuantity = 5.50M,
                Quantity = 20,
                TotalCost = 5.5M * 20,
                ProjectId = 1,
                SubProjectId = 1,
            };

            var actual = await this.AdditionalCostsService
                .AddAsync<AddCostAddToSubProjectInputModel>(inputModel);

            // Assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.PricePerQuantity, actual.PricePerQuantity);
            Assert.Equal(expected.Quantity, actual.Quantity);
            Assert.Equal(expected.TotalCost, actual.TotalCost);
            Assert.Equal(expected.ProjectId, actual.ProjectId);
            Assert.Equal(expected.SubProjectId, actual.SubProjectId);
        }

        [Fact]
        public async Task GetByIdAsyncShouldReturnCorrectCost()
        {
            // Arrange
            var costs = new List<AdditionalCost>
            {
                new AdditionalCost
                {
                    Id = 1,
                    ProjectId = 2,
                },
                new AdditionalCost
                {
                    Id = 2,
                    ProjectId = 3,
                },
            };

            await this.PlanItDbContext.AdditionalCosts.AddRangeAsync(costs);
            await this.PlanItDbContext.SaveChangesAsync();

            var addCostId = 1;

            // Act
            var expected = new AddCostViewModel
            {
                ProjectId = 2,
            };

            var actual = await this.AdditionalCostsService
                .GetByIdAsync<AddCostViewModel>(addCostId);

            // Assert
            Assert.Equal(expected.ProjectId, actual.ProjectId);
        }

        [Fact]
        public async Task EditAsyncShouldEditCostCorrectly()
        {
            // Arrange
            var inputModel = new AddCostEditInputModel
            {
                Id = 1,
                Name = "Print",
                Description = "Drawings",
                PricePerQuantity = 5.50M,
                Quantity = 20,
            };

            var cost = new AdditionalCost
            {
                Id = 1,
                Name = "Print 2",
                Description = "Drawings Many",
                PricePerQuantity = 10.50M,
                Quantity = 20,
            };

            await this.PlanItDbContext.AdditionalCosts.AddAsync(cost);
            await this.PlanItDbContext.SaveChangesAsync();

            // Act
            var expected = new AdditionalCost
            {
                Id = 1,
                Name = "Print",
                Description = "Drawings",
                PricePerQuantity = 5.50M,
                Quantity = 20,
                TotalCost = 5.5M * 20,
            };

            var actual = await this.AdditionalCostsService
                .EditAsync<AddCostEditInputModel>(inputModel);

            // Assert
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.PricePerQuantity, actual.PricePerQuantity);
            Assert.Equal(expected.Quantity, actual.Quantity);
            Assert.Equal(expected.TotalCost, actual.TotalCost);
        }

        [Fact]
        public async Task DeleteAsyncShouldDeleteCost()
        {
            // Arrange
            var costs = new List<AdditionalCost>
            {
                new AdditionalCost
                {
                    Id = 1,
                    ProjectId = 2,
                },
            };

            await this.PlanItDbContext.AdditionalCosts.AddRangeAsync(costs);
            await this.PlanItDbContext.SaveChangesAsync();

            var addCostId = 1;

            // Act
            var expectedCount = 0;

            await this.AdditionalCostsService
                .DeleteAsync(addCostId);

            // Assert
            Assert.Equal(expectedCount, this.PlanItDbContext.AdditionalCosts.Count());
        }
    }
}
