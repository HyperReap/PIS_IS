using Hotel_PIS.DAL;
using Hotel_PIS.IServices.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Hotel_PIS.IServices;
using Microsoft.AspNetCore.Mvc;

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

        public Failure Save(int id, Failure obj)
        {
            Failure savedFailure;

            if (id == 0) // Create
            {
                savedFailure = CreateNewFailure(obj);
            }
            else
            {
                savedFailure = UpdateFailure(obj);
            }

            return savedFailure;
        }
        private Failure CreateNewFailure(Failure failure)
        {
            using (var db = new HotelContext())
            {
                db.Failures.Add(failure);
                db.SaveChanges();

                return failure;
            }
        }

        private Failure UpdateFailure(Failure failure)
        {
            using (var db = new HotelContext())
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
        }
        public bool Solve(int id)
        {
            using (var db = new HotelContext())
            {
                var dbFailure = db.Failures.Where(x => x.Id == id).FirstOrDefault();
                if (dbFailure == null)
                    throw new Exception($"Failure with id:'{id}' was not found in database.");
                
                dbFailure.IsSolved = true;
                db.Failures.Update(dbFailure);

                db.SaveChanges();
                
                return true;
            }
        }
        
        public Failure Get(int id)
        {
            using (var db = new HotelContext())
            {
                var failure = db.Failures.Where(x => x.Id == id).FirstOrDefault();
                if (failure == null)
                    throw new Exception($"Failure with id:'{id}' was not found in database.");

                return failure;
            }

        }

        public List<Failure> GetAll()
        {
            using (var db = new HotelContext())
            {
                return db.Failures.ToList();
            }
        }
    }
}
