using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;
using UserService.Helpers;
using UserService.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(UserServiceStartup.Startup))]

namespace UserServiceStartup
{
    public class Startup : FunctionsStartup
    {
        // private readonly IConfiguration _config;
        // public Startup(IConfiguration config)
        // {
        //     _config = config;
        // }

        // public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer("Server=tcp:dotnet-pravda-db.database.windows.net,1433;Initial Catalog=dotnet-pravda-db;Persist Security Info=False;User ID=dotnet-pravda-db-user;Password=M$&R7Z\\amkN>])\"5;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            });

            builder.Services.AddTransient<IUserService, UserService.Services.UserService>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            // configure strongly typed settings objects
            // var appSettingsSection = _config.GetSection("AppSettings");
            // builder.Services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            // var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.UTF8.GetBytes("this is my custom Secret key for authentication");
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("WebJobsAuthLevel", x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        var userId = int.Parse(context.Principal.Identity.Name);
                        var user = userService.GetById(userId);
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    },
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // configure DI for application services
            builder.Services.AddScoped<IUserService, UserService.Services.UserService>();
        }

        // // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        // {
        //     if (env.IsDevelopment())
        //     {
        //         app.UseDeveloperExceptionPage();
        //     }

        //     app.UseHttpsRedirection();

        //     app.UseRouting();

        //     app.UseCors();

        //     app.UseAuthorization();

        //     app.UseEndpoints(endpoints =>
        //     {
        //         endpoints.MapControllers();
        //     });
        // }
    }
}
