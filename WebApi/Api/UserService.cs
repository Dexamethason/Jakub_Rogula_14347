using WebApi.Api;

namespace WebApi.Service
{
    public class UserService : IUserService
    {
        private readonly List<User> _users = new List<User>
        {
            new User { Username = "admin", Password = "password", Role = "Admin" },
            new User { Username = "user", Password = "password", Role = "User" }
        };

        public Task<User> AuthenticateAsync(string username, string password)
        {
            var user = _users.Find(u => u.Username == username && u.Password == password);
            return Task.FromResult(user);
        }
    }

}