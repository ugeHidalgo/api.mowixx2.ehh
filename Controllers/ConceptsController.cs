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
    public class ConceptsController : ControllerBase
    {
        private readonly ConceptsService _service;

        public ConceptsController(ConceptsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Concept>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{company}", Name = "GetConceptsForCompany")]
        public async Task<ActionResult<List<Concept>>> GetAllForCompany(string company)
        {
            return Ok(await _service.GetAllForCompanyAsync(company));
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Concept>> GetById(string id)
        {
            Concept concept = await _service.GetByIdAsync(id);

            if (concept == null)
            {
                return NotFound(string.Format("No concept with Id: \'{0}\' exist", id));

            }

            return concept;
        }

        [HttpPost]
        public async Task<List<Concept>> CreateConcepts([FromBody] List<Concept> concepts)
        {
            List<Concept> newConcepts = await _service.CreateConceptsAsync(concepts);

            return newConcepts;
        }
    }
}