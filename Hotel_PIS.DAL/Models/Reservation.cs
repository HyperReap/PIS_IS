namespace Hotel_PIS.DAL
{
    public class Reservation : BaseModel
    {
        public int NumberOfPeople { get; set; }
        public Decimal Cost { get; set; }
        public bool IsPayed { get; set; }

        public ReservationStateEnum ReservationState { get; set; }
    }
}