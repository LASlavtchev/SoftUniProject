namespace PlanIt.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using PlanIt.Common;
    using PlanIt.Data.Models;
    using PlanIt.Web.ViewModels.Tasks;
    using Xunit;

    public class ProblemsServiceTests : BaseServiceTests
    {
        private const string DateTimeFormat = "MM/dd/yy H:mm";

        private IProblemsService ProblemsService => this.ServiceProvider.GetRequiredService<IProblemsService>();

        [Fact]
        public async Task AllTasksCountByUserIdProblemWithManyUsersShouldReturnCorretcNumber()
        {
            // Arrange
            await this.AddTestingProblemsToDb();
            var userId = "First";

            // Act
            var expected = 2;

            var actual = this.ProblemsService.AllTasksCountByUserId(userId);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task AllTasksCountByUserIdUserWithManyProblemsShouldReturnCorretcNumber()
        {
            // Arrange
            var user = new PlanItUser
            {
                Id = "First",
                UserProblems = new List<UserProblem>
                {
                    new UserProblem
                    {
                        Problem = new Problem
                        {
                            ProgressStatus = new ProgressStatus
                            {
                                Name = GlobalConstants.ProgressStatusCompleted,
                            },
                        },
                    },
                    new UserProblem
                    {
                        Problem = new Problem
                        {
                            ProgressStatus = new ProgressStatus
                            {
                                Name = GlobalConstants.ProgressStatusInProgress,
                            },
                        },
                    },
                },
            };

            await this.PlanItDbContext.Users.AddAsync(user);
            await this.PlanItDbContext.SaveChangesAsync();

            // Act
            var expected = 2;

            var actual = this.ProblemsService.AllTasksCountByUserId(user.Id);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task AllCompletedTasksCountByUserIdProblemsWithManyUsersShouldReturnCorretcNumber()
        {
            // Arrange
            await this.AddTestingProblemsToDb();
            var userId = "First";

            // Act
            var expected = 1;

            var actual = this.ProblemsService.AllCompletedTasksCountByUserId(userId);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task AllCompletedTasksCountByUserIdUserWithManyProbemsShouldReturnCorretcNumber()
        {
            // Arrange
            var user = new PlanItUser
            {
                Id = "First",
                UserProblems = new List<UserProblem>
                {
                    new UserProblem
                    {
                        Problem = new Problem
                        {
                            ProgressStatus = new ProgressStatus
                            {
                                Name = GlobalConstants.ProgressStatusCompleted,
                            },
                        },
                    },
                    new UserProblem
                    {
                        Problem = new Problem
                        {
                            ProgressStatus = new ProgressStatus
                            {
                                Name = GlobalConstants.ProgressStatusInProgress,
                            },
                        },
                    },
                },
            };

            await this.PlanItDbContext.Users.AddAsync(user);
            await this.PlanItDbContext.SaveChangesAsync();

            // Act
            var expected = 1;

            var actual = this.ProblemsService.AllCompletedTasksCountByUserId(user.Id);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task SumCostsCompleteProjectsByManagerIdFromProjectShouldReturnCorretcValue()
        {
            // Arrange
            var project = new Project
            {
                Id = 1,
                ProjectManagerId = "First",
                IsDeleted = false,
                ProgressStatus = new ProgressStatus
                {
                    Name = GlobalConstants.ProgressStatusCompleted,
                },
            };

            var subProject = new SubProject
            {
                Id = 1,
                ProjectId = 1,
            };

            var problems = new List<Problem>
            {
                new Problem
                {
                    SubProjectId = 1,
                    HourlyRate = 4,
                    TotalHours = 25,
                },
                new Problem
                {
                    SubProjectId = 1,
                    HourlyRate = 4,
                    TotalHours = 100,
                },
            };

            await this.PlanItDbContext.Projects.AddAsync(project);
            await this.PlanItDbContext.SubProjects.AddAsync(subProject);
            await this.PlanItDbContext.Problems.AddRangeAsync(problems);
            await this.PlanItDbContext.SaveChangesAsync();

            var userId = "First";

            // Act
            var expected = 500;

            var actual = this.ProblemsService.SumCostsCompleteProjectsByManagerId(userId);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task SumCostsCompleteProjectsByManagerIdFromProblemsShouldReturnCorretcValue()
        {
            // Arrange
            var problems = new List<Problem>
            {
                new Problem
                {
                    SubProject = new SubProject
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
                    },
                    HourlyRate = 4,
                    TotalHours = 25,
                },
                new Problem
                {
                    SubProject = new SubProject
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
                    },
                    HourlyRate = 4,
                    TotalHours = 100,
                },
                new Problem
                {
                    SubProject = new SubProject
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
                    },
                    HourlyRate = 4,
                    TotalHours = 25,
                },
            };

            await this.PlanItDbContext.Problems.AddRangeAsync(problems);
            await this.PlanItDbContext.SaveChangesAsync();

            var userId = "First";

            // Act
            var expected = 500;

            var actual = this.ProblemsService.SumCostsCompleteProjectsByManagerId(userId);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task AssignAsyncShouldAssignCorrectly()
        {
            // Arrange
            var inputProblem = new TaskAssignInputModel
            {
                Name = "First",
                Instructions = "Draw",
                SubProjectId = 2,
                DueDate = DateTime.Now,
                HourlyRate = 25,
            };

            var status = new ProgressStatus
            {
                Name = GlobalConstants.ProgressStatusAssigned,
            };

            await this.PlanItDbContext.ProgressStatuses.AddAsync(status);
            await this.PlanItDbContext.SaveChangesAsync();

            var user = new PlanItUser
            {
                Id = "First",
            };

            // Act
            var expected = new Problem
            {
                Id = 1,
                Name = "First",
                Instructions = "Draw",
                SubProjectId = 2,
                DueDate = DateTime.UtcNow,
                HourlyRate = 25,
                ProgressStatus = new ProgressStatus
                {
                    Name = GlobalConstants.ProgressStatusAssigned,
                },
                UserProblems = new List<UserProblem>
                {
                    new UserProblem
                    {
                        PlanItUserId = "First",
                        ProblemId = 1,
                    },
                },
            };

            var actual = await this.ProblemsService
                .AssignAsync<TaskAssignInputModel>(user, inputProblem);

            // Assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Instructions, actual.Instructions);
            Assert.Equal(expected.SubProjectId, actual.SubProjectId);
            Assert.Equal(expected.DueDate?.ToString(DateTimeFormat), actual.DueDate?.ToString(DateTimeFormat));
            Assert.Equal(expected.HourlyRate, actual.HourlyRate);
            Assert.Equal(expected.ProgressStatus.Name, actual.ProgressStatus.Name);
            Assert.Equal(expected.UserProblems.First().PlanItUserId, actual.UserProblems.First().PlanItUserId);
            Assert.Equal(expected.UserProblems.First().ProblemId, actual.UserProblems.First().ProblemId);
        }

        [Fact]
        public async Task GetAllAsyncShouldReturnCorrectNumber()
        {
            // Arrange
            var problems = new List<Problem>
            {
                new Problem
                {
                    Id = 1,
                    Name = "Do work1",
                    SubProject = new SubProject
                    {
                        SubProjectType = new SubProjectType
                        {
                            Name = "Type1",
                        },
                        Project = new Project
                        {
                            Name = "Project1",
                        },
                    },
                    Instructions = "Draw",
                    ProgressStatus = new ProgressStatus
                    {
                        Name = GlobalConstants.ProgressStatusCompleted,
                    },
                    TotalHours = 2.5M,
                    DueDate = null,
                },
                new Problem
                {
                    Id = 2,
                    Name = "Do work2",
                    SubProject = new SubProject
                    {
                        SubProjectType = new SubProjectType
                        {
                            Name = "Type1",
                        },
                        Project = new Project
                        {
                            Name = "Project2",
                        },
                    },
                    Instructions = "Draw25",
                    ProgressStatus = new ProgressStatus
                    {
                        Name = GlobalConstants.ProgressStatusInProgress,
                    },
                    TotalHours = 5.5M,
                    DueDate = null,
                },
            };

            await this.PlanItDbContext.Problems.AddRangeAsync(problems);
            await this.PlanItDbContext.SaveChangesAsync();

            // Act
            var expected = 2;

            var viewModels = await this.ProblemsService
                .GetAllAsync<TaskMyTaskViewModel>();

            var actual = viewModels.Count();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetAllByUserIdAsyncShouldReturnCorrectNumber()
        {
            // Arrange
            var problems = new List<Problem>
            {
                new Problem
                {
                    Id = 1,
                    Name = "Do work1",
                    SubProject = new SubProject
                    {
                        SubProjectType = new SubProjectType
                        {
                            Name = "Type1",
                        },
                        Project = new Project
                        {
                            Name = "Project1",
                        },
                    },
                    Instructions = "Draw",
                    ProgressStatus = new ProgressStatus
                    {
                        Name = GlobalConstants.ProgressStatusCompleted,
                    },
                    TotalHours = 2.5M,
                    DueDate = null,
                    UserProblems = new List<UserProblem>
                    {
                        new UserProblem
                        {
                            PlanItUserId = "First",
                        },
                    },
                },
                new Problem
                {
                    Id = 2,
                    Name = "Do work2",
                    SubProject = new SubProject
                    {
                        SubProjectType = new SubProjectType
                        {
                            Name = "Type1",
                        },
                        Project = new Project
                        {
                            Name = "Project2",
                        },
                    },
                    Instructions = "Draw25",
                    ProgressStatus = new ProgressStatus
                    {
                        Name = GlobalConstants.ProgressStatusInProgress,
                    },
                    TotalHours = 5.5M,
                    DueDate = null,
                    UserProblems = new List<UserProblem>
                    {
                        new UserProblem
                        {
                            PlanItUserId = "Second",
                        },
                    },
                },
            };

            await this.PlanItDbContext.Problems.AddRangeAsync(problems);
            await this.PlanItDbContext.SaveChangesAsync();

            var userId = "First";

            // Act
            var expected = 1;

            var viewModels = await this.ProblemsService
                .GetAllByUserIdAsync<TaskMyTaskViewModel>(userId);

            var actual = viewModels.Count();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetAllByManagerIdAsyncShouldReturnCorrectNumber()
        {
            // Arrange
            var problems = new List<Problem>
            {
                new Problem
                {
                    Id = 1,
                    Name = "Do work1",
                    SubProject = new SubProject
                    {
                        SubProjectType = new SubProjectType
                        {
                            Name = "Type1",
                        },
                        Project = new Project
                        {
                            Name = "Project1",
                            ProjectManagerId = "Manager1",
                        },
                    },
                    Instructions = "Draw",
                    ProgressStatus = new ProgressStatus
                    {
                        Name = GlobalConstants.ProgressStatusCompleted,
                    },
                    TotalHours = 2.5M,
                    DueDate = null,
                    UserProblems = new List<UserProblem>
                    {
                        new UserProblem
                        {
                            PlanItUserId = "First",
                        },
                    },
                },
                new Problem
                {
                    Id = 2,
                    Name = "Do work2",
                    SubProject = new SubProject
                    {
                        SubProjectType = new SubProjectType
                        {
                            Name = "Type1",
                        },
                        Project = new Project
                        {
                            Name = "Project2",
                            ProjectManagerId = "Manager2",
                        },
                    },
                    Instructions = "Draw25",
                    ProgressStatus = new ProgressStatus
                    {
                        Name = GlobalConstants.ProgressStatusInProgress,
                    },
                    TotalHours = 5.5M,
                    DueDate = null,
                    UserProblems = new List<UserProblem>
                    {
                        new UserProblem
                        {
                            PlanItUserId = "Second",
                        },
                    },
                },
            };

            await this.PlanItDbContext.Problems.AddRangeAsync(problems);
            await this.PlanItDbContext.SaveChangesAsync();

            var userId = "Manager1";

            // Act
            var expected = 1;

            var viewModels = await this.ProblemsService
                .GetAllByManagerIdAsync<TaskMyTaskViewModel>(userId);

            var actual = viewModels.Count();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetByIdAsyncAsyncWithViewModelShouldReturnCorrectProblem()
        {
            // Arrange
            var problems = new List<Problem>
            {
                new Problem
                {
                    Id = 1,
                    Name = "Do work1",
                    SubProject = new SubProject
                    {
                        SubProjectType = new SubProjectType
                        {
                            Name = "Type1",
                        },
                        Project = new Project
                        {
                            Name = "Project1",
                            ProjectManagerId = "Manager1",
                        },
                    },
                    Instructions = "Draw",
                    ProgressStatus = new ProgressStatus
                    {
                        Name = GlobalConstants.ProgressStatusCompleted,
                    },
                    TotalHours = 2.5M,
                    DueDate = null,
                    UserProblems = new List<UserProblem>
                    {
                        new UserProblem
                        {
                            PlanItUserId = "First",
                        },
                    },
                },
                new Problem
                {
                    Id = 2,
                    Name = "Do work2",
                    SubProject = new SubProject
                    {
                        SubProjectType = new SubProjectType
                        {
                            Name = "Type1",
                        },
                        Project = new Project
                        {
                            Name = "Project2",
                            ProjectManagerId = "Manager2",
                        },
                    },
                    Instructions = "Draw25",
                    ProgressStatus = new ProgressStatus
                    {
                        Name = GlobalConstants.ProgressStatusInProgress,
                    },
                    TotalHours = 5.5M,
                    DueDate = null,
                    UserProblems = new List<UserProblem>
                    {
                        new UserProblem
                        {
                            PlanItUserId = "Second",
                        },
                    },
                },
            };

            await this.PlanItDbContext.Problems.AddRangeAsync(problems);
            await this.PlanItDbContext.SaveChangesAsync();

            var problemId = 2;

            // Act
            var expected = new TaskMyTaskViewModel
            {
                Id = 2,
                Name = "Do work2",
                SubProjectSubProjectTypeName = "Type1",
                SubProjectProjectName = "Project2",
                ProgressStatusName = GlobalConstants.ProgressStatusInProgress,
                Instructions = "Draw25",
                TotalHours = 5.5M,
                DueDate = null,
            };

            var actual = await this.ProblemsService
                .GetByIdAsync<TaskMyTaskViewModel>(problemId);

            // Assert
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.SubProjectProjectName, actual.SubProjectProjectName);
            Assert.Equal(expected.Instructions, actual.Instructions);
        }

        [Fact]
        public async Task GetByIdAsyncAsyncShouldReturnCorrectProblem()
        {
            // Arrange
            var problems = new List<Problem>
            {
                new Problem
                {
                    Id = 1,
                    Name = "Do work1",
                    SubProject = new SubProject
                    {
                        SubProjectType = new SubProjectType
                        {
                            Name = "Type1",
                        },
                        Project = new Project
                        {
                            Name = "Project1",
                            ProjectManagerId = "Manager1",
                        },
                    },
                    Instructions = "Draw",
                    ProgressStatus = new ProgressStatus
                    {
                        Name = GlobalConstants.ProgressStatusCompleted,
                    },
                    TotalHours = 2.5M,
                    DueDate = null,
                    UserProblems = new List<UserProblem>
                    {
                        new UserProblem
                        {
                            PlanItUserId = "First",
                        },
                    },
                },
                new Problem
                {
                    Id = 2,
                    Name = "Do work2",
                    SubProject = new SubProject
                    {
                        SubProjectType = new SubProjectType
                        {
                            Name = "Type1",
                        },
                        Project = new Project
                        {
                            Name = "Project2",
                            ProjectManagerId = "Manager2",
                        },
                    },
                    Instructions = "Draw25",
                    ProgressStatus = new ProgressStatus
                    {
                        Name = GlobalConstants.ProgressStatusInProgress,
                    },
                    TotalHours = 5.5M,
                    DueDate = null,
                    UserProblems = new List<UserProblem>
                    {
                        new UserProblem
                        {
                            PlanItUserId = "Second",
                        },
                    },
                },
            };

            await this.PlanItDbContext.Problems.AddRangeAsync(problems);
            await this.PlanItDbContext.SaveChangesAsync();

            var problemId = 2;

            // Act
            var expected = new Problem
            {
                Id = 2,
                Name = "Do work2",
                Instructions = "Draw25",
                TotalHours = 5.5M,
                DueDate = null,
            };

            var actual = await this.ProblemsService
                .GetByIdAsync(problemId);

            // Assert
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Instructions, actual.Instructions);
        }

        [Fact]
        public async Task AddHoursAsyncShouldAddHoursCorrecty()
        {
            // Arrange
            var problem = new Problem
            {
                Id = 1,
                TotalHours = 20,
            };

            await this.PlanItDbContext.Problems.AddAsync(problem);
            await this.PlanItDbContext.SaveChangesAsync();

            var problemId = 1;
            var hours = 8;

            // Act
            var expected = 28;

            var actual = await this.ProblemsService
                .AddHoursAsync(problemId, hours);

            // Assert
            Assert.Equal(expected, actual.TotalHours);
        }

        [Fact]
        public async Task AddUserAsyncShouldAddUserCorrecty()
        {
            // Arrange
            var problem = new Problem
            {
                Id = 1,
                UserProblems = new List<UserProblem>(),
            };

            await this.PlanItDbContext.Problems.AddAsync(problem);
            await this.PlanItDbContext.SaveChangesAsync();

            var problemId = 1;
            var user = new PlanItUser
            {
                Id = "First",
            };

            // Act
            var expectedCountUsers = 1;
            var expectedUserId = "First";

            var actual = await this.ProblemsService
                .AddUserAsync(problemId, user);

            // Assert
            Assert.Equal(expectedCountUsers, actual.UserProblems.Count());
            Assert.Equal(expectedUserId, actual.UserProblems.First().PlanItUserId);
        }

        [Fact]
        public async Task ChangeStatusAsyncShouldChangeStatusCorrecty()
        {
            // Arrange
            var problem = new Problem
            {
                Id = 1,
                ProgressStatus = new ProgressStatus
                {
                    Name = GlobalConstants.ProgressStatusInProgress,
                },
            };

            await this.PlanItDbContext.Problems.AddAsync(problem);
            await this.PlanItDbContext.SaveChangesAsync();

            var problemId = 1;
            var status = new ProgressStatus
            {
                Name = GlobalConstants.ProgressStatusCompleted,
            };

            // Act
            var expected = new Problem
            {
                Id = 1,
                ProgressStatus = new ProgressStatus
                {
                    Name = GlobalConstants.ProgressStatusCompleted,
                },
            };

            var actual = await this.ProblemsService
                .ChangeStatusAsync(problemId, status);

            // Assert
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.ProgressStatus.Name, actual.ProgressStatus.Name);
        }

        [Fact]
        public async Task EditAsyncShouldEditProblemCorrecty()
        {
            // Arrange
            var problem = new Problem
            {
                Id = 1,
                Name = "Work2",
                Instructions = "GoGoNo",
                DueDate = DateTime.UtcNow,
                HourlyRate = 5.5M,
                ProgressStatusId = 3,
            };

            await this.PlanItDbContext.Problems.AddAsync(problem);
            await this.PlanItDbContext.SaveChangesAsync();

            var inputModel = new TaskEditInputModel
            {
                Id = 1,
                Name = "Work1",
                Instructions = "GoGo",
                DueDate = DateTime.Now.AddDays(10),
                HourlyRate = 2.5M,
                ProgressStatusId = 2,
            };

            // Act
            var expected = new Problem
            {
                Id = 1,
                Name = "Work1",
                Instructions = "GoGo",
                DueDate = DateTime.UtcNow.AddDays(10),
                HourlyRate = 2.5M,
                ProgressStatusId = 2,
            };

            var actual = await this.ProblemsService
                .EditAsync<TaskEditInputModel>(inputModel);

            // Assert
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Instructions, actual.Instructions);
            Assert.Equal(expected.DueDate?.ToString(DateTimeFormat), actual.DueDate?.ToString(DateTimeFormat));
            Assert.Equal(expected.HourlyRate, actual.HourlyRate);
            Assert.Equal(expected.ProgressStatusId, actual.ProgressStatusId);
        }

        private async Task AddTestingProblemsToDb()
        {
            var problems = new List<Problem>
            {
                new Problem
                {
                    Id = 1,
                    UserProblems = new List<UserProblem>
                    {
                        new UserProblem
                        {
                        PlanItUserId = "First",
                        },
                    },
                    ProgressStatus = new ProgressStatus
                    {
                        Name = GlobalConstants.ProgressStatusCompleted,
                    },
                },
                new Problem
                {
                    Id = 2,
                    UserProblems = new List<UserProblem>
                    {
                        new UserProblem
                        {
                            PlanItUserId = "Second",
                        },
                    },
                    ProgressStatus = new ProgressStatus
                    {
                        Name = GlobalConstants.ProgressStatusCompleted,
                    },
                },
                new Problem
                {
                    Id = 3,
                    UserProblems = new List<UserProblem>
                    {
                        new UserProblem
                        {
                        PlanItUserId = "First",
                        },
                    },
                    ProgressStatus = new ProgressStatus
                    {
                        Name = GlobalConstants.ProgressStatusInProgress,
                    },
                },
            };

            await this.PlanItDbContext.Problems.AddRangeAsync(problems);
            await this.PlanItDbContext.SaveChangesAsync();
        }
    }
}
