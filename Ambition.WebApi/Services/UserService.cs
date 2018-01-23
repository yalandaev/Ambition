using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ambition.WebApi.Contract;
using Ambition.WebApi.Models;

namespace Ambition.WebApi.Services
{
    public class UserService
    {
        private static List<User> Users = new List<User>();
        private static int Counter = 1;

        static UserService()
        {
            Users.Add(new User() { FirstName = "admin", Email = "admin@app.com", Id = 1, Password = "secret" });
        }

        public User ValidateUser(string email, string password)
        {
            // Here you can write the code to validate
            // User from database and return accordingly
            // To test we use dummy list here
            var user = Users.FirstOrDefault(x => x.Email == email && x.Password == HashPassword(password));
            return user;
        }

        public void Register(RegisterUserResourceModel model)
        {
            if (Users.Any(x => x.Email == model.Email))
            {
                throw new HttpException(400, "User already exists");
            }

            Users.Add(new User()
            {
                Id = ++Counter,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = HashPassword(model.Password),
                Phone = model.Phone
            });
        }

        public GetUserResourceModel GetUser(int id)
        {
            var user = Users.SingleOrDefault(x => x.Id == id);
            return new GetUserResourceModel()
            {
                Email = user.Email,
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone
            };
        }

        private string HashPassword(string password)
        {
            return $@"#${password}$#@";
        }
    }
}