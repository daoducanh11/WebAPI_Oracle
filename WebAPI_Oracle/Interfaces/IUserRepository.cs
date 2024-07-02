using WebAPI_Oracle.Entities;
using WebAPI_Oracle.Models;

namespace WebAPI_Oracle.Interfaces
{
    public interface IUserRepository
    {
        Task<int> CheckLogin(LoginModel model);

        Task<int> UpdateRefreshToken(User user);

        Task<User> GetByID(int id);
    }
}
