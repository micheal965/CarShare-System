using System.Linq.Expressions;

namespace Youth_Innovation_System.Core.Specifications
{
    public interface ISpecification<T> where T : class
    {
        //Filteration
        Expression<Func<T, bool>> Criteria { get; set; }
        //Includes
        List<Expression<Func<T, object>>> Includes { get; set; }

        //Orderby
        Expression<Func<T, object>> OrderByAsc { get; set; }
        Expression<Func<T, object>> OrderByDesc { get; set; }
        int? Take { get; set; }
        int? Skip { get; set; }
        bool IsPagingEnabled { get; set; }
    }
}
