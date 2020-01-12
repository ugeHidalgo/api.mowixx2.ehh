using API.Mowizz2.EHH.Models;
using API.Mowizz2.EHH.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Mowizz2.EHH.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AccountsService _service;

        public AccountsController(AccountsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Account>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{company}", Name = "GetAccountsForCompany")]
        public async Task<ActionResult<List<Account>>> GetAllForCompany(string company)
        {
            return Ok(await _service.GetAllForCompanyAsync(company));
        }

        [HttpGet("{company}/{status}", Name = "GetAccountsForCompanyAndStatus")]
        public async Task<ActionResult<List<Account>>> GetAllForCompanyWithStatus(string company, bool status)
        {
            return Ok(await _service.GetAllForCompanyWithStatusAsync(company, status));
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Account>> GetById(string id)
        {
            Account account = await _service.GetByIdAsync(id);

            if (account == null)
            {
                return NotFound(string.Format("No acount with Id: \'{0}\' exist", id));

            }

            return account;
        }
        
        [HttpPost]
        public async Task<List<Account>> CreateAccounts([FromBody] List<Account> accounts)
        {
            List<Account> newAccounts = await _service.CreateAccounts(accounts);

            return newAccounts;
        }
    }
}