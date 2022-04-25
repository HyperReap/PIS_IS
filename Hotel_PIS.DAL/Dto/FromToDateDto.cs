using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_PIS.DAL.Dto
{
    /// <summary>
    /// used for transfering BookedReservations times for calendar
    /// </summary>
    public class FromToDateDto
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
