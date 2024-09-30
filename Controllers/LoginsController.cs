using AMSWebApi.Models;
using AMSWebApi.Repository;
using AMSWebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AMSWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginsController : ControllerBase
    {
        private IConfiguration _config;
        private readonly ILoginRepository _loginRepository;

        public LoginsController(ILoginRepository loginRepository, IConfiguration config)
        {
            _config = config;
            _loginRepository = loginRepository;
        }
        [HttpGet("{username}/{password}")]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Variables for tracking unauthorized
            IActionResult response = Unauthorized(); //401
            Login dbUser = null;

            // 1 - Authenticate the user by passing username and password
            dbUser = await _loginRepository.ValidateUser(username, password);

            // 2 - Generate JWT Token
            if (dbUser != null)
            {
                // Custom Method for generating token
                var tokenString = GenerateJWTToken(dbUser);

                response = Ok(new
                {
                    userName = dbUser.Username,
                    userType = dbUser.Usertype,
                    loginId = dbUser.LId,
                    token = tokenString,
                });
            }
            return response;
        }


        private string GenerateJWTToken(Login dbUser)
        {
            // 1- Secret Security Key
            //var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            // 2- Algorithm
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //3- JWT
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"], null, expires: DateTime.Now.AddMinutes(20),
                signingCredentials: credentials);
            //4- Writing Token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _loginRepository.GetAllUsersAsync();
            return Ok(users);
        }
        // POST: api/Registration
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserReg user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newUser = await _loginRepository.RegisterUserAsync(user);

                if (newUser != null)
                {
                    return Ok(newUser);
                }
                return NotFound("User could not be created");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // GET: api/<LoginsController>
        /* [HttpGet]
         public IEnumerable<string> Get()
         {
             return new string[] { "value1", "value2" };
         }*/

        /* // GET api/<LoginsController>/5
         [HttpGet("{id}")]
         public string Get(int id)
         {
             return "value";
         }
 
        // POST api/<LoginsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<LoginsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LoginsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}