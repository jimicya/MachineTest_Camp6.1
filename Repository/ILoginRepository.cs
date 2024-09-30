using AMSWebApi.Models;

namespace AMSWebApi.Repository
{
    public interface ILoginRepository
    {
        //get user details by using username and password
        public Task<Login> ValidateUser(string username, string password);
        public Task<UserReg> RegisterUserAsync(UserReg user);
        Task<IEnumerable<UserReg>> GetAllUsersAsync();
    }

}
