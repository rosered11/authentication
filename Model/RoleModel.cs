using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Model
{
    public class RoleRequest
    {
        public IEnumerable<string> Role { get; set; }
    }


    public class RoleResponse
    {
        public string Name { get; set; }
    }
}
