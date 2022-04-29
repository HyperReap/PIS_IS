using Hotel_PIS.DAL;
using Hotel_PIS.DAL.Dto;
using Hotel_PIS.IServices;
using Microsoft.AspNetCore.Authorization;
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
    //pro swagger zkouseni ve format: Bearer JWT
    //e.g. Bearer BJFNEWQJKHFBKASBDFHJAVFJAHBFV.AJVJXGHVUASVBDFJHAQ
    //[Authorize(Roles = "TODO, nameOfRole")]
    [Authorize(Roles = "Manager")]
    public class EmployeeController : Controller, IEmployeeRepository
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly ILogger logger;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
            this.logger = logger;
        }

        [HttpDelete]
        public bool Delete(int id)
        {
            return employeeRepository.Delete(id);
        }

        [HttpGet]
        public Employee Get(int id)
        {
            return employeeRepository.Get(id);
        }

        [HttpGet]
        public List<Employee> GetAll()
        {
            return employeeRepository.GetAll();
        }

        [HttpPost]
        public UserDto Login(UserDto user)
        {
            return employeeRepository.Login(user);

        }

        [HttpPost]
        public Employee Save(int id, Employee obj)
        {
            return employeeRepository.Save(id, obj);
        }
    }
}
