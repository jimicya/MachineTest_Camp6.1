using AMSWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AMSWebApi.Repository
{
    public class LoginRepository : ILoginRepository
    {
        // Database context
        private readonly AssetMsDbContext _context;

        // Dependency Injection - DI
        public LoginRepository(AssetMsDbContext context)
        {
            _context = context;
        }

        // Method to validate user credentials
        public async Task<Login> ValidateUser(string username, string password)
        {
            try
            {
                if (_context != null)
                {
                    return await _context.Logins.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw; 
            }
        }

        // Method to register a new user
        public async Task<UserReg> RegisterUserAsync(UserReg user)
        {
            try
            {

                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user), "Employee data is null");
                }

                if (_context == null)
                {
                    throw new InvalidOperationException("Database context is not initialized.");
                }
                await _context.UserRegs.AddAsync(user);
                await _context.SaveChangesAsync();
                var userWithLoginId = await _context.UserRegs
                    .Include(u => u.LIdNavigation).FirstOrDefaultAsync(u => u.LId == user.LId);
                return userWithLoginId;

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while registering the user.", ex);
            }

        }

        public async Task<IEnumerable<UserReg>> GetAllUsersAsync()
        {
            return await _context.UserRegs.ToListAsync();
        }
    }
}
