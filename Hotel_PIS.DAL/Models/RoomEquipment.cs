using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_PIS.DAL
{
    public class RoomEquipment :BaseModel
    {
        public int RoomId { get; set; }
        [ForeignKey(nameof(RoomId))]
        public virtual Room Room { get; set; }

        public int EquipmentId { get; set; }
        [ForeignKey(nameof(EquipmentId))]
        public virtual Equipment Equipment { get; set; }
    }
}
