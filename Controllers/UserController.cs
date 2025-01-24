using mapa_back.Data;
using mapa_back.Enums;
using mapa_back.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace mapa_back.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly IUsersService usersService;
        private SHA256 sha256;
        private JwtHelper jwt;
        public UserController(IUsersService usersService)
        {
            sha256 = SHA256.Create();
            jwt = new JwtHelper();
            this.usersService = usersService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string login, string password)
        {
            User user = await usersService.GetUserByEmailAsync(login);
            if(user != null)
            {
                if (user.Password == Convert.ToHexString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password))))
                {
                    string token = jwt.GenerateJwtToken(user.Id.ToString(), user.IdRole.ToString());

                    return Ok(token);
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(string login, string password, string firstname, string lastname)
        {
            User user = await usersService.GetUserByEmailAsync(login);
            string passHash = Convert.ToHexString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
            if (user == null)
            {
                user = new User()
                {
                    Email = login,
                    FirstName = firstname,
                    LastName = lastname,
                    Password = passHash,
                    IdRole = (int)Role.User
                };
                await usersService.PostSingleUser(user);
                return Ok();
            }
            else
            {
                return Conflict();
            }
        }
    }
}
