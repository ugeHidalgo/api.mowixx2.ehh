using API.Mowizz2.EHH.Models;
using API.Mowizz2.EHH.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace API.Mowizz2.EHH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly UsersService _service;

        public UsersController(IOptions<JwtIssuerOptions> jwtOptions, UsersService service)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);

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
            userToken = await _service.AddTokenToAuthorizedUser(userToken, _jwtOptions);
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

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }
    }
}
