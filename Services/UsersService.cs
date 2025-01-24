using mapa_back.Data;
using Microsoft.EntityFrameworkCore;

namespace mapa_back.Services
{
    public class UsersService: IUsersService
    {
        private readonly DatabaseContext _dbContext;

        public UsersService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> PostSingleUser(User user){
            try
            {
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
