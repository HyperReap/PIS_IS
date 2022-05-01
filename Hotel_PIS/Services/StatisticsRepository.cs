using Hotel_PIS.DAL;
using Hotel_PIS.DAL.Dto;
using Hotel_PIS.IServices;
using Microsoft.EntityFrameworkCore;

namespace Hotel_PIS.Services
{
    public class StatisticsRepository : IStatisticsRepository
    {
        /// <summary>
        /// prasarna fce, ale strasne se mi kvuli skolnimu projektu to nechce refaktorovat :D
        /// </summary>
        /// <returns></returns>
        public StatisticsDto GetStatistics()
        {
            using(var db = new HotelContext())
            {
                var dbRooms = db.Rooms.Include(e => e.RoomReservations).ToList();

                var busyRooms = dbRooms.Select(x => new {x.Id, x.RoomNumber, x.RoomReservations.Count }).ToList();
                var mostBusy = busyRooms.OrderByDescending(x => x.Count);

                var dbRR = db.RoomReservations.Select(s => new RoomReservation {Id = s.Id, DateFrom= s.DateFrom.Date, DateTo = s.DateTo.Date });

                int sum = 0;
                int tmpMax = 0;
                foreach (var rr in dbRR)
                {
                    var diff = (rr.DateTo.Date - rr.DateFrom.Date).Days;
                    sum += diff;
                    if(tmpMax < diff)
                        tmpMax = diff;
                }

                float averageLenOfStay = sum/dbRR.Count();

                int longestStay = tmpMax;

                var reservationClients = db.Reservations.Include(x => x.Client).Select(x=>x.Client).ToList();//list of clients with duplicities

                    var rcs = reservationClients.MaxBy(x=>x.Email); //collection of 1 client with most records


                var roomWithMostFailures = db.Rooms.Include(e => e.Failures).OrderBy(x => x.Failures.Count).FirstOrDefault();

                var employees = db.Employees.Where(x => x.ContractDueDae != null && x.ContractDueDae <= DateTime.Now.AddMonths(3)).Take(6).ToList();




                return new StatisticsDto
                {
                    AverageStay = averageLenOfStay,
                    LongestStay = longestStay,

                    NotWantedRoomId = mostBusy.Last().Id,
                    NotWantedRoomNumber = mostBusy.Last().RoomNumber,
                    NotWantedRoomCount = mostBusy.Last().Count,

                    MostBusyRoomId = mostBusy.First().Id,
                    MostBusyRoomNumber = mostBusy.First().RoomNumber,
                    MostBusyRoomCount = mostBusy.First().Count,

                    RoomWithMostFailuresId = mostBusy.First().Id,
                    RoomWithMostFailuresNumber = mostBusy.First().RoomNumber,
                    RoomWithMostFailuresCount = mostBusy.First().Count,

                    mostLoyalClient = rcs.Email,
                    EmployeesWithEndingContract = employees,
                };
            }
        }



    }
}
