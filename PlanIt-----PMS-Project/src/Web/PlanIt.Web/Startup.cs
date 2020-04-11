namespace PlanIt.Web
{
    using System.Reflection;

    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using PlanIt.Data;
    using PlanIt.Data.Common;
    using PlanIt.Data.Common.Repositories;
    using PlanIt.Data.Models;
    using PlanIt.Data.Repositories;
    using PlanIt.Data.Seeding;
    using PlanIt.Services.Data;
    using PlanIt.Services.Mapping;
    using PlanIt.Services.Messaging;
    using PlanIt.Web.ViewModels;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Main database
            services.AddDbContext<PlanItDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            // Helper database for inviting users
            services.AddDbContext<InvitesDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnectionInvites")));

            services.AddDefaultIdentity<PlanItUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<PlanItRole>().AddEntityFrameworkStores<PlanItDbContext>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()); // CSRF
            });

            services.Configure<CookieAuthenticationOptions>(options =>
            {
                options.LoginPath = new PathString("/Account/Login");
            });

            services.AddRazorPages();

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(InvitesRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender, NullMessageSender>();
            services.AddTransient<IInvitesService, InvitesService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IClientsServices, ClientsService>();
            services.AddTransient<IProjectsService, ProjectsService>();
            services.AddTransient<IProgressStatusesService, ProgressStatusesService>();
            services.AddTransient<ISubProjectTypesService, SubProjectTypesService>();

            var sendGridApiKey = this.configuration["SendGrid:ApiKey"];
            services
                .AddTransient<IEmailSender>(serviceProvider => new SendGridEmailSender(sendGridApiKey));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var mainDbContext = serviceScope.ServiceProvider.GetRequiredService<PlanItDbContext>();
                var inviteUserDbContext = serviceScope.ServiceProvider.GetRequiredService<InvitesDbContext>();

                if (env.IsDevelopment())
                {
                    mainDbContext.Database.Migrate();
                    inviteUserDbContext.Database.Migrate();
                }

                new PlanItDbContextSeeder().SeedAsync(mainDbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
                new InvitesDbContextSeeder().SeedAsync(inviteUserDbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapRazorPages();
                });
        }
    }
}
