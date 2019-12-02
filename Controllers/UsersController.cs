using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Mowizz2.EHH.Models;
using API.Mowizz2.EHH.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Mowizz2.EHH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _service;

        public UsersController(UsersService service)
        {
            _service = service;
        }


        [HttpGet("{idOrUserName}", Name = "GetUser")]
        public async Task<ActionResult<User>> Get(string idOrUserName)
        {
            var user = await _service.Get(idOrUserName);

            if (user == null)
            {
                user = await _service.GetByUserName(idOrUserName);
                if (user == null)
                {
                    return NotFound();
                }
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post([FromBody] User user)
        {
            await _service.Post(user);
            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }

        [HttpPost("auth")]
        public async Task<ActionResult<UserToken>> GetToken([FromBody] UserToken userToken)
        {
            userToken = await _service.AddTokenToAuthorizedUser(userToken);
            if (userToken.Token == null || userToken.Token == string.Empty)
            {
                return Unauthorized(userToken);
            }            
            return Ok(userToken);
        }

        //// PUT: api/Users/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
