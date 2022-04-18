using Hotel_PIS.DAL;
using Hotel_PIS.IServices.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Hotel_PIS.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_PIS.Services
{
    public class ReservationRepository : IReservationRepository
    {
        public bool Delete(int id)
        {
            using (var db = new HotelContext())
            {
                try
                {
                    var reservation = db.Reservations.FirstOrDefault(x => x.Id == id);
                    db.Entry(reservation).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        public Reservation Get(int id)
        {
            using (var db = new HotelContext())
            {
                var reservation = db.Reservations.Where(x => x.Id == id).FirstOrDefault();
                if (reservation == null)
                    throw new Exception($"Reservation with id:'{id}' was not found in database.");

                return reservation;
            }

        }

        public List<Reservation> GetAll()
        {
            using (var db = new HotelContext())
            {
                return db.Reservations.ToList();
            }
        }

        public Reservation Save(Reservation obj, int roomId)
        {
            Reservation savedReservation;
            if (obj.Id == 0)
            {
                savedReservation = CreateReservation(obj, roomId);
            }
            else
            {
                savedReservation = UpdateReservation(obj);
            }

            return savedReservation;
        }

        private Reservation CreateReservation(Reservation reservation, int roomId)
        {
            using (var db = new HotelContext())
            {
                reservation.RoomReservations.Add(new RoomReservation { RoomId = roomId});
                db.Reservations.Add(reservation);
                db.SaveChanges();

                return reservation;
            }
        }

        private Reservation UpdateReservation(Reservation reservation)
        {
            using (var db = new HotelContext())
            {
                var dbReservation = db.Reservations.Where(x => x.Id == reservation.Id).FirstOrDefault();
                if (dbReservation == null)
                    throw new Exception($"Reservation with id:'{reservation.Id}' was not found in database.");

                if (dbReservation.Equals(reservation))
                    return reservation;

                db.SaveChanges();
                return reservation;
            }
        }

        public void CancelReservation(int id)
        {
            changeState(id, ReservationStateEnum.Canceled);
        }

        public void Pay(int id)
        {
            using (var db = new HotelContext())
            {
                var dbReservation = Get(id);
                dbReservation.Payed = dbReservation.Cost;
                db.Reservations.Update(dbReservation);
                db.SaveChanges();
            }
        }

        public void PayDeposit(int id, decimal amount)
        {
            using (var db = new HotelContext())
            {
                var dbReservation = Get(id);
                dbReservation.Payed = amount;
                db.Reservations.Update(dbReservation);
                db.SaveChanges();
            }
        }

        public void PayArrear(int id)
        {
            Pay(id);
        }

        public void CheckIn(int id)
        {
            changeState(id, ReservationStateEnum.Check_in);
        }

        public void CheckOut(int id)
        {
            changeState(id, ReservationStateEnum.Check_out);
        }

        private void changeState(int id, ReservationStateEnum reservationState)
        {
            using (var db = new HotelContext())
            {
                var dbReservation = Get(id);
                dbReservation.ReservationState = reservationState;
                db.Reservations.Update(dbReservation);
                db.SaveChanges();
            }
        }
    }
}
