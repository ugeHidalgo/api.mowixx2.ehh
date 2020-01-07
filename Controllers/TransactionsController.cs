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

        [HttpGet("{company}/{id}", Name = "GetTransactionById")]
        public async Task<ActionResult<Transaction>> GetById(string company, string id)
        {
            Transaction transaction = await _service.GetById(company, id);

            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }

        [HttpPost("import/")]
        public async Task<List<Transaction>> ImportTransactions([FromBody] List<ImportTransaction> transactionsToImport)
        {
            List<Transaction> newTransactions = await _service.ImportTransactions(transactionsToImport);

            return newTransactions;
        }

        [HttpPost]
        public async Task<ActionResult<Transaction>> CreateTransaction([FromBody] ImportTransaction transaction)
        {
            await _service.CreateTransaction(transaction);
            var result = Created("", transaction);
            return result;
        }
    }
}