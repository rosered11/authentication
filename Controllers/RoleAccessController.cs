using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleAccessController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("AllAccess")]
        public IActionResult AllAccess()
        {
            return Ok("Access success.");
        }

        [HttpGet("AuthorizeAccess")]
        public IActionResult AuthorizeAccess()
        {
            return Ok("Access success.");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("AdminAccess")]
        public IActionResult AdminAccess()
        {
            return Ok("Access success.");
        }

        [Authorize(Roles = "User")]
        [HttpGet("UserAccess")]
        public IActionResult UserAccess()
        {
            return Ok("Access success.");
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("UserOrAdminAccess")]
        public IActionResult UserOrAdminAccess()
        {
            return Ok("Access success.");
        }
    }
}
