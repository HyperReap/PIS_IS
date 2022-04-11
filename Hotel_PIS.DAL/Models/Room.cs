namespace Hotel_PIS.DAL
{
    public class Room : BaseModel
    {
        public Room(ICollection<RoomReservation> roomReservations)
        {
            RoomReservations = roomReservations;
        }
        public int NumberOfBeds { get; set; }
        public int NumberOfSideBeds { get; set; }
        public double RoomSize { get; set; }
        public int Floor { get; set; }
        public int RoomNumber { get; set; }
        public bool IsCleaned { get; set; }
        public ICollection<RoomReservation> RoomReservations { get; set; }

    }
}