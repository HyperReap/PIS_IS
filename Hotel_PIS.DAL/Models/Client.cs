using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_PIS.DAL
{
    public class Client : BasePerson
    {
        public string PhoneNumber { get; set; }
        public string? IdentityCardNumber { get; set; }
        [InverseProperty("Client")]
        public List<Reservation>? Reservations { get; set; }
    }
}