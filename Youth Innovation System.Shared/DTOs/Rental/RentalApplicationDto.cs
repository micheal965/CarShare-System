using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Youth_Innovation_System.Shared.DTOs.Rental
{
    public class RentalApplicationDto
    {
        public int CarId { get; set; }
        [Required]
        public IFormFile licenseFile { get; set; }
        [Required]
        public IFormFile proposalFile { get; set; }
    }
}
