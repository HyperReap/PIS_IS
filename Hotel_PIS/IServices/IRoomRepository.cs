﻿using Hotel_PIS.DAL;

namespace Hotel_PIS.IServices
{
    public interface IRoomRepository
    {
        bool Delete(int id);
        Room Get(int id);
        List<Room> GetAll();
        List<Equipment> GetEquipments();
        List<Equipment> GetEquipmentsOfRoom(int roomId);
        List<Room> GetFiltered(EquipmentsList equipmentsList, DateTime? from, DateTime? to, decimal? minPrice, decimal? maxPrice, int? minNumberOfBeds, int? maxNumberOfBeds);
        Room Save(int id, Room obj);
    }
}