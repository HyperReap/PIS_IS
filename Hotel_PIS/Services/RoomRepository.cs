using Hotel_PIS.DAL;
using Hotel_PIS.DAL.Dto;
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
        
        public List<Room> GetAllUncleaned()
        {
            using (var db = new HotelContext())
            {
                var rooms = db.Rooms.Where(x => !x.IsCleaned).ToList();
                return rooms;
            }
        }
        
        public List<Room> GetAllCleaned()
        {
            using (var db = new HotelContext())
            {
                var rooms = db.Rooms.Where(x => x.IsCleaned).ToList();
                return rooms;
            }
        }

        private bool AssignEquipmentsToRoom(int roomId, List<int> equipmentIds)
        {
            using (var db = new HotelContext())
            {
                foreach (var eqId in equipmentIds)
                {
                    db.RoomEquipments.Add(new RoomEquipment
                    {
                        RoomId = roomId,
                        EquipmentId = eqId
                    });
                }

                db.SaveChanges();
            }
            return true;
        }


        public Room Save(int id, Room obj,List<int> equipmentIds)
        {
            //throw new NotImplementedException();
            Room savedRoom;
            if (id == 0) // Create
            {
                savedRoom = CreateNew(obj);
            }
            else
            {
                savedRoom = Update(id,obj);
            }

            AssignEquipmentsToRoom(savedRoom.Id, equipmentIds);
            return savedRoom;
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

        public List<Room> GetFiltered(EquipmentListDto equipmentsList, DateTime? from, DateTime? to, decimal? minPrice, decimal? maxPrice, int? minNumberOfBeds, int? maxNumberOfBeds)
        {
            var equipments = equipmentsList.Equipments;

            bool useDates = from != null || to != null;

            var dateFrom = from == null ? new DateTime(01, 01, 01) : from;
            var dateTo = to == null ? new DateTime(2222, 01, 01) : to;

            using (var db = new HotelContext())
            {

                var rooms = db.Rooms
                    .Include(e => e.RoomEquipments).ThenInclude(e => e.Equipment)
                    .Include(e => e.RoomReservations).ThenInclude(e => e.Reservation)
                    .ToList();

                var tmp = rooms.Where(x =>
                    (minPrice is null || minPrice <= x.CostPerNight)
                 && (maxPrice is null || maxPrice >= x.CostPerNight)
                 && (minNumberOfBeds is null || minNumberOfBeds <= x.NumberOfBeds)
                 && (maxNumberOfBeds is null || maxNumberOfBeds >= x.NumberOfBeds)
                     ).ToList();

                tmp = tmp.Where(x =>
                      (equipments is null || equipments.Count == 0 || 
                      equipments.All(e=>x.RoomEquipments.Any(re=>re.Equipment.Name == e.Name)))
                     ).ToList();


                if (!useDates)
                    return tmp;

                tmp = tmp.Where(x =>
                  useDates 
                  && 
                  (x.RoomReservations is null || x.RoomReservations.Count == 0 
                  || !x.RoomReservations.Any(r => 
                      r.Reservation.ReservationState != ReservationStateEnum.Canceled
                      && !IsNotReserved(dateFrom.Value, dateTo.Value, r.DateFrom, r.DateTo)))
                      ).ToList();


                return tmp;
            }
        }

        /// <summary>
        /// Reservation 4-10; want to show only rooms 0-4, 10-100000
        /// </summary>
        /// <param name="ff"></param>
        /// <param name="ft"></param>
        /// <param name="rf"></param>
        /// <param name="rt"></param>
        /// <returns></returns>
        private bool IsNotReserved(DateTime ff, DateTime ft, DateTime rf, DateTime rt)
        {

            if ((ff <= rf && ft <= rf) || (ff >= rf && ff >= rt))
                return true;

            return false;

        }

        public Room Update(int id, Room room)
        {
            using (var db = new HotelContext())
            {
                var dbRoom = db.Rooms.Where(x => x.Id == id).FirstOrDefault();
                if (dbRoom == null)
                    throw new Exception($"Room with id:'{room.Id}' was not found in database.");

                if (dbRoom.Equals(room))
                    return room;

                dbRoom.NumberOfBeds = room.NumberOfBeds;
                dbRoom.NumberOfSideBeds = room.NumberOfSideBeds;
                dbRoom.RoomNumber = room.RoomNumber;
                dbRoom.CostPerNight = room.CostPerNight;
                dbRoom.Floor = room.Floor;
                dbRoom.IsCleaned = room.IsCleaned;
                dbRoom.Picture = room.Picture;
                dbRoom.RoomSize = room.RoomSize;

                db.SaveChanges();

                return room;
            }
        }

        public List<Equipment> GetEquipments()
        {
            using (var db = new HotelContext())
            {
                var equipments = db.Equipments.ToList();
                return equipments;
            }
        }
        public List<Equipment> GetEquipmentsOfRoom(int roomId)
        {
            using (var db = new HotelContext())
            {
                var equipments = db.RoomEquipments.Where(x=>x.RoomId==roomId).Select(s=>s.Equipment).ToList();
                return equipments;
            }
        }
        public bool MarkAsCleaned(int roomId)
        {
            using (var db = new HotelContext())
            {
                var room = db.Rooms.Where(x => x.Id == roomId).FirstOrDefault();
                if (room == null)
                    throw new Exception($"Room with id:'{roomId}' was not found in database.");
                room.IsCleaned = true;

                db.SaveChanges();
                return true;
            }
        }
    }
}
