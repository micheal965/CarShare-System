using System.ComponentModel.DataAnnotations;

namespace Youth_Innovation_System.Shared.DTOs.Identity.Roles
{
    public class ApproveAccountDto
    {
        [Required]
        public string userId { get; set; }
        [Required]
        public bool IsApproved { get; set; }
    }
}
