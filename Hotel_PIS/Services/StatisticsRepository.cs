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

                //TODO  rooms none wants

                var busyRooms = dbRooms.Select(x => new {x.Id, x.RoomNumber, x.RoomReservations.Count }).ToList();
                var mostBusy = busyRooms.OrderByDescending(x => x.Count);

                var dbRR = db.RoomReservations.Select(s => new RoomReservation {Id = s.Id, DateFrom= s.DateFrom.Date, DateTo = s.DateTo.Date });

                int sum = 0;
                foreach (var rr in dbRR)
                    sum += (rr.DateTo.Date - rr.DateFrom.Date).Days;

                float averageLenOfStay = sum/dbRR.Count();


                var reservationClients = db.Reservations.Include(x => x.Client).Select(x=>x.Client).ToList();//list of clients with duplicities

                    var rcs = reservationClients.MaxBy(x=>x.Email); //collection of 1 client with most records


                return new StatisticsDto
                {
                    AverageStay = averageLenOfStay,
                    NotWantedRoomId = mostBusy.Last().Id,
                    NotWantedRoomNumber = mostBusy.Last().RoomNumber,
                    NotWantedRoomCount = mostBusy.Last().Count,
                    MostBusyRoomId = mostBusy.First().Id,
                    MostBusyRoomNumber = mostBusy.First().RoomNumber,
                    MostBusyRoomCount = mostBusy.First().Count,
                    mostLoyalClient = rcs.Email,
                };
            }
        }



    }
}
