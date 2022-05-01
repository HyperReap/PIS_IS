using Hotel_PIS.DAL;
using Hotel_PIS.DAL.Dto;
using Hotel_PIS.IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hotel_PIS.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoomController : ControllerBase, IRoomRepository
    {
        private readonly IRoomRepository roomRepository;
        private readonly ILogger logger;

        public RoomController(ILogger<RoomController> logger, IRoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
            this.logger = logger;
        }

        [HttpDelete]
        public bool Delete(int id)
        {
            return roomRepository.Delete(id);
        }

        [HttpGet]
        public Room Get(int id)
        {
            return roomRepository.Get(id);
        }

        [HttpGet]
        public List<Room> GetAll()
        {
            return roomRepository.GetAll();
        }
        
        [HttpGet]
        public List<Room> GetAllUncleaned()
        {
            return roomRepository.GetAllUncleaned();
        }
        
        [HttpGet]
        public List<Room> GetAllCleaned()
        {
            return roomRepository.GetAllUncleaned();
        }

        [HttpGet]
        public List<Equipment> GetEquipments()
        {
            return roomRepository.GetEquipments();
        }

        [HttpGet]
        public List<Equipment> GetEquipmentsOfRoom(int roomId)
        {
            return roomRepository.GetEquipmentsOfRoom(roomId);
        }

        [HttpPost]
        public List<Room> GetFiltered([FromBody] EquipmentListDto equipmentsList, DateTime? dateFrom, DateTime? dateTo, decimal? minPrice, decimal? maxPrice, int? minNumberOfBeds, int? maxNumberOfBeds)
        {
            return roomRepository.GetFiltered(equipmentsList, dateFrom, dateTo, minPrice, maxPrice, minNumberOfBeds, maxNumberOfBeds);
        }

        [HttpPost]
        public Room Save(int id, [FromBody]Room obj, [FromQuery]List<int> equipmentIds)
        {
            return roomRepository.Save(id, obj,equipmentIds);
        }

        [HttpGet]
        public bool MarkAsCleaned(int roomId)
        {
            return roomRepository.MarkAsCleaned(roomId);
        }
    }
}
