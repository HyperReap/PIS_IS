using Hotel_PIS.DAL;

namespace Hotel_PIS.IServices
{
    public interface IRoomRepository
    {
        bool Delete(int id);
        Room Get(int id);
        List<Room> GetAll();
        Room Save(int id, Room obj);
    }
}