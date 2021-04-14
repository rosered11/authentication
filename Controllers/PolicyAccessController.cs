using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyAccessController : ControllerBase
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

        [Authorize(Policy = "AdminIce")]
        [HttpGet("AdminIceAccess")]
        public IActionResult AdminIceAccess()
        {
            return Ok("Access success.");
        }
    }
}
