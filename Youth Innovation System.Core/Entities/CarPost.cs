using System.ComponentModel.DataAnnotations;
using Youth_Innovation_System.Core.Roles;

namespace Youth_Innovation_System.Core.Entities
{
    public class CarPost : BaseEntity
    {

        [Required]
        public string OwnerId { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public string CarType { get; set; } // Sedan, SUV, Truck, etc.

        [Required]
        public string Brand { get; set; } // Toyota, BMW, etc.

        [Required]
        public string Model { get; set; } // Camry, X5, etc.

        [Required]
        public int Year { get; set; }

        [Required]
        public string Transmission { get; set; } // Automatic or Manual

        [Required]
        public string Location { get; set; } // City, Country

        [Required]
        public string RentalStatus { get; set; } = CarStatus.Pending.ToString();

        [Required]
        public DateTime AvailabilityStart { get; set; }

        [Required]
        public DateTime AvailabilityEnd { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal RentalPrice { get; set; }

        //Navigation properties
        public List<PostImage> postImages { get; set; } = new();
        public List<RentalApplication> RentalApplications { get; set; } = new();
        public List<CarFeedback> CarFeedbacks { get; set; } = new();
    }
}
