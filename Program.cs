using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace RedLeg.Forms
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseSentry(dsn: "https://31730b790946460e8c5c786136c81231@o255975.ingest.sentry.io/5218373")
                        .UseStartup<Startup>();
                });
    }
}
