namespace Hotel_PIS.DAL
{
    public class Room : BaseModel
    {
        public Room()
        {
            RoomReservations = new HashSet<RoomReservation>();
            RoomEquipments = new HashSet<RoomEquipment>();
            Failures = new HashSet<Failure>();
        }
        public int NumberOfBeds { get; set; }
        public int NumberOfSideBeds { get; set; }
        public double RoomSize { get; set; }
        public int Floor { get; set; }
        public int RoomNumber { get; set; }
        public bool IsCleaned { get; set; }
        public string Picture { get; set; }
        public int CostPerNight { get; set; }



        public ICollection<Failure> Failures { get; set; }
        public ICollection<RoomEquipment> RoomEquipments { get; set; }
        public ICollection<RoomReservation> RoomReservations { get; set; }

    }
}