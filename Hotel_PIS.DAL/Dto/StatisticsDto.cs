using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_PIS.DAL.Dto
{
    public  class StatisticsDto
    {
        public int MostBusyRoomNumber { get; set; }
        public int MostBusyRoomId { get; set; }
        public float AverageStay { get; set; }
    }
}
