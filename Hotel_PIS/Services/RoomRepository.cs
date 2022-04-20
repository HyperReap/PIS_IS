using Hotel_PIS.DAL;
using Hotel_PIS.IServices;
using Microsoft.EntityFrameworkCore;

namespace Hotel_PIS.Services
{
    public class RoomRepository : IRoomRepository
    {
        public bool Delete(int id)
        {
            using (var db = new HotelContext())
            {
                try
                {
                    var room = db.Rooms.FirstOrDefault(x => x.Id == id);
                    db.Entry(room).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        public Room Get(int id)
        {
            using (var db = new HotelContext())
            {
                var room = db.Rooms.Where(x => x.Id == id).FirstOrDefault();
                if (room == null)
                    throw new Exception($"Room with id:'{id}' was not found in database.");

                return room;
            }

        }

        public List<Room> GetAll()
        {
            using (var db = new HotelContext())
            {
                var rooms = db.Rooms.ToList();
                return rooms;
            }
        }

        public Room Save(int id, Room obj)
        {
            throw new NotImplementedException();
            Room savedClient;

            if (id == 0) // Create
            {
                savedClient = CreateNew(obj);
            }
            else
            {
                savedClient = Update(obj);
            }

            return savedClient;
        }

        public Room CreateNew(Room client)
        {
            using (var db = new HotelContext())
            {
                db.Rooms.Add(client);
                db.SaveChanges();

                return client;
            }
        }

        public List<Room> GetFiltered(EquipmentsList equipmentsList, DateTime? from, DateTime? to, decimal? minPrice, decimal? maxPrice, int? minNumberOfBeds, int? maxNumberOfBeds)
        {
            var equipments = equipmentsList.Equipments;

            bool useDates = from != null && to != null;

            var dateFrom = from == null ? new DateTime(01, 01, 01) : from;
            var dateTo = to == null ? new DateTime(2222, 01, 01) : to;

            using (var db = new HotelContext())
            {

                var rooms = db.Rooms
                    .Include(e => e.RoomEquipments).ThenInclude(e => e.Equipment)
                    .Include(e => e.RoomReservations).ThenInclude(e => e.Reservation)
                    .ToList();

                var tmp = rooms.Where(x =>
                 (equipments is null || equipments.Count == 0 || x.RoomEquipments.Any(re => equipments.Contains(re.Equipment)))
                 && (minPrice is null || minPrice <= x.CostPerNight)
                 && (maxPrice is null || maxPrice >= x.CostPerNight)
                 && (minNumberOfBeds is null || minNumberOfBeds <= x.NumberOfBeds)
                 && (maxNumberOfBeds is null || maxNumberOfBeds >= x.NumberOfBeds)
                     ).ToList();

                tmp = tmp.Where(x=>
                  (useDates && x.RoomReservations.Any(r => r.Reservation.ReservationState != ReservationStateEnum.Canceled &&
                  (  (dateFrom <= r.DateFrom && dateTo >= r.DateTo) //reservation= 4-10; filter=3-11
                  || (dateFrom <= r.DateFrom && dateTo <= r.DateTo) //reservation= 4-10; filter=1-5
                  || (dateFrom >= r.DateFrom && dateTo >= r.DateTo) //reservation= 4-10; filter=5-11
                  || (dateFrom >= r.DateFrom && dateTo <= r.DateTo) //reservation= 4-10; filter=3-11
                  ))
                )).ToList();


                return tmp;
            }
        }

        public Room Update(Room room)
        {
            using (var db = new HotelContext())
            {
                var dbRoom = db.Rooms.Where(x => x.Id == room.Id).FirstOrDefault();
                if (dbRoom == null)
                    throw new Exception($"Room with id:'{room.Id}' was not found in database.");

                if (dbRoom.Equals(room))
                    return room;


                db.SaveChanges();

                return room;
            }
        }
    }
}
