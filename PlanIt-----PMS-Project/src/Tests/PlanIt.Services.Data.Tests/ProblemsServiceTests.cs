namespace PlanIt.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using PlanIt.Data.Models;
    using PlanIt.Web.ViewModels.Administration.Invites;
    using Xunit;

    public class ProblemsServiceTests : BaseServiceTests
    {
        private IProblemsService ProblemsService => this.ServiceProvider.GetRequiredService<IProblemsService>();

       
    }
}
