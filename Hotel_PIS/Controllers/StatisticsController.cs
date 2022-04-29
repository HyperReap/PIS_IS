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
    public class StatisticsController : ControllerBase, IStatisticsRepository
    {
        private readonly IStatisticsRepository statsRepository;
        private readonly ILogger logger;

        public StatisticsController(ILogger<ClientController> logger, IStatisticsRepository statisticsRepository)
        {
            this.statsRepository = statisticsRepository;
            this.logger = logger;
        }

        [HttpGet]
        public StatisticsDto GetStatistics()
        {
            return statsRepository.GetStatistics();
        }
    }
}
