using _100uslug.Models;
using _100uslug.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoUslug.Common;
using StoUslug.Db.Context;
using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using StoUslug.DeployerService;

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
            services.Configure<CommonOptions>(Configuration);
            services.AddControllersWithViews();
            services.AddLogging();
            services.AddSingleton<IErrorNotifyService, ErrorNotifyService>();
            services.AddDbContextPool<DbPgContext>((opt) =>
            {
                opt.EnableSensitiveDataLogging();
                var connectionString = Configuration.GetConnectionString("MainConnection");
                opt.UseNpgsql(connectionString);
            });

            services.AddCors();
            services.AddAuthentication()
            .AddJwtBearer("Token", options =>
            {
                AuthOptions settings = Configuration.GetSection("AuthOptions").Get<AuthOptions>();
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    //// строка, представляющая издателя
                    ValidIssuer = settings.Issuer,

                    //// будет ли валидироваться потребитель токена
                    ValidateAudience = true,
                    //// установка потребителя токена
                    ValidAudience = settings.Audience,
                    //// будет ли валидироваться время существования
                    ValidateLifetime = true,

                    // установка ключа безопасности
                    IssuerSigningKey = settings.GetSymmetricSecurityKey(),
                    // валидация ключа безопасности
                    ValidateIssuerSigningKey = true,

                };
            }).AddCookie("Cookies", options => {
                options.LoginPath = new PathString("/Account/Login");
                options.LogoutPath = new PathString("/Account/Logout");
            });

            services
                .AddAuthorization(options =>
                {
                    var cookiePolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .AddAuthenticationSchemes("Cookies")
                        .Build();
                    options.AddPolicy("Cookie", cookiePolicy);
                    options.AddPolicy("Token", new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .AddAuthenticationSchemes("Token")
                        .Build());
                    options.DefaultPolicy = cookiePolicy;
                });

            //services.AddScoped<IRepository<Db.Model.User>, Repository<Db.Model.User>>();
            //services.AddScoped<IRepository<Db.Model.Client>, Repository<Db.Model.Client>>();
            //services.AddScoped<IRepository<Db.Model.Release>, Repository<Db.Model.Release>>();
            //services.AddScoped<IRepository<Db.Model.ReleaseArchitect>, Repository<Db.Model.ReleaseArchitect>>();
            //services.AddScoped<IRepository<Db.Model.UserHistory>, Repository<Db.Model.UserHistory>>();
            //services.AddScoped<IRepository<Db.Model.ClientHistory>, Repository<Db.Model.ClientHistory>>();
            //services.AddScoped<IRepository<Db.Model.ReleaseHistory>, Repository<Db.Model.ReleaseHistory>>();
            //services.AddScoped<IRepository<Db.Model.ReleaseArchitectHistory>, Repository<Db.Model.ReleaseArchitectHistory>>();
            //services.AddDataServices();
            services.AddScoped<IDeployService, DeployService>();
            services.ConfigureAutoMapper();
            services.AddSwaggerGen(swagger =>
            {
                //s.OperationFilter<AddRequiredHeaderParameter>();

                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
            });

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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

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
