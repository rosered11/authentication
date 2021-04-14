using System.ComponentModel.DataAnnotations;

namespace Authentication.Model
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }

    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class AuthenticateResponse
    {
        public string Name { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(string name, string token)
        {
            Name = name;
            Token = token;
        }
    }
}
