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
    public class BankAccountsController : ControllerBase
    {
        private readonly BankAccountsService _service;

        public BankAccountsController(BankAccountsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<BankAccount>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{company}", Name = "GetBankAccountsForCompany")]
        public async Task<ActionResult<List<BankAccount>>> GetAllForCompany(string company)
        {
            return Ok(await _service.GetAllForCompanyAsync(company));
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<BankAccount>> GetById(string id)
        {
            BankAccount bankAccount = await _service.GetByIdAsync(id);

            if (bankAccount == null)
            {
                return NotFound(string.Format("No bank acount with Id: \'{0}\' exist", id));

            }

            return bankAccount;
        }
        
        [HttpPost]
        public async Task<List<BankAccount>> CreateBankAccounts([FromBody] List<BankAccount> bankAccounts)
        {
            List<BankAccount> newBankAccounts = await _service.CreateBankAccounts(bankAccounts);

            return newBankAccounts;
        }
    }
}