using API.Mowizz2.EHH.Facades;
using API.Mowizz2.EHH.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace API.Mowizz2.EHH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        private readonly BankAccountFacade _facade;
        
        public BankAccountController(BankAccountFacade facade)
        {
            _facade = facade;
        }

        [HttpGet]
        public ActionResult<List<BankAccount>> Get()
        {
            return _facade.Get();
        }
    }
}