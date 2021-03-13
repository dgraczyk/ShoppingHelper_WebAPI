using API.Middleware;
using Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Persistence;
using Serilog;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });

            services.AddApplicationServices();
            services.AddPersistenceServices(Configuration);

            services.AddHealthChecks()
                 .AddSqlServer(Configuration.GetConnectionString("ShoppingHelperConnectionString"), name: "ShoppingHelperDb", failureStatus: HealthStatus.Unhealthy);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSerilogRequestLogging();

            app.UseCustomExceptionHandler();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    ResponseWriter = WriteHealthCheckResponse,
                    AllowCachingResponses = false
                });
            });
        }

        private Task WriteHealthCheckResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";

            var json = new JObject(
                new JProperty("Status", result.Status.ToString()),
                new JProperty("TotalChecksDuration", result.TotalDuration.TotalSeconds.ToString("0:0.00")),
                new JProperty("DependencyHealthChecks", new JObject(result.Entries.Select(item =>
                    new JProperty(item.Key, new JObject(
                        new JProperty("Status", item.Value.Status.ToString()),
                        new JProperty("Duration", item.Value.Duration.TotalSeconds.ToString("0:0.00"))
                        ))
                    )))
                );

            return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));
        }
    }
}
