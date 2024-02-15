using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace WPInventory.Data.Models.Entities
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public Role Role { get; set; }
    }
}
