using mapa_back.Data;

namespace mapa_back.Services
{
    public interface IUsersService
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(int id);

        Task<bool> PostSingleUser(User user);
    }
}
