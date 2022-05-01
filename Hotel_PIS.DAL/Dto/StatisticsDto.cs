using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_PIS.DAL.Dto
{
    public  class StatisticsDto
    {
        /// <summary>
        /// most reservation ordered for this room
        /// </summary>
        public int MostBusyRoomNumber { get; set; }
        public int MostBusyRoomId { get; set; }
        public int MostBusyRoomCount { get; set; }
        /// <summary>
        /// average of all reservations date range
        /// </summary>
        public float AverageStay { get; set; }
        /// <summary>
        /// most reservations ordered
        /// </summary>
        public string mostLoyalClient { get; set; }

        /// <summary>
        /// least wanted room, least number of reservations
        /// </summary>
        public int NotWantedRoomNumber { get; set; }
        public int NotWantedRoomId { get; set; }
        public int NotWantedRoomCount { get; set; }
        public int RoomWithMostFailuresCount { get; set; }
        public int RoomWithMostFailuresNumber { get; set; }
        public int RoomWithMostFailuresId { get; set; }
        public List<Employee> EmployeesWithEndingContract { get; set; }
        public int LongestStay { get; set; }
    }
}
