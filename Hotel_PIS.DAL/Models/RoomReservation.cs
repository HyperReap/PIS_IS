using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_PIS.DAL
{
    public class RoomReservation : BaseModel
    {
        public int ReservationId { get; set; }
        [ForeignKey("ReservationId")]
        public virtual Reservation Reservation { get; set; }
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}