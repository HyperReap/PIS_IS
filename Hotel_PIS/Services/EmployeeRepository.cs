using Hotel_PIS.DAL;
using Hotel_PIS.IServices.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Hotel_PIS.IServices;
using Microsoft.AspNetCore.Mvc;
using bc = BCrypt.Net.BCrypt;
using Hotel_PIS.DAL.Dto;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Hotel_PIS.Services
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IConfiguration _configuration;
        public EmployeeRepository()
        {

        }

        public EmployeeRepository(IConfiguration config)
        {
            this._configuration = config;
        }


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
                savedEmployee = UpdateEmployee(id,obj);
            }

            return savedEmployee;
        }

        private Employee CreateNewEmployee(Employee employee)
        {
            using (var db = new HotelContext())
            {
                db.Employees.Add(employee);
                employee.Password = GenerateNewPassword(employee.Password);
                db.SaveChanges();

                return employee;
            }
        }

        private Employee UpdateEmployee(int id, Employee employee)
        {
            using (var db = new HotelContext())
            {
                var dbEmployee = db.Employees.Where(x => x.Id == id).FirstOrDefault();
                if (dbEmployee == null)
                    throw new Exception($"Employee with id:'{employee.Id}' was not found in database.");

                if (dbEmployee.Equals(employee))
                    return employee;

                dbEmployee.Email = employee.Email;
                dbEmployee.ContractDueDae = employee.ContractDueDae;
                dbEmployee.FirstName = employee.FirstName;
                dbEmployee.SecondName = employee.SecondName;
                dbEmployee.RoleId = employee.RoleId;


                db.SaveChanges();
                return employee;
            }
        }



        private string GenerateNewPassword(string pwd)
        {
            string passwordHash = bc.HashPassword(pwd);
            return passwordHash;
        }

        public UserDto Login(UserDto user)
        {
            using (var db = new HotelContext())
            {
                var employee = db.Employees
                    .Include(e=>e.Role).Where(x => x.Email == user.Email).FirstOrDefault();
                if (employee == null)
                    throw new Exception($"Employee with email:'{employee.Email}' was not found in database.");

                var res = bc.Verify(user.Password, employee.Password);
                if (!res)
                    throw new Exception("Wrong Password, try again, biatch~! ");


                user.FirstName = employee.FirstName;
                user.SecondName = employee.SecondName;
                user.RoleId = employee.RoleId.Value;
                user.Role = employee.Role.NameOfRole;
                
            }

            string token = this.CreateToken(user);

            user.JWT = token;

            return user;
        }

        private string CreateToken(UserDto user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                 new Claim(ClaimTypes.Role, user.Role)

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
