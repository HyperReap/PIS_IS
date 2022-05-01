using Hotel_PIS.DAL;
using Hotel_PIS.DAL.Dto;

namespace Hotel_PIS.IServices
{
    public interface IRoomRepository
    {
        bool Delete(int id);
        Room Get(int id);
        List<Room> GetAll();
        List<Room> GetAllCleaned();
        List<Room> GetAllUncleaned();
        List<Equipment> GetEquipments();
        List<Equipment> GetEquipmentsOfRoom(int roomId);
        List<Room> GetFiltered(EquipmentListDto equipmentsList, DateTime? from, DateTime? to, decimal? minPrice, decimal? maxPrice, int? minNumberOfBeds, int? maxNumberOfBeds);
        bool MarkAsCleaned(int roomId);
        Room Save(int id, Room obj, List<int> equipmentIds);
    }
}