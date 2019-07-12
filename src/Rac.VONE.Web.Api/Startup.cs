using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Rac.VOne.Web.Api.Middleware;

namespace Rac.VOne.Web.Api
{


    /// <summary>
    /// Web Api の初期設定
    /// 
    /// Swagger の configuration
    /// Middleware / Filter の設定
    /// DI の設定
    /// 適切な単位に 処理を分ける
    /// </summary>
    public class Startup
    {


        //private Container container = new Container();

        private readonly IConfiguration configuration;

        /// <summary></summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services
                .ConfigureDIContainer()
                .ConfigureProcessors()
                .ConfigureQueryProcessors()
                .ConfigureImportProcessors()
                .ConfigureSpreadsheetProcessors()
                ;

            services.AddMvc() // add framework services options => options.Filters.Add...
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(x => x.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore);

            services.AddCors(options => options.AddPolicy("AllowAnyOriginMethodHeader",
                    builder => builder
                        /* TODO: set appropriate origin */
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        /* TODO: set appropriate header */
                        .AllowAnyHeader()
                        .AllowCredentials()
                        ));


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "V-One-Next Web API", Version = "v1" });
                c.IncludeXmlComments($@"{AppDomain.CurrentDomain.BaseDirectory}\Rac.VOne.Web.Api.xml");

            });

            services.AddSignalR();
        }

        private bool IsLoginApi(HttpContext context)
            => context.Request.Path.StartsWithSegments("/api/login");

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // Add custom middleware
            app
                .UseWhen(context => IsLoginApi(context),
                configuration => configuration
                    .UseStrictIPAddressFilteringMiddleware()
                    )
                .UseExceptionHandlingMiddleware()
                    ;

            app.UseHttpsRedirection();

            app.UseCors("AllowAnyOriginMethodHeader");
            app.UseSignalR(routes => routes.MapHub<Hubs.ProgressHub>("/progress"));
            app.UseMvc(routes
                => routes.MapRoute("default", "api/{controller}/{action}/{id?}"));

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });

        }
    }
}
