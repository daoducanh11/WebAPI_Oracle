using Microsoft.AspNetCore.Identity;

namespace WebAPI_Oracle.Entities
{
    public class User : IdentityUser
    {
        public int UserId { get; set; }
        public string Password { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
