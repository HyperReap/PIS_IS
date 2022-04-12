using Hotel_PIS.DAL;
using Hotel_PIS.IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hotel_PIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase, IClientRepository
    {
        private readonly IClientRepository clientRepository;
        private readonly ILogger logger;

        public ClientController(ILogger<ClientController> logger, IClientRepository clientRepository)
        {
            this.clientRepository = clientRepository;
            this.logger = logger;
        }

        [HttpDelete]
        public bool Delete(int id)
        {
            return clientRepository.Delete(id);
        }

        [HttpGet]
        public Client Get(int id)
        {
            return clientRepository.Get(id);
        }

        [HttpGet]
        public List<Client> GetAll()
        {
            return clientRepository.GetAll();
        }

        [HttpPost]
        public Client Save(int id, Client obj)
        {
            return clientRepository.Save(id, obj);
        }
    }
}
