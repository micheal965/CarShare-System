using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Youth_Innovation_System.Shared.DTOs.Rental
{
    public class RentalApplicationDto
    {
        public int CarId { get; set; }
        [Required]
        public IFormFile LicenseFile { get; set; }
        [Required]
        public IFormFile ProposalFile { get; set; }
    }
}
