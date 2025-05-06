using Youth_Innovation_System.Core.Entities;
using Youth_Innovation_System.Core.Roles;

namespace Youth_Innovation_System.Core.Specifications.PostSpecifications
{
    public class GetPendingPostsSpecifications : BaseSpecification<CarPost>
    {
        public GetPendingPostsSpecifications()
            : base(cp => cp.RentalStatus == CarStatus.Pending.ToString())
        {
            Includes.Add(cr => cr.postImages);
        }
    }
}
