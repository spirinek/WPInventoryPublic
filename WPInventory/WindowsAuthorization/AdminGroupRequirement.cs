using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WPInventory.WindowsAuthorization
{
    public class AdminGroupRequirement : IAuthorizationRequirement
    {
        public string SSID { get; set; }
    }
}
