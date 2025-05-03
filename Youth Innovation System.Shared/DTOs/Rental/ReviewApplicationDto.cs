using System.ComponentModel.DataAnnotations;

namespace Youth_Innovation_System.Shared.DTOs.Rental
{
    public class ReviewApplicationDto
    {
        public int applicationId { get; set; }
        [Required]
        public bool isAccepted { get; set; }
    }
}
