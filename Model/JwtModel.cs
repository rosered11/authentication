using System;

namespace Authentication.Model
{
    public class JwtResponse
    {
        public string Token { get; set; }
        public string ExpiredThai { get; set; }
        public string ExpiredJapan { get; set; }
        public JwtResponse(string token, DateTimeOffset expired)
        {
            Token = token;
            ExpiredThai = $"{expired.ToOffset(TimeSpan.FromHours(7))}";
            ExpiredJapan = $"{expired.ToOffset(TimeSpan.FromHours(9))}";
        }
    }
}
