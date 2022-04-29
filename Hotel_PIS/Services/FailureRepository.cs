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
    public class FailureRepository : IFailureRepository
    {
        public bool Delete(int id)
        {
            using (var db = new HotelContext())
            {
                try
                {
                    var failure = db.Failures.FirstOrDefault(x => x.Id == id);
                    db.Entry(failure).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        public FailureDto Save(Failure obj)
        {
            Failure savedFailure;
            using (var db = new HotelContext())
            {
                if (obj.Id == 0) // Create
                {
                    savedFailure = CreateNewFailure(obj,db);
                }
                else
                {
                    savedFailure = UpdateFailure(obj, db);
                }
                //jenom kvuli cislu Pokoje.. nechce se mi vic premyslet
                savedFailure = db.Failures.Where(x => x.Id == obj.Id).Include(e => e.Room).First(); 
            }

            return new FailureDto
            {
                Id = savedFailure.Id,
                Description = savedFailure.Description,
                IsSolved = savedFailure.IsSolved,
                RoomId = savedFailure.RoomId,
                RoomNumber = savedFailure.Room.RoomNumber
            };
        }
        private Failure CreateNewFailure(Failure failure, HotelContext db)
        {
            db.Failures.Add(failure);
            db.SaveChanges();

            return failure;
        }

        private Failure UpdateFailure(Failure failure, HotelContext db)
        {
            var dbFailure = db.Failures.Where(x => x.Id == failure.Id).FirstOrDefault();
            if (dbFailure == null)
                throw new Exception($"Failure with id:'{failure.Id}' was not found in database.");

            if (dbFailure.Equals(failure))
                return failure;

            dbFailure.Description = failure.Description;
            db.Failures.Update(dbFailure);

            db.SaveChanges();

            return dbFailure;
        }

        public bool Solve(int failureId)
        {
            using (var db = new HotelContext())
            {
                var dbFailure = db.Failures.Where(x => x.Id == failureId).FirstOrDefault();
                if (dbFailure == null)
                    throw new Exception($"Failure with id:'{failureId}' was not found in database.");
                
                dbFailure.IsSolved = true;
                db.SaveChanges();
                
                return true;
            }
        }
        
        public FailureDto Get(int id)
        {
            using (var db = new HotelContext())
            {
                var failure = db.Failures.Where(x => x.Id == id)
                    .Include(e => e.Room).FirstOrDefault();
                if (failure == null)
                    throw new Exception($"Failure with id:'{id}' was not found in database.");

                return  new FailureDto
                {
                    Id = failure.Id,
                    Description = failure.Description,
                    IsSolved = failure.IsSolved,
                    RoomId = failure.RoomId,
                    RoomNumber = failure.Room.RoomNumber
                };
            }

        }

        public List<FailureDto> GetAll()
        {
            using (var db = new HotelContext())
            {
                    var res = db.Failures
                    .Where(x=>!x.IsSolved)
                    .Include(e=>e.Room).ToList();

                return res.Select(s=>new FailureDto
                {
                    Id = s.Id,
                    Description = s.Description,
                    IsSolved = s.IsSolved,
                    RoomId = s.RoomId,
                    RoomNumber = s.Room.RoomNumber
                }).ToList();
            }
        }
    }
}
