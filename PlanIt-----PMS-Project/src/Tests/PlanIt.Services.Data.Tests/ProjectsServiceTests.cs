namespace PlanIt.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using PlanIt.Common;
    using PlanIt.Data.Common.Repositories;
    using PlanIt.Data.Models;
    using PlanIt.Web.ViewModels.Client.Projects;
    using Xunit;

    public class ProjectsServiceTests : BaseServiceTests
    {
        private const string DateTimeFormat = "MM/dd/yy H:mm";

        private IProjectsService ProjectsService => this.ServiceProvider.GetRequiredService<IProjectsService>();

        private IDeletableEntityRepository<Project> ProjectRepository =>
            this.ServiceProvider.GetRequiredService<IDeletableEntityRepository<Project>>();

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

        [Fact]
        public async Task GetAllAsyncShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingProjectsToDb();

            // Act
            var expected = 5;

            var projects = await this.ProjectsService.GetAllAsync<ProjectViewModel>();
            var actual = projects.Count();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetAllByClientIdAsyncShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingProjectsToDb();

            var clientId = 1;

            // Act
            var expected = 2;

            var projects = await this.ProjectsService.GetAllByClientIdAsync<ProjectViewModel>(clientId);
            var actual = projects.Count();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetAllApprovedByClientIdAsyncShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingProjectsToDb();

            var clientId = 1;

            // Act
            var expected = 2;

            var projects = await this.ProjectsService.GetAllApprovedByClientIdAsync<ProjectViewModel>(clientId);
            var actual = projects.Count();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetAllNotApprovedByClientIdAsyncShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingProjectsToDb();

            var clientId = 1;

            // Act
            var expected = 0;

            var projects = await this.ProjectsService.GetAllNotApprovedByClientIdAsync<ProjectViewModel>(clientId);
            var actual = projects.Count();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetAllByManagerIdAsyncWithViewModelShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingProjectsToDb();

            var managerId = "First";

            // Act
            var expected = 2;

            var projects = await this.ProjectsService
                .GetAllByManagerIdAsync<ProjectViewModel>(managerId);
            var actual = projects.Count();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetAllByManagerIdAsyncShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingProjectsToDb();

            var managerId = "First";

            // Act
            var expected = 2;

            var projects = this.ProjectsService
                .GetAllByManagerId(managerId);
            var actual = projects.Count();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetByIdAsyncWithModelShouldReturnCorrectProject()
        {
            // Arrange
            var project = new Project
            {
                Id = 1,
                Name = "Project1",
                Budget = 1000,
                DueDate = DateTime.Parse("4/29/2020"),
                ClientBudget = 2000.20M,
                ClientDueDate = DateTime.Parse("4/27/2020"),
            };

            await this.PlanItDbContext.Projects.AddAsync(project);
            await this.PlanItDbContext.SaveChangesAsync();

            var id = 1;

            // Act
            var expected = new ProjectViewModel
            {
                Id = 1,
                Name = "Project1",
                Budget = 1000,
                DueDate = DateTime.Parse("4/29/2020"),
                ClientBudget = 2000.20M,
                ClientDueDate = DateTime.Parse("4/27/2020"),
            };

            var actual = await this.ProjectsService.GetByIdAsync<ProjectViewModel>(id);

            // Assert
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.DueDate, actual.DueDate);
            Assert.Equal(expected.ClientBudget, actual.ClientBudget);
            Assert.Equal(expected.ClientDueDate, actual.ClientDueDate);
        }

        [Fact]
        public async Task GetByIdAsyncShouldReturnCorrectProject()
        {
            // Arrange
            var project = new Project
            {
                Id = 1,
                Name = "Project1",
                Budget = 1000,
                DueDate = null,
                ClientBudget = 2000.20M,
                ClientDueDate = DateTime.Parse("4/27/2020"),
            };

            await this.PlanItDbContext.Projects.AddAsync(project);
            await this.PlanItDbContext.SaveChangesAsync();

            var id = 1;

            // Act
            var expected = new Project
            {
                Id = 1,
                Name = "Project1",
                Budget = 1000,
                DueDate = null,
                ClientBudget = 2000.20M,
                ClientDueDate = DateTime.Parse("4/27/2020"),
            };

            var actual = await this.ProjectsService.GetByIdAsync(id);

            // Assert
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Null(actual.DueDate);
            Assert.Equal(expected.ClientBudget, actual.ClientBudget);
            Assert.Equal(expected.ClientDueDate, actual.ClientDueDate);
        }

        [Fact]
        public async Task GetDeletedByIdAsyncShouldReturnCorrectProject()
        {
            // Arrange
            var project = new Project
            {
                Id = 1,
                Name = "Deleted",
                IsDeleted = true,
            };

            await this.PlanItDbContext.Projects.AddAsync(project);
            await this.PlanItDbContext.SaveChangesAsync();

            var id = 1;

            // Act
            var expected = new ProjectViewModel
            {
                Id = 1,
                Name = "Deleted",
            };

            var actual = await this.ProjectsService
                .GetDeletedByIdAsync<ProjectViewModel>(id);

            // Assert
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
        }

        [Fact]
        public async Task CreateByClientAsyncShouldCreateProjectCorrectly()
        {
            // Arrange
            var inputProject = new ProjectCreateInputModel
            {
                Name = "New",
                ClientBudget = 200,
                ClientDueDate = DateTime.Now,
                ClientId = 1,
            };

            var status = new ProgressStatus
            {
                Name = "Not Assigned",
            };

            await this.PlanItDbContext.ProgressStatuses.AddAsync(status);
            await this.PlanItDbContext.SaveChangesAsync();

            // Act
            var expected = new Project
            {
                Name = "New",
                ClientBudget = 200,
                ClientDueDate = DateTime.UtcNow,
                ClientId = 1,
                IsBudgetApproved = false,
                Budget = 0,
                ProgressStatus = new ProgressStatus
                {
                    Name = "Not Assigned",
                },
            };

            var actual = await this.ProjectsService
                .CreateByClientAsync<ProjectCreateInputModel>(inputProject);

            // Assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Budget, actual.Budget);
            Assert.Equal(expected.ClientBudget, actual.ClientBudget);
            Assert.Equal(expected.ClientDueDate.ToString(DateTimeFormat), actual.ClientDueDate.ToString(DateTimeFormat));
            Assert.Equal(expected.ClientId, actual.ClientId);
            Assert.False(actual.IsBudgetApproved);
            Assert.Equal(expected.ProgressStatus.Name, actual.ProgressStatus.Name);
        }

        [Fact]
        public async Task CreateByManagerAsyncShouldCreateProjectCorrectly()
        {
            // Arrange
            var inputProject = new Web.ViewModels.Management.Projects.ProjectCreateInputModel
            {
                Name = "New",
                StartDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(10),
                ClientId = 1,
            };

            var status = new ProgressStatus
            {
                Name = "Not Assigned",
            };

            await this.PlanItDbContext.ProgressStatuses.AddAsync(status);
            await this.PlanItDbContext.SaveChangesAsync();

            // Act
            var expected = new Project
            {
                Name = "New",
                StartDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(10),
                ClientId = 1,
                IsBudgetApproved = false,
                Budget = 0,
                ProgressStatus = new ProgressStatus
                {
                    Name = "Not Assigned",
                },
            };

            var actual = await this.ProjectsService
                .CreateByManagerAsync<Web.ViewModels.Management.Projects.ProjectCreateInputModel>(inputProject);

            // Assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Budget, actual.Budget);
            Assert.Equal(expected.ClientBudget, actual.ClientBudget);
            Assert.Equal(expected.StartDate?.ToString(DateTimeFormat), actual.StartDate?.ToString(DateTimeFormat));
            Assert.Equal(expected.DueDate?.ToString(DateTimeFormat), actual.DueDate?.ToString(DateTimeFormat));
            Assert.Equal(expected.ClientId, actual.ClientId);
            Assert.False(actual.IsBudgetApproved);
            Assert.Equal(expected.ProgressStatus.Name, actual.ProgressStatus.Name);
        }

        [Fact]
        public async Task ApproveAsyncWithNullDueDateShouldEditProjectCorrectly()
        {
            // Arrange
            var notApprovedProject = new Project
            {
                Id = 1,
                Name = "Project",
                StartDate = null,
                DueDate = null,
                ClientBudget = 1000,
                Budget = 2000,
                IsBudgetApproved = false,
                ClientDueDate = DateTime.UtcNow.AddDays(20),
            };

            var status = new ProgressStatus
            {
                Id = 1,
                Name = GlobalConstants.ProgressStatusInProgress,
            };

            var subProject = new SubProject
            {
                Id = 1,
                Project = notApprovedProject,
            };

            var problem = new Problem
            {
                SubProject = subProject,
            };

            await this.PlanItDbContext.Projects.AddAsync(notApprovedProject);
            await this.PlanItDbContext.ProgressStatuses.AddAsync(status);
            await this.PlanItDbContext.SubProjects.AddAsync(subProject);
            await this.PlanItDbContext.Problems.AddAsync(problem);
            await this.PlanItDbContext.SaveChangesAsync();

            var projectId = 1;

            // Act
            var expected = new Project
            {
                Name = "Project",
                Budget = 2000,
                ClientBudget = notApprovedProject.Budget,
                StartDate = DateTime.UtcNow,
                DueDate = notApprovedProject.ClientDueDate,
                IsBudgetApproved = true,
                ProgressStatus = new ProgressStatus
                {
                    Name = GlobalConstants.ProgressStatusInProgress,
                },
            };

            var actual = await this.ProjectsService
                .ApproveAsync(projectId);
            var actualSubProject = actual.SubProjects.First();
            var actualProblem = actualSubProject.Problems.First();

            // Assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Budget, actual.Budget);
            Assert.Equal(expected.ClientBudget, actual.ClientBudget);
            Assert.Equal(expected.StartDate?.ToString(DateTimeFormat), actual.StartDate?.ToString(DateTimeFormat));
            Assert.Equal(expected.DueDate?.ToString(DateTimeFormat), actual.DueDate?.ToString(DateTimeFormat));
            Assert.True(actual.IsBudgetApproved);
            Assert.Equal(expected.ProgressStatus.Name, actual.ProgressStatus.Name);
            Assert.True(actualSubProject.ProgressStatus.Name == GlobalConstants.ProgressStatusInProgress);
            Assert.True(actualProblem.ProgressStatus.Name == GlobalConstants.ProgressStatusInProgress);
        }

        [Fact]
        public async Task ApproveAsyncWithBiggerDueDateShouldEditProjectCorrectly()
        {
            // Arrange
            var notApprovedProject = new Project
            {
                Id = 1,
                Name = "Project",
                StartDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(20),
                ClientBudget = 1000,
                Budget = 2000,
                IsBudgetApproved = false,
                ClientDueDate = DateTime.UtcNow.AddDays(10),
            };

            var status = new ProgressStatus
            {
                Id = 1,
                Name = GlobalConstants.ProgressStatusInProgress,
            };

            var subProject = new SubProject
            {
                Id = 1,
                Project = notApprovedProject,
            };

            var problem = new Problem
            {
                SubProject = subProject,
            };

            await this.PlanItDbContext.Projects.AddAsync(notApprovedProject);
            await this.PlanItDbContext.ProgressStatuses.AddAsync(status);
            await this.PlanItDbContext.SubProjects.AddAsync(subProject);
            await this.PlanItDbContext.Problems.AddAsync(problem);
            await this.PlanItDbContext.SaveChangesAsync();

            var projectId = 1;

            // Act
            var expected = new Project
            {
                Name = "Project",
                Budget = 2000,
                ClientBudget = notApprovedProject.Budget,
                StartDate = DateTime.UtcNow,
                ClientDueDate = (DateTime)notApprovedProject.DueDate,
                IsBudgetApproved = true,
                ProgressStatus = new ProgressStatus
                {
                    Name = GlobalConstants.ProgressStatusInProgress,
                },
            };

            var actual = await this.ProjectsService
                .ApproveAsync(projectId);
            var actualSubProject = actual.SubProjects.First();
            var actualProblem = actualSubProject.Problems.First();

            // Assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Budget, actual.Budget);
            Assert.Equal(expected.ClientBudget, actual.ClientBudget);
            Assert.Equal(expected.StartDate?.ToString(DateTimeFormat), actual.StartDate?.ToString(DateTimeFormat));
            Assert.Equal(expected.ClientDueDate.ToString(DateTimeFormat), actual.ClientDueDate.ToString(DateTimeFormat));
            Assert.True(actual.IsBudgetApproved);
            Assert.Equal(expected.ProgressStatus.Name, actual.ProgressStatus.Name);
            Assert.True(actualSubProject.ProgressStatus.Name == GlobalConstants.ProgressStatusInProgress);
            Assert.True(actualProblem.ProgressStatus.Name == GlobalConstants.ProgressStatusInProgress);
        }

        [Fact]
        public async Task ApproveAsyncWithSmallerDueDateShouldEditProjectCorrectly()
        {
            // Arrange
            var notApprovedProject = new Project
            {
                Id = 1,
                Name = "Project",
                StartDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(9),
                ClientBudget = 1000,
                Budget = 2000,
                IsBudgetApproved = false,
                ClientDueDate = DateTime.UtcNow.AddDays(10),
            };

            var status = new ProgressStatus
            {
                Id = 1,
                Name = GlobalConstants.ProgressStatusInProgress,
            };

            var subProject = new SubProject
            {
                Id = 1,
                Project = notApprovedProject,
            };

            var problem = new Problem
            {
                SubProject = subProject,
            };

            await this.PlanItDbContext.Projects.AddAsync(notApprovedProject);
            await this.PlanItDbContext.ProgressStatuses.AddAsync(status);
            await this.PlanItDbContext.SubProjects.AddAsync(subProject);
            await this.PlanItDbContext.Problems.AddAsync(problem);
            await this.PlanItDbContext.SaveChangesAsync();

            var projectId = 1;

            // Act
            var expected = new Project
            {
                Name = "Project",
                Budget = 2000,
                ClientBudget = notApprovedProject.Budget,
                StartDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(9),
                ClientDueDate = DateTime.UtcNow.AddDays(10),
                IsBudgetApproved = true,
                ProgressStatus = new ProgressStatus
                {
                    Name = GlobalConstants.ProgressStatusInProgress,
                },
            };

            var actual = await this.ProjectsService
                .ApproveAsync(projectId);
            var actualSubProject = actual.SubProjects.First();
            var actualProblem = actualSubProject.Problems.First();

            // Assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Budget, actual.Budget);
            Assert.Equal(expected.ClientBudget, actual.ClientBudget);
            Assert.Equal(expected.StartDate?.ToString(DateTimeFormat), actual.StartDate?.ToString(DateTimeFormat));
            Assert.Equal(expected.DueDate?.ToString(DateTimeFormat), actual.DueDate?.ToString(DateTimeFormat));
            Assert.Equal(expected.ClientDueDate.ToString(DateTimeFormat), actual.ClientDueDate.ToString(DateTimeFormat));
            Assert.True(actual.IsBudgetApproved);
            Assert.Equal(expected.ProgressStatus.Name, actual.ProgressStatus.Name);
            Assert.True(actualSubProject.ProgressStatus.Name == GlobalConstants.ProgressStatusInProgress);
            Assert.True(actualProblem.ProgressStatus.Name == GlobalConstants.ProgressStatusInProgress);
        }

        [Fact]
        public async Task AssignManagerAsyncShouldAssignProjectManager()
        {
            // Arrange
            var project = new Project
            {
                Id = 1,
            };

            var status = new ProgressStatus
            {
                Name = GlobalConstants.ProgressStatusAssigned,
            };

            var manager = new PlanItUser
            {
                Id = "Manager",
            };

            await this.PlanItDbContext.Projects.AddAsync(project);
            await this.PlanItDbContext.ProgressStatuses.AddAsync(status);
            await this.PlanItDbContext.Users.AddAsync(manager);
            await this.PlanItDbContext.SaveChangesAsync();

            var projectId = 1;
            var managerId = "Manager";

            // Act
            var expected = new Project
            {
                ProjectManagerId = "Manager",
                ProgressStatus = new ProgressStatus
                {
                    Name = GlobalConstants.ProgressStatusAssigned,
                },
            };

            var actual = await this.ProjectsService
                .AssignManagerAsync(projectId, managerId);

            // Assert
            Assert.Equal(expected.ProjectManagerId, actual.ProjectManagerId);
            Assert.Equal(expected.ProgressStatus.Name, actual.ProgressStatus.Name);
        }

        [Fact]
        public async Task DeleteAsyncShouldDeleteProject()
        {
            // Arrange
            var project = new Project
            {
                Id = 1,
            };

            var status = new ProgressStatus
            {
                Name = GlobalConstants.ProgressStatusCanceled,
            };

            await this.PlanItDbContext.Projects.AddAsync(project);
            await this.PlanItDbContext.ProgressStatuses.AddAsync(status);
            await this.PlanItDbContext.SaveChangesAsync();

            var projectId = 1;

            // Act
            var expected = new Project
            {
                IsDeleted = true,
                ProgressStatus = new ProgressStatus
                {
                    Name = GlobalConstants.ProgressStatusCanceled,
                },
            };

            await this.ProjectsService.DeleteAsync(projectId);
            var actual = this.ProjectRepository.AllWithDeleted().First();

            // Assert
            Assert.True(actual.IsDeleted);
            Assert.Equal(expected.ProgressStatus.Name, actual.ProgressStatus.Name);
        }

        [Fact]
        public async Task RestoreAsyncShouldRestoreProject()
        {
            // Arrange
            var project = new Project
            {
                Id = 1,
                IsDeleted = true,
            };

            var status = new ProgressStatus
            {
                Name = GlobalConstants.ProgressStatusSuspended,
            };

            await this.PlanItDbContext.Projects.AddAsync(project);
            await this.PlanItDbContext.ProgressStatuses.AddAsync(status);
            await this.PlanItDbContext.SaveChangesAsync();

            var projectId = 1;

            // Act
            var expected = new Project
            {
                IsDeleted = false,
                ProgressStatus = new ProgressStatus
                {
                    Name = GlobalConstants.ProgressStatusSuspended,
                },
            };

            await this.ProjectsService.RestoreAsync(projectId);
            var actual = this.ProjectRepository.AllWithDeleted().First();

            // Assert
            Assert.False(actual.IsDeleted);
            Assert.Equal(expected.ProgressStatus.Name, actual.ProgressStatus.Name);
        }

        [Fact]
        public async Task EditByClientAsyncShouldEditProjectCorrectly()
        {
            // Arrange
            var project = new Project
            {
                Id = 1,
                Name = "Project",
                ClientBudget = 1000,
                ClientDueDate = DateTime.UtcNow.AddDays(5),
            };

            var inputModel = new ProjectEditInputModel
            {
                Id = 1,
                Name = "Project1",
                ClientBudget = 2000,
                ClientDueDate = DateTime.Now.AddDays(10),
            };

            await this.PlanItDbContext.Projects.AddAsync(project);
            await this.PlanItDbContext.SaveChangesAsync();

            // Act
            var expected = new Project
            {
                Id = 1,
                Name = "Project1",
                ClientBudget = 2000,
                ClientDueDate = DateTime.UtcNow.AddDays(10),
            };

            var actual = await this.ProjectsService
                .EditByClientAsync<ProjectEditInputModel>(inputModel);

            // Assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.ClientBudget, actual.ClientBudget);
            Assert.Equal(expected.ClientDueDate.ToString(DateTimeFormat), actual.ClientDueDate.ToString(DateTimeFormat));
        }

        [Fact]
        public async Task EditByManagerAsyncWithNullManagerAndEqualDatesShouldEditProjectCorrectly()
        {
            // Arrange
            var inputModel = new PlanIt.Web.ViewModels.Management.Projects.ProjectEditInputModel
            {
                Id = 1,
                Name = "Project1",
                StartDate = DateTime.Parse("04-28-2020").ToLocalTime(),
                DueDate = DateTime.Parse("04-30-2020").ToLocalTime(),
                ProgressStatusId = 2,
                ProjectManagerId = null,
            };

            var project = new Project
            {
                Id = 1,
                Name = "Project2",
                StartDate = DateTime.Parse("04-28-2020"),
                DueDate = DateTime.Parse("04-30-2020"),
                ProgressStatusId = 1,
                ProjectManagerId = "First",
                IsBudgetApproved = true,
            };

            await this.PlanItDbContext.Projects.AddAsync(project);
            await this.PlanItDbContext.SaveChangesAsync();

            // Act
            var expected = new Project
            {
                Id = 1,
                Name = "Project1",
                StartDate = DateTime.Parse("04-28-2020"),
                DueDate = DateTime.Parse("04-30-2020"),
                ProgressStatusId = 1,
                ProjectManagerId = "First",
            };

            var actual = await this.ProjectsService
                .EditByManagerAsync<PlanIt.Web.ViewModels.Management.Projects.ProjectEditInputModel>(inputModel);

            // Assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Budget, actual.Budget);
            Assert.Equal(expected.StartDate?.ToString(DateTimeFormat), actual.StartDate?.ToString(DateTimeFormat));
            Assert.Equal(expected.DueDate?.ToString(DateTimeFormat), actual.DueDate?.ToString(DateTimeFormat));
            Assert.Equal(expected.ProgressStatusId, actual.ProgressStatusId);
            Assert.Equal(expected.ProjectManagerId, actual.ProjectManagerId);
            Assert.True(actual.IsBudgetApproved);
        }

        [Fact]
        public async Task EditByManagerAsyncWithNotNullManagerAndEqualDatesShouldEditProjectCorrectly()
        {
            // Arrange
            var inputModel = new PlanIt.Web.ViewModels.Management.Projects.ProjectEditInputModel
            {
                Id = 1,
                Name = "Project1",
                StartDate = DateTime.Parse("04-28-2020").ToLocalTime(),
                DueDate = DateTime.Parse("04-30-2020").ToLocalTime(),
                ProgressStatusId = 2,
                ProjectManagerId = "Second",
            };

            var project = new Project
            {
                Id = 1,
                Name = "Project2",
                StartDate = DateTime.Parse("04-28-2020"),
                DueDate = DateTime.Parse("04-30-2020"),
                ProgressStatusId = 1,
                ProjectManagerId = "First",
                IsBudgetApproved = true,
            };

            await this.PlanItDbContext.Projects.AddAsync(project);
            await this.PlanItDbContext.SaveChangesAsync();

            // Act
            var expected = new Project
            {
                Id = 1,
                Name = "Project1",
                StartDate = DateTime.Parse("04-28-2020"),
                DueDate = DateTime.Parse("04-30-2020"),
                ProgressStatusId = 2,
                ProjectManagerId = "Second",
            };

            var actual = await this.ProjectsService
                .EditByManagerAsync<PlanIt.Web.ViewModels.Management.Projects.ProjectEditInputModel>(inputModel);

            // Assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Budget, actual.Budget);
            Assert.Equal(expected.StartDate?.ToString(DateTimeFormat), actual.StartDate?.ToString(DateTimeFormat));
            Assert.Equal(expected.DueDate?.ToString(DateTimeFormat), actual.DueDate?.ToString(DateTimeFormat));
            Assert.Equal(expected.ProgressStatusId, actual.ProgressStatusId);
            Assert.Equal(expected.ProjectManagerId, actual.ProjectManagerId);
            Assert.True(actual.IsBudgetApproved);
        }

        [Fact]
        public async Task EditByManagerAsyncWithNotNullManagerAndNotEqualStartDateShouldEditProjectCorrectly()
        {
            // Arrange
            var inputModel = new PlanIt.Web.ViewModels.Management.Projects.ProjectEditInputModel
            {
                Id = 1,
                Name = "Project1",
                StartDate = DateTime.Parse("04-29-2020").ToLocalTime(),
                DueDate = DateTime.Parse("04-30-2020").ToLocalTime(),
                ProgressStatusId = 2,
                ProjectManagerId = "Second",
            };

            var project = new Project
            {
                Id = 1,
                Name = "Project2",
                StartDate = DateTime.Parse("04-28-2020"),
                DueDate = DateTime.Parse("04-30-2020"),
                ProgressStatusId = 1,
                ProjectManagerId = "First",
                IsBudgetApproved = true,
            };

            var status = new ProgressStatus
            {
                Id = 5,
                Name = GlobalConstants.ProgressStatusSuspended,
            };

            await this.PlanItDbContext.Projects.AddAsync(project);
            await this.PlanItDbContext.ProgressStatuses.AddAsync(status);
            await this.PlanItDbContext.SaveChangesAsync();

            // Act
            var expected = new Project
            {
                Id = 1,
                Name = "Project1",
                StartDate = DateTime.Parse("04-29-2020"),
                DueDate = DateTime.Parse("04-30-2020"),
                ProgressStatusId = 5,
                ProjectManagerId = "Second",
            };

            var actual = await this.ProjectsService
                .EditByManagerAsync<PlanIt.Web.ViewModels.Management.Projects.ProjectEditInputModel>(inputModel);

            // Assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Budget, actual.Budget);
            Assert.Equal(expected.StartDate?.ToString(DateTimeFormat), actual.StartDate?.ToString(DateTimeFormat));
            Assert.Equal(expected.DueDate?.ToString(DateTimeFormat), actual.DueDate?.ToString(DateTimeFormat));
            Assert.Equal(expected.ProgressStatusId, actual.ProgressStatusId);
            Assert.Equal(expected.ProjectManagerId, actual.ProjectManagerId);
            Assert.False(actual.IsBudgetApproved);
        }

        [Fact]
        public async Task EditByManagerAsyncWithNotNullManagerAndNotEqualDueDateShouldEditProjectCorrectly()
        {
            // Arrange
            var inputModel = new PlanIt.Web.ViewModels.Management.Projects.ProjectEditInputModel
            {
                Id = 1,
                Name = "Project1",
                StartDate = DateTime.Parse("04-28-2020").ToLocalTime(),
                DueDate = DateTime.Parse("05-30-2020").ToLocalTime(),
                ProgressStatusId = 2,
                ProjectManagerId = "Second",
            };

            var project = new Project
            {
                Id = 1,
                Name = "Project2",
                StartDate = DateTime.Parse("04-28-2020"),
                DueDate = DateTime.Parse("04-30-2020"),
                ProgressStatusId = 1,
                ProjectManagerId = "First",
                IsBudgetApproved = true,
            };

            var status = new ProgressStatus
            {
                Id = 5,
                Name = GlobalConstants.ProgressStatusSuspended,
            };

            await this.PlanItDbContext.Projects.AddAsync(project);
            await this.PlanItDbContext.ProgressStatuses.AddAsync(status);
            await this.PlanItDbContext.SaveChangesAsync();

            // Act
            var expected = new Project
            {
                Id = 1,
                Name = "Project1",
                StartDate = DateTime.Parse("04-28-2020"),
                DueDate = DateTime.Parse("05-30-2020"),
                ProgressStatusId = 5,
                ProjectManagerId = "Second",
            };

            var actual = await this.ProjectsService
                .EditByManagerAsync<PlanIt.Web.ViewModels.Management.Projects.ProjectEditInputModel>(inputModel);

            // Assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Budget, actual.Budget);
            Assert.Equal(expected.StartDate?.ToString(DateTimeFormat), actual.StartDate?.ToString(DateTimeFormat));
            Assert.Equal(expected.DueDate?.ToString(DateTimeFormat), actual.DueDate?.ToString(DateTimeFormat));
            Assert.Equal(expected.ProgressStatusId, actual.ProgressStatusId);
            Assert.Equal(expected.ProjectManagerId, actual.ProjectManagerId);
            Assert.False(actual.IsBudgetApproved);
        }

        [Fact]
        public async Task GetAllApprovedAsyncShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingProjectsToDb();

            // Act
            var expected = 4;

            var projects = await this.ProjectsService
                .GetAllApprovedAsync<ProjectViewModel>();

            var actual = projects.Count();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetAllNotApprovedAsyncShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingProjectsToDb();

            // Act
            var expected = 1;

            var projects = await this.ProjectsService
                .GetAllNotApprovedAsync<ProjectViewModel>();

            var actual = projects.Count();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetAllDeletedAsyncShouldReturnCorrectNumber()
        {
            // Arrange
            var projects = new List<Project>
            {
                new Project
                {
                    Id = 1,
                    IsDeleted = true,
                    Client = new Client
                    {
                        Id = 1,
                        IsDeleted = false,
                    },
                },
                new Project
                {
                    Id = 2,
                    IsDeleted = false,
                    Client = new Client
                    {
                        Id = 2,
                        IsDeleted = false,
                    },
                },
                new Project
                {
                    Id = 3,
                    IsDeleted = true,
                    Client = new Client
                    {
                        Id = 3,
                        IsDeleted = true,
                    },
                },
            };

            await this.PlanItDbContext.Projects.AddRangeAsync(projects);
            await this.PlanItDbContext.SaveChangesAsync();

            // Act
            var expected = 1;

            var deletedProjects = await this.ProjectsService
                .GetAllDeletedAsync<ProjectViewModel>();

            var actual = deletedProjects.Count();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task CalculateProjectBudgetWithNotApprovedBudgetShouldEditCorrectlyProject()
        {
            // Arrange
            var project = new Project
            {
                Id = 1,
                IsBudgetApproved = false,
            };

            await this.PlanItDbContext.Projects.AddAsync(project);

            for (int i = 0; i < 3; i++)
            {
                var subProject = new SubProject
                {
                    ProjectId = 1,
                    Budget = 1000,
                };

                await this.PlanItDbContext.SubProjects.AddAsync(subProject);
            }

            await this.PlanItDbContext.SaveChangesAsync();

            int projectId = 1;

            // Act
            var expected = new Project
            {
                Id = 1,
                Budget = 3000,
                IsBudgetApproved = false,
            };

            var actual = await this.ProjectsService.CalculateProjectBudget(projectId);

            // Assert
            Assert.Equal(expected.Budget, actual.Budget);
            Assert.False(actual.IsBudgetApproved);
        }

        [Fact]
        public async Task CalculateProjectBudgetWithApprovedBudgetShouldEditCorrectlyProject()
        {
            // Arrange
            var project = new Project
            {
                Id = 1,
                IsBudgetApproved = true,
            };

            await this.PlanItDbContext.Projects.AddAsync(project);

            for (int i = 0; i < 3; i++)
            {
                var subProject = new SubProject
                {
                    ProjectId = 1,
                    Budget = 1000,
                };

                await this.PlanItDbContext.SubProjects.AddAsync(subProject);
            }

            await this.PlanItDbContext.SaveChangesAsync();

            int projectId = 1;

            // Act
            var expected = new Project
            {
                Id = 1,
                Budget = 3000,
                IsBudgetApproved = false,
            };

            var actual = await this.ProjectsService.CalculateProjectBudget(projectId);

            // Assert
            Assert.Equal(expected.Budget, actual.Budget);
            Assert.False(actual.IsBudgetApproved);
        }

        [Fact]
        public async Task ChangeStatusAsyncWithSubProjectsCompletedShouldChangeStatusToCompleted()
        {
            // Arrange
            var project = new Project
            {
                Id = 1,
                ProgressStatus = new ProgressStatus
                {
                    Name = GlobalConstants.ProgressStatusInProgress,
                },
            };

            await this.PlanItDbContext.Projects.AddAsync(project);

            for (int i = 0; i < 3; i++)
            {
                var subProject = new SubProject
                {
                    ProjectId = 1,
                    ProgressStatus = new ProgressStatus
                    {
                        Name = GlobalConstants.ProgressStatusCompleted,
                    },
                };

                await this.PlanItDbContext.SubProjects.AddAsync(subProject);
            }

            await this.PlanItDbContext.SaveChangesAsync();

            int projectId = 1;

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

            var actual = await this.ProjectsService.ChangeStatusAsync(projectId, status);

            // Assert
            Assert.Equal(expected.ProgressStatus.Name, actual.ProgressStatus.Name);
        }

        [Fact]
        public async Task ChangeStatusAsyncWithOneSubProjectsNotCompletedShouldNotChangeStatus()
        {
            // Arrange
            var project = new Project
            {
                Id = 1,
                ProgressStatus = new ProgressStatus
                {
                    Name = GlobalConstants.ProgressStatusAssigned,
                },
            };

            await this.PlanItDbContext.Projects.AddAsync(project);

            var subProjects = new List<SubProject>
            {
                new SubProject
                {
                    ProjectId = 1,
                    ProgressStatus = new ProgressStatus
                    {
                        Name = GlobalConstants.ProgressStatusCompleted,
                    },
                },
                new SubProject
                {
                    ProjectId = 1,
                    ProgressStatus = new ProgressStatus
                    {
                        Name = GlobalConstants.ProgressStatusInProgress,
                    },
                },
            };

            await this.PlanItDbContext.SubProjects.AddRangeAsync(subProjects);

            await this.PlanItDbContext.SaveChangesAsync();

            int projectId = 1;

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

            var actual = await this.ProjectsService.ChangeStatusAsync(projectId, status);

            // Assert
            Assert.Equal(expected.ProgressStatus.Name, actual.ProgressStatus.Name);
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
                    Client = new Client(),
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
