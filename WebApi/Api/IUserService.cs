namespace WebApi.Api
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(string username, string password);
    }

}
