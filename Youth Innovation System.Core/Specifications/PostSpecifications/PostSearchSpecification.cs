using Youth_Innovation_System.Core.Entities;
using Youth_Innovation_System.Core.Roles;

namespace Youth_Innovation_System.Core.Specifications.PostSpecifications
{
    public class PostSearchSpecification : BaseSpecification<CarPost>
    {
        public PostSearchSpecification(string? CarType, decimal? RentalPrice, int pageNumber, int pageSize)
            : base(p => (p.RentalStatus == CarStatus.Accepted.ToString()) &&
                (string.IsNullOrEmpty(CarType) || p.CarType.ToLower().Contains(CarType.ToLower()))
                  || (RentalPrice == null || p.RentalPrice == RentalPrice))
        {
            ApplyPaging((pageNumber - 1) * pageSize, pageSize);
            Includes.Add(p => p.postImages);
        }
        //For total count
        public PostSearchSpecification(string? CarType, decimal? RentalPrice)
            : base(p =>
                (string.IsNullOrEmpty(CarType) || p.CarType.ToLower().Contains(CarType.ToLower()))
                  && (RentalPrice == null || p.RentalPrice == RentalPrice)
                  && (p.RentalStatus == CarStatus.Accepted.ToString()))
        {
        }

    }
}
