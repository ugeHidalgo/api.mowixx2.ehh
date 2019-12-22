using API.Mowizz2.EHH.Facades;
using API.Mowizz2.EHH.Models;
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
        public async Task<ActionResult<List<BankAccount>>> Get()
        {
            return await _service.Get();
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<BankAccount>> Get(string id)
        {
            BankAccount bankAccount = await _service.Get(id);

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