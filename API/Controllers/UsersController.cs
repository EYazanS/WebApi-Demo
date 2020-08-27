using Business.Managers;
using Business.Resources;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserManager _manager;

        public UsersController(IUserManager manager)
        {
            _manager = manager;
        }

        [HttpPost("authenticate"), AllowAnonymous]
        public IActionResult Authenticate(AuthenticateResource model)
        {
            var response = _manager.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }
    }
}
