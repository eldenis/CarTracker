using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace restapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValuesController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]JObject userInfo)
        {
            var username = userInfo["username"].ToString();
            var password = userInfo["password"].ToString();

            //We would validate against the DB
            if (username != "user" || password != "123")
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            // return basic user info (without password) and token to store on the front-end
            return Ok(CreateUserToken(1));
        }

        private string CreateUserToken(int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Startup.JwtKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }



        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //Dummy method, only for testing REST API
            return new[] { "value1", "value2" };
        }

        // GET api/values/5
        /// <summary>
        /// Method that gets the current location of a vehicule starting with a starting location
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            var location = LocationHelper.GetCurrentLocation(id);

            dynamic jsonObject = new JObject();
            jsonObject.Latitude = location.latitude;
            jsonObject.Longitude = location.longitude;

            return jsonObject.ToString();
        }

    }
}
