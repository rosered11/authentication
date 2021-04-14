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
        private readonly RoleService _roleService;

        public AccountController(UserService userService, RoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
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

        [HttpPost("CloseAccount")]
        public async Task<IActionResult> Delete(CloseAccountRequest model)
        {
            if (ModelState.IsValid)
            {
                await _userService.Delete(model);
                return Ok();
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

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(RoleRequest model)
        {
            if (ModelState.IsValid)
            {
                await _roleService.CreateRole(model);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("DeleteRole")]
        public async Task<IActionResult> DeleteRole(RoleRequest model)
        {
            if (ModelState.IsValid)
            {
                await _roleService.DeleteRole(model);
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("GetAllRole")]
        public async Task<IActionResult> GetAllRole()
        {
            return Ok(await Task.FromResult(_roleService.GetRole()));
        }

        [HttpPost("LockUser")]
        public async Task<IActionResult> LockUser(string username)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LockUser(username);
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpPost("UnLockUser")]
        public async Task<IActionResult> UnLockUser(string username)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.UnLockUser(username);
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
