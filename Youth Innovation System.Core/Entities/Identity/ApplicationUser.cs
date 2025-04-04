using Microsoft.AspNetCore.Identity;
using Youth_Innovation_System.Core.Roles;

namespace Youth_Innovation_System.Core.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string? pictureUrl { get; set; }
        //for tracking ip address for each login
        public List<UserLoginHistory> userLoginsHistory { get; set; }
        public List<RefreshToken> refreshTokens { get; set; }
        public string status { get; set; } = UserStatus.pending.ToString();
    }
}
