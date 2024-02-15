using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WPInventory.Data.Models.Entities
{
    public class User : IdentityUser <Guid>
    {
        public List<UserRole> Roles { get; set; }
    }
}
