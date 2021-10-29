using System;
using PlatformService.Data;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PlatformService.SyncDataService.Http;
using Microsoft.Extensions.DependencyInjection;

namespace PlatformService
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            this.Configuration = configuration;
            this._env = env;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            if (this._env.IsProduction())
            {
                Console.WriteLine("--> Using SqlServer Db");
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("PlatformsConn")));
            }
            else
            {
                Console.WriteLine("--> Using InMem Db");

                services
                    .AddDbContext<AppDbContext>(options =>
                        options.UseInMemoryDatabase("InMem"));
            }

            services.AddScoped<IPlatformRepo, PlatformRepo>();

            services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

            services.AddControllers();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlatformService", Version = "v1" }));

            Console.WriteLine($"--> CommandService Endpoint {this.Configuration["CommandService"]}");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app
                    .UseDeveloperExceptionPage()
                    .UseSwagger()
                    .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlatformService v1"));
            }

            app
                .UseHttpsRedirection()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

            PrepDb.PrepPopulation(app, env.IsProduction());
        }
    }
}
