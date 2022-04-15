using Hotel_PIS.DAL;
using Hotel_PIS.IServices.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Hotel_PIS.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_PIS.Services
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public bool Delete(int id)
        {
            using (var db = new HotelContext())
            {
                try
                {
                    var client = db.Employees.FirstOrDefault(x => x.Id == id);
                    db.Entry(client).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        public Employee Get(int id)
        {
            using (var db = new HotelContext())
            {
                var employee = db.Employees.Where(x => x.Id == id).FirstOrDefault();
                if (employee == null)
                    throw new Exception($"Client with id:'{id}' was not found in database.");

                return employee;
            }

        }

        public List<Employee> GetAll()
        {
            using (var db = new HotelContext())
            {
                return db.Employees.ToList();
            }
        }

        public Employee Save(int id, Employee obj)
        {
            Employee savedEmployee;

            if (id == 0) // Create
            {
                savedEmployee = CreateNewEmployee(obj);
            }
            else
            {
                savedEmployee = UpdateEmployee(obj);
            }

            return savedEmployee;
        }

        private Employee CreateNewEmployee(Employee employee)
        {
            using (var db = new HotelContext())
            {
                db.Employees.Add(employee);
                db.SaveChanges();

                return employee;
            }
        }

        private Employee UpdateEmployee(Employee employee)
        {
            using (var db = new HotelContext())
            {
                var dbEmployee = db.Employees.Where(x => x.Id == employee.Id).FirstOrDefault();
                if (dbEmployee == null)
                    throw new Exception($"Employee with id:'{employee.Id}' was not found in database.");

                if (dbEmployee.Equals(employee))
                    return employee;

                db.SaveChanges();
                return employee;
            }
        }
    }
}
