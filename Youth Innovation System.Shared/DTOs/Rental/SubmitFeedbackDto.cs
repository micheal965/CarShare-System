using System.ComponentModel.DataAnnotations;

namespace Youth_Innovation_System.Shared.DTOs.Rental
{
    public class SubmitFeedbackDto
    {
        public int carPostId { get; set; }
        [Required]
        public int rating { get; set; }
        [Required]
        public string feedback { get; set; }
    }
}
