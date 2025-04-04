using Youth_Innovation_System.Core.Entities;
using Youth_Innovation_System.Core.Roles;

namespace Youth_Innovation_System.Core.Specifications.PostSpecifications
{
    public class GetPostSpecification : BaseSpecification<CarPost>
    {
        public GetPostSpecification(int postId)
        : base(p => p.Id == postId && p.RentalStatus == CarStatus.Accepted.ToString())
        {
            Includes.Add(p => p.postImages);
            Includes.Add(p => p.CarFeedbacks);
        }
    }
}
