using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Data.Entity;
using PipeCI.Blob.Sample.Models;

namespace PipeCI.Blob.Sample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFramework()
                .AddInMemoryDatabase()
                .AddDbContext<SampleContext>(x => x.UseInMemoryDatabase());

            services.AddBlob()
                .AddEntityFrameworkStorage<SampleContext>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory logger)
        {
            logger.AddConsole();
            logger.AddDebug();
            logger.MinimumLevel = LogLevel.Information;

            var db = app.ApplicationServices.GetRequiredService<SampleContext>();
            if (!db.Database.EnsureCreated())
                throw new Exception("Database init failed.");

            app.UseIISPlatformHandler();
            app.UseBlob();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync(
@"<html>
    <body>
        <form action=""/blob/upload"" method=""post"" enctype=""multipart/form-data"">
            <input type=""file"" name=""file"" />
            <input type=""submit"" />
        </form>
    </body>
</html>");
            });
        }

        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
