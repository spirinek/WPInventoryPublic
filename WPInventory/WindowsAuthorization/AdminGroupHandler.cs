using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace WPInventory.WindowsAuthorization
{
    public class AdminGroupHandler : AuthorizationHandler<AdminGroupRequirement>
    {
        private readonly ILogger<AdminGroupHandler> _logger;

        public AdminGroupHandler(ILogger<AdminGroupHandler> logger)
        {
            _logger = logger;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminGroupRequirement requirement)
        {
            var groups = new List<string>();
            var windowsInfo = (WindowsIdentity)context.User.Identity;
            if (windowsInfo.Groups != null)
            {
                _logger.LogCritical(requirement.SSID);
                foreach (var group in windowsInfo.Groups)
                {
                    try
                    {
                        groups.Add(group.Value);
                        _logger.LogCritical(group.Value);
                    }
                    catch (Exception e)
                    {
                        _logger.LogCritical(e.Message);
                    }
                }
            }
            if (groups.Contains(requirement.SSID))
            {
                context.Succeed(requirement);
            }
            
            return Task.CompletedTask;
        }
    }
}
