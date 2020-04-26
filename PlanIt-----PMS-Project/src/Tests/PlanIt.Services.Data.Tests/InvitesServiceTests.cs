namespace PlanIt.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using PlanIt.Data;
    using PlanIt.Data.Common.Repositories;
    using PlanIt.Data.Models;
    using PlanIt.Data.Repositories;
    using PlanIt.Services.Mapping;
    using PlanIt.Web.ViewModels.Administration.Invites;
    using Xunit;

    public class InvitesServiceTests : BaseServiceTests
    {
        private IInvitesService InvitesService => this.ServiceProvider.GetRequiredService<IInvitesService>();

        [Fact]
        public async Task CreateInviteByUserAsyncShouldCreateInviteCorrectly()
        {
            // Arrange
            var email = "aaa@aaa.aa";
            var purpose = "New project";
            string format = "MM/dd/yy H:mm";

            // Act
            var expected = new Invite
            {
                Email = email,
                Purpose = purpose,
                IsInvited = false,
                RequestExpiredOn = DateTime.Now.ToUniversalTime().AddHours(24),
                CreatedOn = DateTime.UtcNow,
            };

            var actual = await this.InvitesService.CreateInviteByUserAsync(email, purpose);

            // Assert
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.Purpose, actual.Purpose);
            Assert.False(actual.IsInvited);
            Assert.Equal(expected.RequestExpiredOn.ToString(format), actual.RequestExpiredOn.ToString(format));
            Assert.Equal(expected.CreatedOn.ToString(format), actual.CreatedOn.ToString(format));
            Assert.Null(actual.SecurityValue);
            Assert.Null(actual.ExpiredOn);
        }

        [Fact]
        public async Task CreateAsyncShouldCreateInviteCorrectly()
        {
            // Arrange
            var email = "aaa@aaa.aa";
            var purpose = "New project";
            string format = "MM/dd/yy H:mm";

            // Act
            var expected = new Invite
            {
                Email = email,
                Purpose = purpose,
                IsInvited = true,
                ExpiredOn = DateTime.UtcNow.AddDays(2),
                CreatedOn = DateTime.UtcNow,
            };

            expected.RequestExpiredOn = (DateTime)expected.ExpiredOn;

            var actual = await this.InvitesService.CreateAsync(expected);

            // Assert
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.Purpose, actual.Purpose);
            Assert.True(actual.IsInvited);
            Assert.Equal(expected.CreatedOn.ToString(format), actual.CreatedOn.ToString(format));
            Assert.Equal(expected.ExpiredOn?.ToString(format), actual.ExpiredOn?.ToString(format));
            Assert.Equal(expected.RequestExpiredOn.ToString(format), actual.RequestExpiredOn.ToString(format));
            Assert.True(!string.IsNullOrEmpty(actual.SecurityValue) && !string.IsNullOrWhiteSpace(actual.SecurityValue));
        }

        [Fact]
        public async Task GetAllCountShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingInvitesToDb();

            // Act
            var expected = 4;

            var actual = this.InvitesService.GetAllCount();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetAllApprovedCountShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingInvitesToDb();

            // Act
            var expected = 1;

            var actual = this.InvitesService.GetAllApprovedCount();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetAllInvitedExpiredOnCountShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingInvitesToDb();

            // Act
            var expected = 1;

            var actual = this.InvitesService.GetAllInvitedExpiredOnCount();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetAllRequestExpiredOnCountShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingInvitesToDb();

            // Act
            var expected = 1;

            var actual = this.InvitesService.GetAllRequestExpiredOnCount();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetAllAsyncShouldReturnCorrectNumber()
        {
            // Arrange
            await this.AddTestingInvitesToDb();

            var invites = await this.InvitesService.GetAllAsync<InviteViewModel>();

            // Act
            var expected = 4;

            var actual = invites.Count();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetByEmailAsyncWithExistingEmailShouldReturnCorrectInvite()
        {
            // Arrange
            await this.AddTestingInvitesToDb();

            string email = "aaa1@aaa.aa";

            // Act
            var actual = await this.InvitesService.GetByEmailAsync<InviteViewModel>(email);

            // Assert
            Assert.Equal(1, actual.Id);
        }

        [Fact]
        public async Task GetByEmailAsyncWithNoExistingEmailShouldReturnNull()
        {
            // Arrange
            await this.AddTestingInvitesToDb();

            string email = "notExist";

            // Act
            var actual = await this.InvitesService.GetByEmailAsync<InviteViewModel>(email);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public async Task GetByIdAsyncWithExistingIdShouldReturnCorrectInvite()
        {
            // Arrange
            await this.AddTestingInvitesToDb();

            int id = 1;

            // Act
            var expected = "aaa1@aaa.aa";

            var actual = await this.InvitesService.GetByIdAsync<InviteViewModel>(id);

            // Assert
            Assert.Equal(expected, actual.Email);
        }

        [Fact]
        public async Task GetByIdAsyncWithNoExistingIdShouldReturnNull()
        {
            // Arrange
            await this.AddTestingInvitesToDb();

            int id = 10;

            // Act
            var actual = await this.InvitesService.GetByIdAsync<InviteViewModel>(id);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public async Task ApproveAsyncShouldUpdateInvite()
        {
            // Arrange
            var existingInvite = new Invite
            {
                Id = 1,
                Email = "aaa1@aaa.aa",
                Purpose = "Project1",
                IsInvited = false,
                RequestExpiredOn = DateTime.UtcNow,
            };

            await this.InvitesDbContext.Invites.AddAsync(existingInvite);
            await this.InvitesDbContext.SaveChangesAsync();

            string format = "MM/dd/yy H:mm";

            // Act
            var expected = new Invite
            {
                Id = 1,
                Email = "aaa1@aaa.aa",
                Purpose = "Project1",
                IsInvited = true,
                ExpiredOn = DateTime.UtcNow.AddHours(24),
                RequestExpiredOn = DateTime.UtcNow.AddHours(24),
                SecurityValue = this.InvitesService.GenerateUniqueSecurityValue(),
            };

            var actual = await this.InvitesService.ApproveAsync(existingInvite.Id);

            // Assert
            Assert.Equal(expected.Id, actual.Id);
            Assert.True(actual.IsInvited);
            Assert.Equal(expected.ExpiredOn?.ToString(format), actual.ExpiredOn?.ToString(format));
            Assert.Equal(expected.RequestExpiredOn.ToString(format), actual.RequestExpiredOn.ToString(format));
            Assert.True(!string.IsNullOrEmpty(actual.SecurityValue) && !string.IsNullOrWhiteSpace(actual.SecurityValue));
        }

        [Fact]
        public async Task ExtendAsyncWithNullExpiredOnShouldUpdateInvite()
        {
            // Arrange
            var existingInvite = new Invite
            {
                Id = 1,
                Email = "aaa1@aaa.aa",
                IsInvited = false,
                ExpiredOn = null,
                RequestExpiredOn = DateTime.UtcNow,
            };

            await this.InvitesDbContext.Invites.AddAsync(existingInvite);
            await this.InvitesDbContext.SaveChangesAsync();

            var inviteExtendInputModel = await this.InvitesService
                .GetByIdAsync<InviteExtendInputModel>(existingInvite.Id);

            string format = "MM/dd/yy H:mm";

            // Act
            var expected = new Invite
            {
                Id = 1,
                IsInvited = false,
                ExpiredOn = null,
                RequestExpiredOn = DateTime.UtcNow.AddHours(24),
            };

            var actual = await this.InvitesService
                .ExtendAsync<InviteExtendInputModel>(inviteExtendInputModel);

            // Assert
            Assert.Equal(expected.Id, actual.Id);
            Assert.False(actual.IsInvited);
            Assert.Null(actual.ExpiredOn);
            Assert.Equal(expected.RequestExpiredOn.ToString(format), actual.RequestExpiredOn.ToString(format));
            Assert.False(!string.IsNullOrEmpty(actual.SecurityValue) && !string.IsNullOrWhiteSpace(actual.SecurityValue));
        }

        [Fact]
        public async Task ExtendAsyncWithNotNullExpiredOnShouldUpdateInvite()
        {
            // Arrange
            var existingInvite = new Invite
            {
                Id = 1,
                Email = "aaa1@aaa.aa",
                IsInvited = true,
                ExpiredOn = DateTime.UtcNow,
                RequestExpiredOn = DateTime.UtcNow,
                SecurityValue = this.InvitesService.GenerateUniqueSecurityValue(),
            };

            await this.InvitesDbContext.Invites.AddAsync(existingInvite);
            await this.InvitesDbContext.SaveChangesAsync();

            var inviteExtendInputModel = await this.InvitesService
                .GetByIdAsync<InviteExtendInputModel>(existingInvite.Id);

            string format = "MM/dd/yy H:mm";

            // Act
            var expected = new Invite
            {
                Id = 1,
                IsInvited = true,
                ExpiredOn = DateTime.UtcNow.AddHours(24),
                RequestExpiredOn = DateTime.UtcNow.AddHours(24),
                SecurityValue = this.InvitesService.GenerateUniqueSecurityValue(),
            };

            var actual = await this.InvitesService
                .ExtendAsync<InviteExtendInputModel>(inviteExtendInputModel);

            // Assert
            Assert.Equal(expected.Id, actual.Id);
            Assert.True(actual.IsInvited);
            Assert.Equal(expected.ExpiredOn?.ToString(format), actual.ExpiredOn?.ToString(format));
            Assert.Equal(expected.RequestExpiredOn.ToString(format), actual.RequestExpiredOn.ToString(format));
            Assert.True(!string.IsNullOrEmpty(actual.SecurityValue) && !string.IsNullOrWhiteSpace(actual.SecurityValue));
        }

        [Fact]
        public async Task DeleteAsyncShouldDecreaseCountByOne()
        {
            // Arrange
            await this.AddTestingInvitesToDb();

            int id = 1;

            // Act
            var expected = 3;

            await this.InvitesService.DeleteAsync(id);

            // Assert
            Assert.Equal(expected, this.InvitesDbContext.Invites.Count());
        }

        private async Task AddTestingInvitesToDb()
        {
            var invites = new List<Invite>
            {
                new Invite
                {
                    Id = 1,
                    Email = "aaa1@aaa.aa",
                    Purpose = "Project1",
                    IsInvited = false,
                    RequestExpiredOn = DateTime.Now.ToUniversalTime().AddHours(24),
                },
                new Invite
                {
                    Id = 2,
                    SecurityValue = this.InvitesService.GenerateUniqueSecurityValue(),
                    Email = "aaa2@aaa.aa",
                    Purpose = "Project2",
                    IsInvited = true,
                    ExpiredOn = DateTime.UtcNow.AddDays(2),
                    RequestExpiredOn = DateTime.UtcNow.AddDays(2),
                },
                new Invite
                {
                    Id = 3,
                    Email = "aaa3@aaa.aa",
                    Purpose = "Project3",
                    IsInvited = false,
                    RequestExpiredOn = DateTime.UtcNow.AddDays(-1),
                },
                new Invite
                {
                    Id = 4,
                    Email = "aaa4@aaa.aa",
                    Purpose = "Project4",
                    IsInvited = true,
                    ExpiredOn = DateTime.UtcNow.AddDays(-1),
                },
            };

            await this.InvitesDbContext.Invites.AddRangeAsync(invites);
            await this.InvitesDbContext.SaveChangesAsync();
        }
    }
}
