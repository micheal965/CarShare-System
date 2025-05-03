using System.ComponentModel.DataAnnotations.Schema;
using Youth_Innovation_System.Core.Roles;

namespace Youth_Innovation_System.Core.Entities
{
    public class RentalApplication : BaseEntity
    {
        public string RenterId { get; set; } // Link to User table manually instead of foreign key

        [ForeignKey("carPost")]
        public int PostId { get; set; }
        public CarPost carPost { get; set; }
        public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;
        public string LicenseFileUrl { get; set; }
        public string ProposalFileUrl { get; set; }
        public string Status { get; set; } = RentalAppStatus.Pending.ToString(); // Pending, Approved, Rejected
    }
}
