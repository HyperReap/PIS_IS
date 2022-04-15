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
        public Employee Save(int id, Employee obj)
        {
            return employeeRepository.Save(id, obj);
        }
    }
}
