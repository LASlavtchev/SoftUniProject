namespace PlanIt.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using PlanIt.Data.Models;
    using Xunit;

    public class ProjectsServiceTests : BaseServiceTests
    {
        private IProjectsService ProjectsService => this.ServiceProvider.GetRequiredService<IProjectsService>();

        private UserManager<PlanItUser> UserManager => this.UserManager;

        [Fact]
        public async Task AllCountShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingProjectsToDb();

            // Act
            var expected = 5;

            var actual = this.ProjectsService.AllCount();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task AllApprovedCountShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingProjectsToDb();

            // Act
            var expected = 4;

            var actual = this.ProjectsService.AllApprovedCount();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task AllCompletedCountShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingProjectsToDb();

            // Act
            var expected = 3;

            var actual = this.ProjectsService.AllCompletedCount();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task AllCountByManagerIdShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingProjectsToDb();
            string managerId = "First";

            // Act
            var expected = 2;

            var actual = this.ProjectsService.AllCountByManagerId(managerId);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task AllCompletedCountByManagerIdShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingProjectsToDb();
            string managerId = "First";

            // Act
            var expected = 1;

            var actual = this.ProjectsService.AllCompletedCountByManagerId(managerId);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task TotalBudgetByManagerIdShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingProjectsToDb();
            string managerId = "First";

            // Act
            var expected = 5000;

            var actual = this.ProjectsService.TotalBudgetByManagerId(managerId);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task CompletedTotalBudgetByManagerIdShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingProjectsToDb();
            string managerId = "First";

            // Act
            var expected = 1000;

            var actual = this.ProjectsService.CompletedTotalBudgetByManagerId(managerId);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task TotalBudgetApprovedShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingProjectsToDb();

            // Act
            var expected = 10000;

            var actual = this.ProjectsService.TotalBudgetApproved();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task TotalBudgetCompletedShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingProjectsToDb();

            // Act
            var expected = 6000;

            var actual = this.ProjectsService.TotalBudgetCompleted();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task TotalCostsCompletedShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingProjectsToDb();
            await this.AddTestingSubProjectToDb();
            await this.AddTestingAdditionalCostsToDb();
            await this.AddTestingProblemsToDb();

            var addCostsProjectsOnly = 1100;
            var addCostsSubProjects = 2000;
            var costs = 500;

            // Act
            var expected = addCostsProjectsOnly + addCostsSubProjects + costs;

            var actual = this.ProjectsService.TotalCostsCompleted();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task AllCountByClientIdShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingProjectsToDb();

            var clientId = 1;

            // Act
            var expected = 2;

            var actual = this.ProjectsService.AllCountByClientId(clientId);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task AllApprovedCountByClientIdShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingProjectsToDb();

            var clientId = 1;

            // Act
            var expected = 2;

            var actual = this.ProjectsService.AllApprovedCountByClientId(clientId);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task AllNotApprovedCountByClientIdShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingProjectsToDb();

            var clientId = 1;

            // Act
            var expected = 0;

            var actual = this.ProjectsService.AllNotApprovedCountByClientId(clientId);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task CalculateApprovedProjectsBudgetByClientIdShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingProjectsToDb();

            var clientId = 1;

            // Act
            var expected = 4000;

            var actual = this.ProjectsService.CalculateApprovedProjectsBudgetByClientId(clientId);

            // Assert
            Assert.Equal(expected, actual);
        }










        private async Task AddTestingProjectsToDb()
        {
            var projects = new List<Project>
            {
                new Project
                {
                    Id = 1,
                    IsBudgetApproved = true,
                    ProgressStatus = new ProgressStatus { Name = "Completed" },
                    ProjectManagerId = "First",
                    Budget = 1000,
                    ClientId = 1,
                },
                new Project
                {
                    Id = 2,
                    IsBudgetApproved = true,
                    ProgressStatus = new ProgressStatus { Name = "Completed" },
                    ProjectManagerId = "Second",
                    Budget = 2000,
                    ClientId = 2,
                },
                new Project
                {
                    Id = 3,
                    IsBudgetApproved = true,
                    ProgressStatus = new ProgressStatus { Name = "Completed" },
                    ProjectManagerId = "Second",
                    Budget = 3000,
                    ClientId = 1,
                },
                new Project
                {
                    Id = 4,
                    IsBudgetApproved = true,
                    ProgressStatus = new ProgressStatus { Name = "In Progress" },
                    ProjectManagerId = "First",
                    Budget = 4000,
                    ClientId = 3,
                },
                new Project
                {
                    Id = 5,
                    IsBudgetApproved = false,
                    ProgressStatus = new ProgressStatus { Name = "Not Assigned" },
                    Budget = 5000,
                    ClientId = 4,
                },
            };

            await this.PlanItDbContext.Projects.AddRangeAsync(projects);
            await this.PlanItDbContext.SaveChangesAsync();
        }

        private async Task AddTestingAdditionalCostsToDb()
        {
            var additionalCosts = new List<AdditionalCost>
            {
                new AdditionalCost
                {
                    Id = 1,
                    ProjectId = 1,
                    TotalCost = 100, // Ok
                },
                new AdditionalCost
                {
                    Id = 2,
                    ProjectId = 2,
                    TotalCost = 200, // Ok
                },
                new AdditionalCost
                {
                    Id = 3,
                    ProjectId = 3,
                    TotalCost = 300, // Ok
                },
                new AdditionalCost
                {
                    Id = 4,
                    ProjectId = 4,
                    TotalCost = 400,
                },
                new AdditionalCost
                {
                    Id = 5,
                    ProjectId = 1,
                    TotalCost = 500, // Ok
                },
                new AdditionalCost
                {
                    Id = 6,
                    ProjectId = 1,
                    SubProjectId = 1,
                    TotalCost = 1000, // Ok
                },
                new AdditionalCost
                {
                    Id = 7,
                    ProjectId = 1,
                    SubProjectId = 1,
                    TotalCost = 1000, // Ok
                },
            };

            await this.PlanItDbContext.AdditionalCosts.AddRangeAsync(additionalCosts);
            await this.PlanItDbContext.SaveChangesAsync();
        }

        private async Task AddTestingSubProjectToDb()
        {
            var subProjects = new List<SubProject>
            {
                new SubProject
                {
                    Id = 1,
                    ProjectId = 1,
                },
                new SubProject
                {
                    Id = 2,
                    ProjectId = 1,
                },
                new SubProject
                {
                    Id = 3,
                    ProjectId = 2,
                },
                new SubProject
                {
                    Id = 4,
                    ProjectId = 4,
                },
            };

            await this.PlanItDbContext.SubProjects.AddRangeAsync(subProjects);
            await this.PlanItDbContext.SaveChangesAsync();
        }

        private async Task AddTestingProblemsToDb()
        {
            var problems = new List<Problem>
            {
                new Problem
                {
                    Id = 1,
                    SubProjectId = 1,
                    HourlyRate = 25,
                    TotalHours = 4, // Ok 100
                },
                new Problem
                {
                    Id = 2,
                    SubProjectId = 1,
                    HourlyRate = 25,
                    TotalHours = 4, // Ok 100
                },
                new Problem
                {
                    Id = 3,
                    SubProjectId = 2,
                    HourlyRate = 25,
                    TotalHours = 4, // Ok 100
                },
                new Problem
                {
                    Id = 4,
                    SubProjectId = 3,
                    HourlyRate = 25,
                    TotalHours = 4, // Ok 100
                },
                new Problem
                {
                    Id = 5,
                    SubProjectId = 3,
                    HourlyRate = 25,
                    TotalHours = 4, // Ok 100
                },
                 new Problem
                {
                    Id = 6,
                    SubProjectId = 4,
                    HourlyRate = 25,
                    TotalHours = 4,
                },
            };

            await this.PlanItDbContext.Problems.AddRangeAsync(problems);
            await this.PlanItDbContext.SaveChangesAsync();
        }







    }
}
