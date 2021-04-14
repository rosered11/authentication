using Authentication.DataAccess.Model;
using Authentication.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
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

            if (isChecked)
            {
                var token = GenerateJwtToken(user);

                return new AuthenticateResponse(user.Name, token);
            }

            return null;
        }

        public async Task<AuthenticateResponse> Create(RegisterRequest model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Name = model.Name };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var token = GenerateJwtToken(user);

                return new AuthenticateResponse(user.Name, token);
            }
            return null;
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("mysecret");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("Name", user.Name) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
