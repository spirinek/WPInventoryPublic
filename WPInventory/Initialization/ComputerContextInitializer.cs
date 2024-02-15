using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WPInventory.Data;
using WPInventory.Data.DataSeed;
using WPInventory.Data.Models.Entities;

namespace WPInventory.Initialization
{
    class ComputerContextInitializer : DataInitializer<ComputerInfoContext>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public ComputerContextInitializer(ComputerInfoContext dbContext, UserManager<User> userManager, RoleManager<Role> roleManager) :base(dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        protected override async Task DataInit()
        {
            await AddfirstAdminUser(DataSeed.AdminRoles, DataSeed.AdminUser,DataSeed.Password);
        }

        private async Task AddfirstAdminUser(List<Role> roles, User adminUser, string password=null)
        {
            if (await _userManager.FindByNameAsync(adminUser.UserName) == null)
            {
              var creationResult =  await _userManager.CreateAsync(adminUser, password);
              if (!creationResult.Succeeded)
              {
                  throw new Exception(string.Join("", creationResult.Errors.Select(x => $"[{x.Code} {x.Description}] \n")));
              }
            }

            var user = await _userManager.FindByNameAsync(adminUser.UserName);

            foreach (var role in roles)
            {
                if (await _roleManager.FindByNameAsync(role.Name) == null)
                {
                    await _roleManager.CreateAsync(role);
                }

                await _userManager.AddToRoleAsync(user, role.Name);
            }
        }
    }
}
