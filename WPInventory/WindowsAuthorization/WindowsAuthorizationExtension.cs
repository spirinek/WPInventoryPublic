using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WPInventory.WindowsAuthorization
{
    public static class WindowsAuthorizationExtension
    {
        public static IServiceCollection AddWindowsAuthorization(this IServiceCollection services, IHostEnvironment environment)
        {
            services.AddSingleton<IAuthorizationHandler, AdminGroupHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DomainAdminPolicy", policy =>
                {
                    policy.AddAuthenticationSchemes(IISDefaults.AuthenticationScheme);
                    if (environment.IsDevelopment())
                    {
                        policy.AddRequirements(new AdminGroupRequirement
                        {
                            SSID = "S-1-5-32-559"
                        });
                    }
                    else if (environment.IsProduction())
                    {
                        policy.AddRequirements(new AdminGroupRequirement
                        {
                            SSID = "S-1-5-21-2862394313-3167561015-351291897-512" //domain admins vmm1
                        });
                    }
                    
                });
            });
            return services;
        }
    }
}
