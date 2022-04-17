namespace Hotel_PIS.DAL
{
    public class Equipment : BaseModel
    {
        public Equipment()
        {
            RoomEquipments = new  HashSet<RoomEquipment>();
        }
        public string Name { get; set; }
        public ICollection<RoomEquipment> RoomEquipments { get; set; }
    }
}