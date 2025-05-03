using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Youth_Innovation_System.Core.Entities
{
    public class CarFeedback : BaseEntity
    {
        [ForeignKey("CarPost")]
        public int CarPostId { get; set; }
        public string RenterId { get; set; }//insert it manually instead of foreign key

        // Feedback Content
        public string FeedbackText { get; set; }
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }
        public DateTime SubmmitedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public CarPost CarPost { get; set; }
    }

}

