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
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FailureController : ControllerBase, IFailureRepository
    {
        private readonly IFailureRepository failureRepository;
        private readonly ILogger logger;

        public FailureController(ILogger<FailureController> logger, FailureController failureRepository)
        {
            this.failureRepository = failureRepository;
            this.logger = logger;
        }

        [HttpDelete]
        public bool Delete(int id)
        {
            return failureRepository.Delete(id);
        }

        [HttpPost]
        public Failure Save(int id, Failure obj)
        {
            return failureRepository.Save(id, obj);
        }
        
        [HttpPost]
        public bool Solve(int id)
        {
            return failureRepository.Solve(id);
        }
    }
}