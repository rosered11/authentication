using Authentication.DataAccess.Model;
using Authentication.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Services
{
    public class UserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            ApplicationUser user = _userManager.Users.SingleOrDefault(x => x.Name == model.Username);

            // return null if user not found
            if (user == null) return null;

            bool isChecked = await _userManager.CheckPasswordAsync(user, model.Password);
            bool isUserLocked = await _userManager.IsLockedOutAsync(user);

            if (isChecked && !isUserLocked)
            {
                var token = await GenerateJwtToken(user);

                var role = await _userManager.GetRolesAsync(user);

                return new AuthenticateResponse(user.Name, token, role);
            }

            return null;
        }

        public async Task<AuthenticateResponse> Create(RegisterRequest model)
        {
            var user = new ApplicationUser { UserName = $"{model.Name}@demo.test", Email = $"{model.Name}@demo.test", Name = model.Name };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddClaimAsync(user, new Claim("Name", user.Name));

                if(model.Role != null && model.Role.Any())
                {
                    await _userManager.AddToRolesAsync(user, model.Role);
                    List<Claim> claims = new List<Claim>();
                    foreach (var item in model.Role)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, item));
                    }

                    // Must to add roles into token, because if token aren't roles, it will be not authorize via method Authorize(Roles = "{something}").
                    await _userManager.AddClaimsAsync(user, claims);
                }

                var token = await GenerateJwtToken(user);

                var role = await _userManager.GetRolesAsync(user);

                return new AuthenticateResponse(user.Name, token, role);
            }
            return null;
        }

        public async Task Delete(CloseAccountRequest model)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Name == model.Name);
            if (user != null)
            {
                if (model.IsRememberPassword)
                {
                    bool isChecked = await _userManager.CheckPasswordAsync(user, model.Password);

                    if (isChecked)
                    {
                        // Wait for implement
                    }
                }

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    var token = GenerateJwtToken(user);
                }
            }
        }

        public async Task<UserLockResponse> LockUser(string username)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Name == username);
            if (user != null)
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now.AddHours(1));
                return await UserLockInfo(username);
            }
            return null;
        }

        public async Task<UserLockResponse> UnLockUser(string username)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Name == username);
            if (user != null)
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now);
                return await UserLockInfo(username);
            }
            return null;
        }

        public async Task<UserLockResponse> UserLockInfo(string username)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Name == username);
            if (user != null)
            {
                var expired = await _userManager.GetLockoutEndDateAsync(user);

                return new UserLockResponse(username, expired);
            }
            return null;
        }

        private async Task<JwtResponse> GenerateJwtToken(ApplicationUser user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("this is my custom Secret key for authnetication");
            var claims = await _userManager.GetClaimsAsync(user);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // This comment can use instead SecurityTokenDescriptor.
            //var tokenJwt = new JwtSecurityToken(expires: expired, claims: claims, signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature));
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return new JwtResponse(tokenHandler.WriteToken(token), DateTime.SpecifyKind(token.ValidTo, DateTimeKind.Utc));
        }
    }
}
