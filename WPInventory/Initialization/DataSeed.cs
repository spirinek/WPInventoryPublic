using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPInventory.Data.Models.Entities;

namespace WPInventory.Initialization
{
    public static class DataSeed
    {
        public static User AdminUser => new User
        {
            UserName = "VmmAdmin"
        };

        public static List<Role> AdminRoles => new List<Role>()
        {
            new Role() {Name = "Admin"}
        };

        public static string Password => "Q08cvv1!";
    }
}
