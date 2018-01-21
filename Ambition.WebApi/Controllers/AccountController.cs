using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using Ambition.WebApi.Models;

namespace Ambition.WebApi.Controllers
{
    [Authorize]
    public class AccountController : ApiController
    {
        [HttpGet]
        [Route("api/account/getuser/{id}")]
        public IHttpActionResult GetUFirstUser(int id)
        {
            // Get user from dummy list
            var userList = new List<User>()
            {
                new User() { Name = "admin", Email = "admin@app.com", Id = 1, Password = "secret"}
            };
            var user = userList.FirstOrDefault(x => x.Id == id);

            var currentUser = HttpContext.Current.User;
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            return Ok(user);
        }
    }
}