using Hotel_PIS.DAL;
using Hotel_PIS.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hotel_PIS.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase, IRoleRepository
    {
        private readonly IRoleRepository roleRepository;
        private readonly ILogger logger;

        public RoleController(ILogger<RoleController> logger, IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
            this.logger = logger;
        }

        [HttpPost]
        public bool AssignRole(int id, Employee employee)
        {
            return roleRepository.AssignRole(id, employee);
        }

        [HttpDelete]
        public bool Delete(int id)
        {
            return roleRepository.Delete(id);
        }

        [HttpGet]
        public Role Get(int id)
        {
            return roleRepository.Get(id);
        }

        [HttpGet]
        public List<Role> GetAll()
        {
            return roleRepository.GetAll();
        }

        [HttpPost]
        public Role Save(int id, Role obj)
        {
            return roleRepository.Save(id, obj);
        }
    }
}
