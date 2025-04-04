using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Youth_Innovation_System.Shared.DTOs.Post
{
    public class CreatePostDto
    {
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public string CarType { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public string Transmission { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string RentalStatus { get; set; }

        [Required]
        public DateTime AvailabilityStart { get; set; }

        [Required]
        public DateTime AvailabilityEnd { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal RentalPrice { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}
