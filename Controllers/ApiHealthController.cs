using API.Mowizz2.EHH.Configs;
using API.Mowizz2.EHH.Facades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace API.Mowizz2.EHH.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiHealthController : ControllerBase
    {
        private readonly BankAccountsService _bankAccountsService;
        private readonly IDataBaseSettings _dbSettings;

        public ApiHealthController(
            IDataBaseSettings settings, 
            BankAccountsService bankAccountsService
            )
        {
            _dbSettings = settings;
            _bankAccountsService = bankAccountsService;
        }

        [HttpGet]
        public HealthData Get()
        {
            return new HealthData
            {
                CheckDate = DateTime.Now,                
                DataBaseName = _dbSettings.DatabaseName,
                ConnectionString = _dbSettings.ConnectionString,
                BankAccountsHealthStatus = _bankAccountsService.Check()            
            };
        }
    }
}
