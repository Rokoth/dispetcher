using _100uslug.Models;
using _100uslug.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace _100uslug
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
            services.AddMvc(opt =>
            {
                opt.EnableEndpointRouting = false;
            });
            services.AddDbContext<BaseContext>((sp, opt) =>
            {
                opt.UseInternalServiceProvider(sp);
            });

            foreach (var type in typeof(Company).Assembly.GetTypes())
            {
                if (typeof(Entity).IsAssignableFrom(type))
                {
                    services.AddScoped(typeof(CustomControllerService<>).MakeGenericType(type));
                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.Use(async (context, next) =>
            {
                bool customExecute = false;
                var path = context.Request.Path.Value;
                string[] segments = path.Split("/")
                    .Where(s => !string.IsNullOrEmpty(s)).ToArray();
                if (segments.Length > 0 && segments[0].Equals("Simple", StringComparison.InvariantCultureIgnoreCase))
                {
                    var model = Array.Find(typeof(Company).Assembly.GetTypes(), s => s.Name.Equals(segments[1],
                            StringComparison.InvariantCultureIgnoreCase));
                    var type = typeof(CustomControllerService<>).MakeGenericType(model);
                    var service = context.RequestServices.GetService(type);
                    if (service != null)
                    {
                        var method = Array.Find(type.GetMethods(), s => s.Name.Equals(segments[2], StringComparison.InvariantCultureIgnoreCase));
                        if (method != null)
                        {
                            customExecute = true;
                            var ret = await (Task<IActionResult>)method.Invoke(service, new object[] { context });
                            if (!(ret is OkResult))
                            {

                            }
                        }
                    }
                }
                if (!customExecute) await next().ConfigureAwait(false);
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }


}
