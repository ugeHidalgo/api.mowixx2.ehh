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

        [HttpPost]
        public async Task<List<Transaction>> ImportTransactions([FromBody] List<Transaction> transactions)
        {
            List<Transaction> newTransactions = await _service.ImportTransactions(transactions);

            return newTransactions;
        }
    }
}