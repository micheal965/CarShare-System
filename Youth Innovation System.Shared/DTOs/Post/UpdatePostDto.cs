using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Youth_Innovation_System.Shared.DTOs.Post
{
    public class UpdatePostDto
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public string CarType { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        public string Transmission { get; set; }

        public string Location { get; set; }

        public string RentalStatus { get; set; }

        public DateTime AvailabilityStart { get; set; }

        public DateTime AvailabilityEnd { get; set; }

        [Range(0, double.MaxValue)]
        public decimal RentalPrice { get; set; }
        public List<IFormFile>? Images { get; set; }

    }
}
