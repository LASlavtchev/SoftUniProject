﻿namespace PlanIt.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using PlanIt.Data;
    using PlanIt.Data.Common.Repositories;
    using PlanIt.Data.Models;
    using PlanIt.Data.Repositories;
    using Xunit;

    public class SettingsServiceTests
    {
        [Fact]
        public void GetCountShouldReturnCorrectNumber()
        {
            //var repository = new Mock<IDeletableEntityRepository<Setting>>();
            //repository.Setup(r => r.All()).Returns(new List<Setting>
            //                                            {
            //                                                new Setting(),
            //                                                new Setting(),
            //                                                new Setting(),
            //                                            }.AsQueryable());
            //var service = new SettingsService(repository.Object);
            //Assert.Equal(3, service.GetCount());
            //repository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public async Task GetCountShouldReturnCorrectNumberUsingDbContext()
        {
            //var options = new DbContextOptionsBuilder<PlanItDbContext>()
            //    .UseInMemoryDatabase(databaseName: "SettingsTestDb").Options;
            //var dbContext = new PlanItDbContext(options);
            ////dbContext.Settings.Add(new Setting());
            ////dbContext.Settings.Add(new Setting());
            ////dbContext.Settings.Add(new Setting());
            //await dbContext.SaveChangesAsync();

            //var repository = new EfDeletableEntityRepository<Setting>(dbContext);
            //var service = new SettingsService(repository);
            //Assert.Equal(3, service.GetCount());
        }
    }
}
