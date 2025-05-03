using Youth_Innovation_System.Core.Entities.Identity;
using Youth_Innovation_System.Core.Roles;

namespace Youth_Innovation_System.Core.Specifications.AuthSpecifications
{
    public class GetPendingAndRejectedAccountsSpecifications : BaseSpecification<ApplicationUser>
    {

        public GetPendingAndRejectedAccountsSpecifications()
            : base(u => u.status == UserStatus.pending.ToString() || u.status == UserStatus.rejected.ToString())
        {
        }

    }
}
