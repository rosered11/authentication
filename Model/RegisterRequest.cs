using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Authentication.Model
{
    public class RegisterRequest
    {
        public string Password { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Role { get; set; }
    }

    public class CloseAccountRequest
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsRememberPassword { get; set; }
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
        public JwtResponse Token { get; set; }
        public IEnumerable<string> Role { get; set; }


        public AuthenticateResponse(string name, JwtResponse token, IEnumerable<string> role)
        {
            Name = name;
            Token = token;
            Role = role;
        }
    }

    public class UserLockResponse
    {
        public string Name { get; set; }
        public string ExpiredThai { get; set; }
        public string ExpiredJapan { get; set; }

        public UserLockResponse(string name, DateTimeOffset? expired)
        {
            Name = name;
            ExpiredThai = $"{expired?.ToOffset(TimeSpan.FromHours(7))}";
            ExpiredJapan = $"{expired?.ToOffset(TimeSpan.FromHours(9))}";
        }
    }
}
