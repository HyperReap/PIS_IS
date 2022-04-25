using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_PIS.DAL.Dto
{
    public class EquipmentListDto
    {
        public EquipmentListDto()
        {
            Equipments = new List<Equipment>();
        }
        public List<Equipment> Equipments { get; set; }

    }
}
