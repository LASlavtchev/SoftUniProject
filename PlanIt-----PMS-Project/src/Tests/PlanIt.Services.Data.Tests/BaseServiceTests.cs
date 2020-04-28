namespace PlanIt.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using PlanIt.Data;
    using PlanIt.Data.Common.Repositories;
    using PlanIt.Data.Models;
    using PlanIt.Data.Repositories;
    using PlanIt.Services.Mapping;
    using PlanIt.Web.ViewModels;

    public abstract class BaseServiceTests : IDisposable
    {
        public BaseServiceTests()
        {
            var services = this.SetServices();

            this.ServiceProvider = services.BuildServiceProvider();
            this.PlanItDbContext = this.ServiceProvider.GetRequiredService<PlanItDbContext>();
            this.InvitesDbContext = this.ServiceProvider.GetRequiredService<InvitesDbContext>();
        }

        protected IServiceProvider ServiceProvider { get; set; }

        protected PlanItDbContext PlanItDbContext { get; set; }

        protected InvitesDbContext InvitesDbContext { get; set; }

        public void Dispose()
        {
            this.SetServices();
        }

        private ServiceCollection SetServices()
        {
            var services = new ServiceCollection();

            services.AddDbContext<PlanItDbContext>(
                options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            services.AddDbContext<InvitesDbContext>(
                options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            services.AddIdentityCore<PlanItUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<PlanItRole>().AddEntityFrameworkStores<PlanItDbContext>();

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(InvitesRepository<>));

            // Application services
            services.AddTransient<IInvitesService, InvitesService>();
            services.AddTransient<IClientsServices, ClientsService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IProjectsService, ProjectsService>();
            services.AddTransient<IProgressStatusesService, ProgressStatusesService>();
            services.AddTransient<IProblemsService, ProblemsService>();
            services.AddTransient<ISubProjectsService, SubProjectsService>();

            // AutoMapper
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            return services;
        }
    }
}
