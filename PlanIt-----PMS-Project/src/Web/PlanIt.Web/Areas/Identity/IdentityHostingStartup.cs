using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(PlanIt.Web.Areas.Identity.IdentityHostingStartup))]

namespace PlanIt.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
