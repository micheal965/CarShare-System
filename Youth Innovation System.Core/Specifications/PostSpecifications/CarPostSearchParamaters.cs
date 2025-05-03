namespace Youth_Innovation_System.Core.Specifications.PostSpecifications
{
    public class CarPostSearchParamaters
    {
        //pagination
        public int pageNumber { get; set; } = 1;
        private int pagesize = 5;
        private int MaxpageSize = 8;
        public int PageSize
        {
            get { return pagesize; }
            set { pagesize = value > MaxpageSize ? MaxpageSize : value; }
        }

        public string CarType { get; set; } // Sedan, SUV, Truck, etc.
        public decimal RentalPrice { get; set; }
    }
}
