using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ClassificationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // ClassificationService.Model.ConsumeModel.ModelPath = args[0];
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
