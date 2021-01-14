using NewsStorage.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(NewsStorageService.Startup))]

namespace NewsStorageService
{
    public class Startup : FunctionsStartup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer("Server=tcp:dotnet-pravda-db.database.windows.net,1433;Initial Catalog=dotnet-pravda-db;Persist Security Info=False;User ID=dotnet-pravda-db-user;Password=M$&R7Z\\amkN>])\"5;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            });

            builder.Services.AddTransient<IClassifiedRepository, CachedClassifiedRepository>();
        }

        //     // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //     public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        //     {
        //         if (env.IsDevelopment())
        //         {
        //             app.UseDeveloperExceptionPage();
        //             app.UseSwagger();
        //             app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NewsStorage v1"));
        //         }

        //         app.UseHttpsRedirection();

        //         app.UseRouting();

        //         app.UseCors();

        //         app.UseAuthorization();

        //         app.UseEndpoints(endpoints =>
        //         {
        //             endpoints.MapControllers();
        //         });
        //     }
    }
}
