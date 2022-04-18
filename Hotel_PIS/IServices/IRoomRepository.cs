using Hotel_PIS.DAL;

namespace Hotel_PIS.IServices
{
    public interface IRoomRepository
    {
        bool Delete(int id);
        Room Get(int id);
        List<Room> GetAll();
        List<Room> GetFiltered(List<Equipment> equipments, DateTime? from, DateTime? to, decimal? minPrice, decimal? maxPrice, int? minNumberOfBeds, int? maxNumberOfBeds);
        Room Save(int id, Room obj);
    }
}