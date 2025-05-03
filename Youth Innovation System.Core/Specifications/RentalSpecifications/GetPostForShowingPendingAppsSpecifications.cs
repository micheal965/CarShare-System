using Youth_Innovation_System.Core.Entities;

namespace Youth_Innovation_System.Core.Specifications.RentalSpecifications
{
    public class GetPostForShowingPendingAppsSpecifications : BaseSpecification<CarPost>
    {
        public GetPostForShowingPendingAppsSpecifications(string userId, int carPostId)
            : base(cp => cp.Id == carPostId && cp.OwnerId == userId)
        {
            Includes.Add(cp => cp.RentalApplications);
        }
    }
}
