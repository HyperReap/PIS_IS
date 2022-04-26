namespace Hotel_PIS.DAL.Dto
{
    public class ReservationDto
    {
        public int RoomId { get; set; }
        public int NumberOfPeople { get; set; }
        public DateTime DateTo { get; set; }
        public DateTime DateFrom { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public int RoomNumber { get; set; }
        public decimal Cost { get; set; }
        public decimal Paid { get; set; }
        public int ReservationId { get; set; }
        public ReservationStateEnum ReservationState { get; set; }
    }
}
