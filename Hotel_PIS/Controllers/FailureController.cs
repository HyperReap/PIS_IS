using Hotel_PIS.DAL;
using Hotel_PIS.DAL.Dto;
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

        public FailureController(ILogger<FailureController> logger, IFailureRepository failureRepository)
        {
            this.failureRepository = failureRepository;
            this.logger = logger;
        }

        [HttpDelete]
        public bool Delete(int failureId)
        {
            return failureRepository.Delete(failureId);
        }

        [HttpPost]
        public Failure Save(Failure obj)
        {
            return failureRepository.Save(obj);
        }
        
        [HttpGet]
        public bool Solve(int failureId)
        {
            return failureRepository.Solve(failureId);
        }

        [HttpGet]
        public FailureDto Get(int failureId)
        {
            return failureRepository.Get(failureId);
        }
        
        [HttpGet]
        public List<FailureDto> GetAll()
        {
            return failureRepository.GetAll();
        }
    }
}