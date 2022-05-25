using Fortnite_Plugins_Center.API.Filters;
using Fortnite_Plugins_Center.Shared.Exceptions.Common;
using Fortnite_Plugins_Center.Shared.Models.Mongo;
using Fortnite_Plugins_Center.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Fortnite_Plugins_Center.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.Filters.Add(new BaseExceptionFilterAttribute())).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.Configure<MongoSettings>(Configuration.GetSection("Mongo"));
            services.AddSingleton<IMongoSettings>(sp => sp.GetRequiredService<IOptions<MongoSettings>>().Value);

            services.AddSingleton<IMongoService, MongoService>();

            services.AddHttpContextAccessor();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMongoService mongo)
        {
            mongo.Ping();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var path = Path.Join(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Cloudstorage");
            Directory.CreateDirectory(path);

            app.UseStatusCodePages(async context =>
            {
                var json = "";
                context.HttpContext.Response.Headers["Content-Type"] = "application/json";

                json = (HttpStatusCode)context.HttpContext.Response.StatusCode switch
                {
                    HttpStatusCode.NotFound => JsonConvert.SerializeObject(new NotFoundException()),
                    _ => json
                };

                await context.HttpContext.Response.WriteAsync(json);
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
