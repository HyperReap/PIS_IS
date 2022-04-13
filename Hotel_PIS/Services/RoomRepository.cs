using Hotel_PIS.DAL;
using Hotel_PIS.IServices;

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
                return db.Rooms.ToList();
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
