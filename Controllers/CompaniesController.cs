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
    public class CompaniesController : ControllerBase
    {
        private readonly CompaniesService _service;

        public CompaniesController(CompaniesService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Company>>> Get()
        {
            return await _service.Get();
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Company>> Get(string id)
        {
            Company company = await _service.Get(id);

            if (company == null)
            {
                return NotFound(string.Format("No company with Id: \'{0}\' exist", id));

            }

            return company;
        }

        [HttpPost]
        public async Task<List<Company>> CreateCompanies([FromBody] List<Company> companies)
        {
            List<Company> newCompanies = await _service.CreateCompanies(companies);

            return newCompanies;
        }
    }
}