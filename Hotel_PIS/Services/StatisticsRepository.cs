using Hotel_PIS.DAL;
using Hotel_PIS.DAL.Dto;
using Hotel_PIS.IServices;
using Microsoft.EntityFrameworkCore;

namespace Hotel_PIS.Services
{
    public class StatisticsRepository : IStatisticsRepository
    {
        public StatisticsDto getStats()
        {
            throw new NotImplementedException();
            using(var db = new HotelContext())
            {
                var dbRooms = db.Rooms.Include(e => e.RoomReservations).ToList();

                //TODO nejvytizenehjsi pokoj, prumerna delka pobytu apod..

                var busyRooms = dbRooms.Select(x => new { x.RoomNumber, x.RoomReservations.Count }).ToList();
                var mostBusy = busyRooms.Max(x => x.Count);

                //var dbRR = db.RoomReservations.


            }
        }



    }
}
