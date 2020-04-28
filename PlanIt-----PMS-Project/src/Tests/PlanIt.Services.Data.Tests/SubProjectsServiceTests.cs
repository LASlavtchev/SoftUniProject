namespace PlanIt.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using PlanIt.Common;
    using PlanIt.Data.Models;
    using PlanIt.Web.ViewModels.SubProjects;
    using PlanIt.Web.ViewModels.Tasks;
    using Xunit;

    public class SubProjectsServiceTests : BaseServiceTests
    {
        private const string DateTimeFormat = "MM/dd/yy H";

        private ISubProjectsService SubProjectsService => this.ServiceProvider.GetRequiredService<ISubProjectsService>();

        [Fact]
        public async Task CreateAsyncWithNullDueDateShouldReturnCorrectSubProject()
        {
            // Arrange
            var project = new Project
            {
                Id = 2,
                ProgressStatusId = 2,
                DueDate = DateTime.UtcNow,
            };

            await this.PlanItDbContext.Projects.AddAsync(project);
            await this.PlanItDbContext.SaveChangesAsync();

            var inputModel = new SubProjectCreateInputModel
            {
                SubProjectTypeId = 1,
                ProjectId = 2,
                Budget = 3000,
                DueDate = null,
            };

            // Act
            var expected = new SubProject
            {
                SubProjectTypeId = 1,
                ProjectId = 2,
                Budget = 3000,
                DueDate = DateTime.UtcNow,
                ProgressStatusId = 2,
            };

            var actual = await this.SubProjectsService
                .CreateAsync<SubProjectCreateInputModel>(inputModel);

            // Assert
            Assert.Equal(expected.SubProjectTypeId, actual.SubProjectTypeId);
            Assert.Equal(expected.ProjectId, actual.ProjectId);
            Assert.Equal(expected.Budget, actual.Budget);
            Assert.Equal(expected.DueDate?.ToString(DateTimeFormat), actual.DueDate?.ToString(DateTimeFormat));
            Assert.Equal(expected.ProgressStatusId, actual.ProgressStatusId);
        }

        [Fact]
        public async Task CreateAsyncWithDueDateShouldReturnCorrectSubProject()
        {
            // Arrange
            var project = new Project
            {
                Id = 2,
                ProgressStatusId = 2,
            };

            await this.PlanItDbContext.Projects.AddAsync(project);
            await this.PlanItDbContext.SaveChangesAsync();

            var inputModel = new SubProjectCreateInputModel
            {
                SubProjectTypeId = 1,
                ProjectId = 2,
                Budget = 3000,
                DueDate = DateTime.Now,
            };

            // Act
            var expected = new SubProject
            {
                SubProjectTypeId = 1,
                ProjectId = 2,
                Budget = 3000,
                DueDate = DateTime.UtcNow,
                ProgressStatusId = 2,
            };

            var actual = await this.SubProjectsService
                .CreateAsync<SubProjectCreateInputModel>(inputModel);

            // Assert
            Assert.Equal(expected.SubProjectTypeId, actual.SubProjectTypeId);
            Assert.Equal(expected.ProjectId, actual.ProjectId);
            Assert.Equal(expected.Budget, actual.Budget);
            Assert.Equal(expected.DueDate?.ToString(DateTimeFormat), actual.DueDate?.ToString(DateTimeFormat));
            Assert.Equal(expected.ProgressStatusId, actual.ProgressStatusId);
        }

        [Fact]
        public async Task GetByIdAsyncShouldReturnCorrectSubProject()
        {
            // Arrange
            var project = new Project
            {
                Id = 1,
                IsDeleted = false,
                SubProjects = new List<SubProject>
                {
                    new SubProject
                    {
                        Id = 1,
                    },
                    new SubProject
                    {
                        Id = 2,
                    },
                },
            };

            await this.PlanItDbContext.Projects.AddAsync(project);
            await this.PlanItDbContext.SaveChangesAsync();

            var subProjectId = 1;

            // Act
            var expected = new SubProjectDeleteViewModel
            {
                Id = 1,
            };

            var actual = await this.SubProjectsService
                .GetByIdAsync<SubProjectDeleteViewModel>(subProjectId);

            // Assert
            Assert.Equal(expected.Id, actual.Id);
        }

        [Fact]
        public async Task EditAsyncShouldReturnCorrectSubProject()
        {
            // Arrange
            var editModel = new SubProjectEditInputModel
            {
                Id = 1,
                Budget = 5000,
                ProgressStatusId = 3,
                DueDate = DateTime.Now.AddDays(20),
            };

            var subProject = new SubProject
            {
                Id = 1,
                Budget = 1000,
                ProgressStatusId = 2,
                DueDate = DateTime.Now.AddDays(10),
            };

            await this.PlanItDbContext.SubProjects.AddAsync(subProject);
            await this.PlanItDbContext.SaveChangesAsync();

            // Act
            var expected = new SubProjectEditInputModel
            {
                Id = 1,
                Budget = 5000,
                ProgressStatusId = 3,
                DueDate = DateTime.UtcNow.AddDays(20),
            };

            var actual = await this.SubProjectsService
                .EditAsync<SubProjectEditInputModel>(editModel);

            // Assert
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.ProgressStatusId, actual.ProgressStatusId);
            Assert.Equal(expected.DueDate?.ToString(DateTimeFormat), actual.DueDate?.ToString(DateTimeFormat));
        }

        [Fact]
        public async Task DeleteAsyncShouldDeletetheCorrectSubProject()
        {
            // Arrange
            var subProject = new SubProject
            {
                Id = 1,
                Budget = 1000,
                ProgressStatusId = 2,
                DueDate = DateTime.Now.AddDays(10),
            };

            await this.PlanItDbContext.SubProjects.AddAsync(subProject);
            await this.PlanItDbContext.SaveChangesAsync();

            var subProjectId = 1;

            // Act
            var expected = 0;

            await this.SubProjectsService.DeleteAsync(subProjectId);

            // Assert
            Assert.Equal(expected, this.PlanItDbContext.SubProjects.Count());
        }

        [Fact]
        public async Task ChangeStatusAsyncWithOneProblemNotCompletedShouldNotChangeStatus()
        {
            // Arrange
            var subProject = new SubProject
            {
                Id = 1,
                ProgressStatus = new ProgressStatus
                {
                    Name = GlobalConstants.ProgressStatusAssigned,
                },
                Project = new Project
                {
                    IsDeleted = false,
                },
            };

            await this.PlanItDbContext.SubProjects.AddAsync(subProject);

            var problems = new List<Problem>
            {
                new Problem
                {
                    SubProjectId = 1,
                    ProgressStatus = new ProgressStatus
                    {
                        Name = GlobalConstants.ProgressStatusCompleted,
                    },
                },
                new Problem
                {
                    SubProjectId = 1,
                    ProgressStatus = new ProgressStatus
                    {
                        Name = GlobalConstants.ProgressStatusInProgress,
                    },
                },
            };

            await this.PlanItDbContext.Problems.AddRangeAsync(problems);

            await this.PlanItDbContext.SaveChangesAsync();

            int subProjectId = 1;

            var status = new ProgressStatus
            {
                Name = GlobalConstants.ProgressStatusCompleted,
            };

            // Act
            var expected = new Project
            {
                Id = 1,
                ProgressStatus = new ProgressStatus
                {
                    Name = GlobalConstants.ProgressStatusAssigned,
                },
            };

            var actual = await this.SubProjectsService
                .ChangeStatusAsync(subProjectId, status);

            // Assert
            Assert.Equal(expected.ProgressStatus.Name, actual.ProgressStatus.Name);
        }

        [Fact]
        public async Task ChangeStatusAsyncWithOneProblemsCompletedShouldChangeStatus()
        {
            // Arrange
            var subProject = new SubProject
            {
                Id = 1,
                ProgressStatus = new ProgressStatus
                {
                    Name = GlobalConstants.ProgressStatusAssigned,
                },
                Project = new Project
                {
                    IsDeleted = false,
                },
            };

            await this.PlanItDbContext.SubProjects.AddAsync(subProject);

            var problems = new List<Problem>
            {
                new Problem
                {
                    SubProjectId = 1,
                    ProgressStatus = new ProgressStatus
                    {
                        Name = GlobalConstants.ProgressStatusCompleted,
                    },
                },
                new Problem
                {
                    SubProjectId = 1,
                    ProgressStatus = new ProgressStatus
                    {
                        Name = GlobalConstants.ProgressStatusCompleted,
                    },
                },
            };

            await this.PlanItDbContext.Problems.AddRangeAsync(problems);

            await this.PlanItDbContext.SaveChangesAsync();

            int subProjectId = 1;

            var status = new ProgressStatus
            {
                Name = GlobalConstants.ProgressStatusCompleted,
            };

            // Act
            var expected = new Project
            {
                Id = 1,
                ProgressStatus = new ProgressStatus
                {
                    Name = GlobalConstants.ProgressStatusCompleted,
                },
            };

            var actual = await this.SubProjectsService
                .ChangeStatusAsync(subProjectId, status);

            // Assert
            Assert.Equal(expected.ProgressStatus.Name, actual.ProgressStatus.Name);
        }
    }
}
