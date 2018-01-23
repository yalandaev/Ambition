using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using Ambition.WebApi.Contract;
using Ambition.WebApi.Models;
using Ambition.WebApi.Services;

namespace Ambition.WebApi.Controllers
{
    [Authorize]
    public class AccountController : ApiController
    {
        private UserService _userService;

        public AccountController()
        {
            _userService = new UserService();
        }
        [HttpGet]
        [Route("api/account/users/{id}")]
        public IHttpActionResult GetUser(int id)
        {
            var user = _userService.GetUser(id);

            //var currentUser = HttpContext.Current.User;
            //var identity = (ClaimsIdentity)User.Identity;
            //IEnumerable<Claim> claims = identity.Claims;

            return Ok(user);
        }

        [HttpPost]
        [Route("api/account/register")]
        public IHttpActionResult Register(RegisterUserResourceModel registerUser)
        {
            _userService.Register(registerUser);
            return Ok();
        }
    }
}