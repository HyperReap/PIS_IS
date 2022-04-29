using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_PIS.DAL
{
    public class Failure : BaseModel
    {
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual Room? Room { get; set; }
        public string Description { get; set; }
        public bool IsSolved { get; set; }
    }
}