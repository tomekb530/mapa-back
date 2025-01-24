using mapa_back.Data;
using mapa_back.Enums;
using mapa_back.Services;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        public async Task<IActionResult> Login(string email, string password)
        {
            User user = await usersService.GetUserByEmailAsync(email);
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
        public async Task<IActionResult> Register(string email, string password, string firstname, string lastname)
        {
            User user = await usersService.GetUserByEmailAsync(email);
            string passHash = Convert.ToHexString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
            if (user == null)
            {
                user = new User()
                {
                    Email = email,
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
