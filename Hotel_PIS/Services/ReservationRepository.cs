using Hotel_PIS.DAL;
using Hotel_PIS.IServices.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Hotel_PIS.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel_PIS.DAL.Dto;

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

        public Reservation Save(ReservationDto obj)
        {
            Reservation savedReservation;
            int? clientId = null;

                //if (clientId == 0)
                    clientId = CreateNewClient(obj.FirstName, obj.SecondName, obj.Email, obj.PhoneNumber);
                savedReservation = CreateReservation(obj.RoomId,obj.NumberOfPeople, obj.DateTo, obj.DateFrom, clientId);
            

            return savedReservation;
        }

        private int? CreateNewClient(string firstName, string secondName, string email, string phoneNumber)
        {
            using (var db = new HotelContext())
            {
                var client = db.Clients.FirstOrDefault(x=>Equals(x.Email, email));
                if (client == null)
                {

                    client = new Client
                    {
                        FirstName = firstName,
                        Email = email,
                        PhoneNumber = phoneNumber,
                        SecondName = secondName,
                    };
                    db.Clients.Add(client);
                    db.SaveChanges();

                    //client = db.Clients.FirstOrDefault(x => Equals(x.Email, client.Email));
                }

                return client?.Id;
            }
        }

        /// <summary>
        /// in reservation send only number of people and clientId and client as null
        /// </summary>
        /// <param name="reservation"></param>
        /// <param name="roomId"></param>
        /// <param name="dateTo"></param>
        /// <param name="dateFrom"></param>
        /// <returns></returns>
        private Reservation CreateReservation(int roomId, int numberOfPeople, DateTime dateTo, DateTime dateFrom, int? clientId)
        {
            using (var db = new HotelContext())
            {
                var room = db.Rooms.Where(x => x.Id == roomId).First();
                int numberOfDays = (int)(dateTo.Date - dateFrom.Date).TotalDays;


                Reservation reservation = new Reservation
                {
                    Cost = numberOfDays * room.CostPerNight,
                    ReservationState = ReservationStateEnum.Reserved,
                    NumberOfPeople = numberOfPeople,
                };
                if (clientId != null)
                    reservation.ClientId = clientId.Value;


                reservation.RoomReservations.Add(new RoomReservation { RoomId = roomId, DateFrom = dateFrom, DateTo = dateTo});

                db.Reservations.Add(reservation);
                db.SaveChanges();

                return reservation;
            }
        }

        /// <summary>
        /// If room is changed, new Reservation is created in method createNewReservation
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private Reservation UpdateReservation(Reservation reservation, int roomId, DateTime dateTo, DateTime dateFrom)
        {
            //MIGHT BE NEEDED SOME TIME //TODO DELETE BEFORE RELEASE
            using (var db = new HotelContext())
            {
                var dbReservation = db.Reservations
                    .Include(e=>e.RoomReservations)
                    .Where(x => x.Id == reservation.Id).FirstOrDefault();
                if (dbReservation == null)
                    throw new Exception($"Reservation with id:'{reservation.Id}' was not found in database.");

                if (dbReservation.Equals(reservation))
                    return reservation;

                dbReservation.ReservationState = reservation.ReservationState;
                dbReservation.RoomReservations.First(x=>x.RoomId == roomId).DateTo = dateTo;
                dbReservation.RoomReservations.First(x => x.RoomId == roomId).DateFrom = dateFrom;
                dbReservation.NumberOfPeople = reservation.NumberOfPeople;
                dbReservation.Cost = reservation.Cost;
                dbReservation.Payed = reservation.Payed;

                db.SaveChanges();

                return reservation;
            }
        }

        public List<FromToDateDto> GetBookedDatesOfRooms(List<int> roomIds, DateTime dateNow)
        {
            using (var db = new HotelContext())
            {
                    var tmp = db.RoomReservations.Where(x =>
                        roomIds.Contains(x.RoomId)
                        && x.DateFrom >= dateNow)
                            .Select(s => new FromToDateDto
                            {
                                DateFrom = s.DateFrom,
                                DateTo= s.DateTo,
                            }).ToList();
                return tmp;
            }
        }

        public List<Reservation> GetReservationsByEmail(string email)
        {
            using (var db = new HotelContext())
            {
                    var tmp =db.RoomReservations.Where(x =>x.Reservation.Client.Email == email)
                    .Select(s=>new Reservation
                    {
                        Cost = s.Reservation.Cost,
                        Payed = s.Reservation.Payed,
                        NumberOfPeople = s.Reservation.NumberOfPeople,
                        ReservationState = s.Reservation.ReservationState
                    }).ToList();
                return tmp;

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
