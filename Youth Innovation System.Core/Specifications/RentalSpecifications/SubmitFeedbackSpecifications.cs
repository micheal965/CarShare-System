using Youth_Innovation_System.Core.Entities;
using Youth_Innovation_System.Core.Roles;

namespace Youth_Innovation_System.Core.Specifications.RentalSpecifications
{
    public class SubmitFeedbackSpecifications : BaseSpecification<RentalApplication>
    {
        public SubmitFeedbackSpecifications(string userId, int carPostId)
            : base(p => p.RenterId == userId
                 && p.PostId == carPostId
                 && p.Status == RentalAppStatus.Approved.ToString())
        {
            Includes.Add(p => p.carPost);
        }

    }
}
