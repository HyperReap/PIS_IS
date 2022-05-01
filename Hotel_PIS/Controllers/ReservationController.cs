using Hotel_PIS.DAL;
using Hotel_PIS.DAL.Dto;
using Hotel_PIS.IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_PIS.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReservationController : Controller, IReservationRepository
    {
        private readonly IReservationRepository reservationRepository;
        private readonly ILogger logger;

        public ReservationController(ILogger<ReservationController> logger, IReservationRepository reservationRepository)
        {
            this.reservationRepository = reservationRepository;
            this.logger = logger;
        }

        [HttpGet]
        public void CancelReservation(int id)
        {
            reservationRepository.CancelReservation(id);
        }

        [HttpGet]
        public void CheckIn(int id)
        {
            reservationRepository.CheckIn(id);
        }

        [HttpGet]
        public void CheckOut(int id)
        {
            reservationRepository.CheckOut(id);
        }

        [HttpGet]
        public Reservation Get(int id)
        {
            return reservationRepository.Get(id);
        }

        [HttpGet]
        public List<Reservation> GetAll()
        {
            return reservationRepository.GetAll();
        }

        [HttpGet]
        public List<FromToDateDto> GetBookedDatesOfRooms([FromQuery]List<int> roomIds, DateTime dateNow)
        {
            return reservationRepository.GetBookedDatesOfRooms(roomIds, dateNow);
        }

        [HttpGet]
        public List<ReservationDto> GetInProgressReservations()
        {
            return reservationRepository.GetInProgressReservations();
        }

        [HttpGet]
        public List<ReservationDto> GetReservationsByEmail([Required]string email)
        {
            return reservationRepository.GetReservationsByEmail(email);
        }

        [HttpGet]
        public void Pay(int id)
        {
            reservationRepository.Pay(id);
        }

        [HttpGet]
        public void PayArrear(int id)
        {
            reservationRepository.PayArrear(id);
        }

        [HttpGet]
        public void PayDeposit(int id, decimal amount)
        {
            reservationRepository.PayDeposit(id, amount);
        }

        [HttpPost]
        public Reservation Save([FromBody]ReservationDto obj)
        {
            return reservationRepository.Save(obj);
        }
    }
}
