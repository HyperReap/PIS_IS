using Hotel_PIS.DAL;
using Hotel_PIS.IServices.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Hotel_PIS.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_PIS.Services
{
    public class RoleRepository : IRoleRepository
    {
        public bool AssignRole(int id, Employee employee)
        {
            using (var db = new HotelContext())
            {
                var role = Get(id);
                if (role.EmployeesWithRole.Contains(employee))
                    return false;
                role.EmployeesWithRole.Add(employee);
                db.Roles.Update(role);
                db.SaveChanges();
                return true;
            }
        }

        public bool Delete(int id)
        {
            using (var db = new HotelContext())
            {
                try
                {
                    var role = db.Roles.FirstOrDefault(x => x.Id == id);
                    db.Entry(role).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        public Role Get(int id)
        {
            using (var db = new HotelContext())
            {
                var role = db.Roles.Where(x => x.Id == id).FirstOrDefault();
                if (role == null)
                    throw new Exception($"Role with id:'{id}' was not found in database.");

                return role;
            }

        }

        public List<Role> GetAll()
        {
            using (var db = new HotelContext())
            {
                return db.Roles.ToList();
            }
        }

        public Role Save(int id, Role obj)
        {
            Role savedRole;

            if (id == 0) // Create
            {
                savedRole = CreateNewRole(obj);
            }
            else
            {
                savedRole = UpdateRole(obj);
            }

            return savedRole;
        }
        private Role CreateNewRole(Role role)
        {
            using (var db = new HotelContext())
            {
                db.Roles.Add(role);
                db.SaveChanges();

                return role;
            }
        }

        private Role UpdateRole(Role role)
        {
            using (var db = new HotelContext())
            {
                var dbClient = db.Roles.Where(x => x.Id == role.Id).FirstOrDefault();
                if (dbClient == null)
                    throw new Exception($"Role with id:'{role.Id}' was not found in database.");

                if (dbClient.Equals(role))
                    return role;

                db.SaveChanges();
                return role;
            }
        }
    }
}
