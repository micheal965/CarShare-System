namespace Youth_Innovation_System.Shared.DTOs.Post
{
    public class PostResponseDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string CarType { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        public string Transmission { get; set; }

        public string Location { get; set; }

        public string RentalStatus { get; set; }

        public DateTime AvailabilityStart { get; set; }

        public DateTime AvailabilityEnd { get; set; }

        public decimal RentalPrice { get; set; }
        public IReadOnlyList<string> ImageUrls { get; set; }
        //Feedback
        public IReadOnlyList<CarFeedbackDto> Feedbacks { get; set; }

    }

}
