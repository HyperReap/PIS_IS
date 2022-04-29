using Hotel_PIS.DAL;
using Hotel_PIS.DAL.Dto;
using Hotel_PIS.IServices;
using Microsoft.EntityFrameworkCore;

namespace Hotel_PIS.Services
{
    public class StatisticsRepository : IStatisticsRepository
    {
        public StatisticsDto GetStatistics()
        {
            using(var db = new HotelContext())
            {
                var dbRooms = db.Rooms.Include(e => e.RoomReservations).ToList();

                //TODO nejvytizenehjsi pokoj, prumerna delka pobytu apod..

                var busyRooms = dbRooms.Select(x => new {x.Id, x.RoomNumber, x.RoomReservations.Count }).ToList();
                var mostBusy = busyRooms.OrderByDescending(x => x.Count);

                var dbRR = db.RoomReservations.Select(s => new RoomReservation {Id = s.Id, DateFrom= s.DateFrom.Date, DateTo = s.DateTo.Date });

                int sum = 0;
                foreach (var rr in dbRR)
                    sum += (rr.DateTo.Date - rr.DateFrom.Date).Days;

                float averageLenOfStay = sum/dbRR.Count();

                return new StatisticsDto
                {
                    AverageStay = averageLenOfStay,
                    MostBusyRoomId = mostBusy.First().Id,
                    MostBusyRoomNumber = mostBusy.First().Count,
                };
            }
        }



    }
}
