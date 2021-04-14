using Authentication.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Services
{
    public class RoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task CreateRole(RoleRequest model)
        {
            foreach(var role in model.Role)
            {
                if(!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public async Task DeleteRole(RoleRequest model)
        {
            foreach (var role in model.Role)
            {
                var result = await _roleManager.FindByNameAsync(role);
                if (result != null)
                {
                    await _roleManager.DeleteAsync(result);
                }
            }
        }

        public IEnumerable<RoleResponse> GetRole()
        {
            return _roleManager.Roles.Select(x => new RoleResponse { Name = x.Name }).ToList();
        }
    }
}
