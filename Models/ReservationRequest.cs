namespace RentACar.Api.Models
{
    public class ReservationRequest
    {
        public int CarId { get; set; }
        public string CustomerName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
