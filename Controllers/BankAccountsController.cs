using API.Mowizz2.EHH.Facades;
using API.Mowizz2.EHH.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Mowizz2.EHH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountsController : ControllerBase
    {
        private readonly BankAccountsFacade _facade;
        
        public BankAccountsController(BankAccountsFacade facade)
        {
            _facade = facade;
        }

        [HttpGet]
        public async Task<ActionResult<List<BankAccount>>> Get()
        {
            return await _facade.Get();
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<BankAccount>> Get(string id)
        {
            BankAccount bankAccount = await _facade.Get(id);

            if (bankAccount == null) 
            { 
                return NotFound(string.Format("No bank acount with Id: \'{0}\' exist", id)); 
            }  
            
            return bankAccount;
        }
    }
}