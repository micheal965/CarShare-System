using Youth_Innovation_System.Core.Entities;
using Youth_Innovation_System.Core.Roles;

namespace Youth_Innovation_System.Core.Specifications.PostSpecifications
{
    public class GetAllUserPosts : BaseSpecification<CarPost>
    {
        //For pagination
        public GetAllUserPosts(string userId, int pageNumber, int pageSize)
        : base(p => p.OwnerId == userId && p.RentalStatus == CarStatus.Accepted.ToString())
        {
            ApplyPaging((pageNumber - 1) * pageSize, pageSize);
            Includes.Add(p => p.postImages);
        }
        //For total count
        public GetAllUserPosts(string userId)
            : base(p => p.OwnerId == userId && p.RentalStatus == CarStatus.Accepted.ToString())
        {

        }
    }
}
