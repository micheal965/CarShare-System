using Youth_Innovation_System.Core.Entities;
using Youth_Innovation_System.Core.Roles;

namespace Youth_Innovation_System.Core.Specifications.PostSpecifications
{
    public class UpdateOrDeletePostSpecification : BaseSpecification<CarPost>
    {
        public UpdateOrDeletePostSpecification(int postId, string userId)
            : base(p => p.Id == postId
                    && p.OwnerId == userId
                    && p.RentalStatus == CarStatus.Accepted.ToString())
        {
            Includes.Add(p => p.postImages);
        }
    }

}
