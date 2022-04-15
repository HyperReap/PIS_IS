using Hotel_PIS.DAL;
using Hotel_PIS.IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        [HttpPost]
        public void CancelReservation(int id)
        {
            reservationRepository.CancelReservation(id);
        }

        [HttpPost]
        public void CheckIn(int id)
        {
            reservationRepository.CheckIn(id);
        }

        [HttpPost]
        public void CheckOut(int id)
        {
            reservationRepository.CheckOut(id);
        }

        [HttpPost]
        public Reservation CreateReservation(Reservation obj)
        {
            return reservationRepository.CreateReservation(obj);
        }

        [HttpDelete]
        public bool Delete(int id)
        {
            return reservationRepository.Delete(id);
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

        [HttpPost]
        public void Pay(int id)
        {
            reservationRepository.Pay(id);
        }

        [HttpPost]
        public void PayArrear(int id)
        {
            reservationRepository.PayArrear(id);
        }

        [HttpPost]
        public void PayDeposit(int id, decimal amount)
        {
            reservationRepository.PayDeposit(id, amount);
        }

        [HttpPost]
        public Reservation Save(int id, Reservation obj)
        {
            return reservationRepository.Save(id, obj);
        }

        [HttpPost]
        public Reservation UpdateReservation(Reservation obj)
        {
            return reservationRepository.UpdateReservation(obj);
        }
    }
}
