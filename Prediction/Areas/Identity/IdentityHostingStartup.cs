using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Prediction.Areas.Identity.IdentityHostingStartup))]
namespace Prediction.Areas.Identity
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