using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_PIS.DAL.Dto
{
    public  class FailureDto
    {
        public int? RoomId { get; set; }
        public int? RoomNumber { get; set; }
        public string Description { get; set; }
        public bool? IsSolved { get; set; }
    }
}
