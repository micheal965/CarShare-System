namespace Youth_Innovation_System.Shared.DTOs.Rental
{
    public class RentalApplicationResponseDto
    {
        public int Id { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string LicenseFileUrl { get; set; }
        public string ProposalFileUrl { get; set; }
        public string Status { get; set; }
    }
}
