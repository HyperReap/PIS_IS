using System.ComponentModel.DataAnnotations.Schema;

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

        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        [InverseProperty("Reservations")]
        public virtual Client? Client { get; set; }

        public ReservationStateEnum ReservationState { get; set; }
        public ICollection<RoomReservation> RoomReservations { get; set; }

    }
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
    }
}