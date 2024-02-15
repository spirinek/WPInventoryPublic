using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WPInventory.WindowsAuthorization;
using Microsoft.OpenApi.Models;
using WPInventory.BL;
using WPInventory.BL.Computers;
using WPInventory.Data;
using WPInventory.Data.Models.Entities;

namespace WPInventory
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Environment = environment;
            Configuration = configuration;
        }
        public IHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ComputerInfoContext>(options => options.UseSqlServer(connection));
            //services.AddHostedService<TimeHostedService>(); - убрал в отдельный воркер.
            services.AddLogging();

            services.AddRazorPages().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddPageRoute("/Login", "/signin");
                options.Conventions.AddPageRoute("/Logout", "/signout");
                options.Conventions.AddPageRoute("/AccesDenied", "/accessdenied");
                options.Conventions.AuthorizePage("/Index");
                options.Conventions.AddPageRoute("/Index", "{*url:regex(^(?!api|doc|swagger).*$)}");
            });

            var assembly = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().FullName.StartsWith("WPInventory.BL")).ToArray();
            services.AddMediatR(assembly);
            services.AddAutoMapper(assembly);

            services.AddIdentity<User, Role>().AddEntityFrameworkStores<ComputerInfoContext>();
            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                options.LogoutPath = new PathString("/signout");
                options.AccessDeniedPath = new PathString("/Accessdenied");
                options.LoginPath = new PathString("/signin");
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    if (context.Request.Path.StartsWithSegments("/api") &&
                        context.Response.StatusCode == StatusCodes.Status200OK)
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    else
                        context.Response.Redirect(context.RedirectUri);
                    return Task.FromResult(0);
                };
                options.Events.OnRedirectToLogin = context =>
                {
                    if (context.Request.Path.StartsWithSegments("/api") &&
                        context.Response.StatusCode == StatusCodes.Status200OK)
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    else
                        context.Response.Redirect(context.RedirectUri);
                    return Task.FromResult(0);
                };
            });

            services.AddWindowsAuthorization(Environment);

            services.AddSwaggerGen(c =>
                    {
                        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                    });
            services.AddMvcCore()
                .AddApiExplorer();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.UseRouting();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }
            
            //app.UseHttpsRedirection();
            //app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
