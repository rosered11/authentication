using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Services
{
    public class JwtService
    {
        public static bool SetPolicyAdminForUserIce(AuthorizationHandlerContext context)
        {
            return context.User.IsInRole("Admin") && context.User.HasClaim(c => c.Type == "Name" && c.Value == "ice");
        }
    }
}
