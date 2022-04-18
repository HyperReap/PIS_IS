namespace Hotel_PIS.DAL
{
    public class Reservation : BaseModel
    {
        public Reservation()
        {
            RoomReservations = new HashSet<RoomReservation>();
        }

        public int NumberOfPeople { get; set; }
        public Decimal Cost { get; set; }
        public Decimal Payed { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public ReservationStateEnum ReservationState { get; set; }
        public ICollection<RoomReservation> RoomReservations { get; set; }

    }
}