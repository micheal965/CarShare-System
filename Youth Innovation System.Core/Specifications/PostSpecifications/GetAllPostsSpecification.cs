using Youth_Innovation_System.Core.Entities;
using Youth_Innovation_System.Core.Roles;

namespace Youth_Innovation_System.Core.Specifications.PostSpecifications
{
    public class GetAllPostsSpecification : BaseSpecification<CarPost>
    {
        public GetAllPostsSpecification(int pageNumber, int pageSize)
            : base(p => p.RentalStatus == CarStatus.Accepted.ToString())
        {
            ApplyPaging((pageNumber - 1) * pageSize, pageSize);
            Includes.Add(p => p.postImages);
            Includes.Add(p => p.CarFeedbacks);
        }
        //For total count
        public GetAllPostsSpecification()
            : base(p => p.RentalStatus == CarStatus.Accepted.ToString())
        {

        }
    }
}
