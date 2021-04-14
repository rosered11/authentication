using Authentication.Model;
using Authentication.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserService _userService;

        public AccountController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Create(model);
                return Ok(result);
            }
            return BadRequest("Faild");
        }

        [HttpPost("GetToken")]
        public async Task<IActionResult> GetToken(AuthenticateRequest model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Authenticate(model);
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
