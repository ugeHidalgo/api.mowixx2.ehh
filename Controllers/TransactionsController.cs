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
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionsService _service;

        public TransactionsController(TransactionsService service)
        {
            _service = service;
        }

        [HttpGet("{company}", Name = "GetTransactionsForCompany")]
        public async Task<ActionResult<List<Transaction>>> GetAllForCompany(string company)
        {
            return Ok(await _service.GetAllForCompanyAsync(company));
        }

        [HttpPost("import/")]
        public async Task<List<Transaction>> ImportTransactions([FromBody] List<ImportTransaction> transactionsToImport)
        {
            List<Transaction> newTransactions = await _service.ImportTransactions(transactionsToImport);

            return newTransactions;
        }
    }
}