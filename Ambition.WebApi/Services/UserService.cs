using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ambition.WebApi.Models;

namespace Ambition.WebApi.Services
{
    public class UserService
    {
        public User ValidateUser(string email, string password)
        {
            // Here you can write the code to validate
            // User from database and return accordingly
            // To test we use dummy list here
            var userList = GetUserList();
            var user = userList.FirstOrDefault(x => x.Email == email && x.Password == password);
            return user;
        }

        public List<User> GetUserList()
        {
            return new List<User>()
            {
                new User() { Name = "admin", Email = "admin@app.com", Id = 1, Password = "secret"}
            };
        }
    }
}