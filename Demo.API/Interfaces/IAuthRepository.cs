using System.Threading.Tasks;
using Demo.API.Models;

namespace Demo.API.Interfaces
{
    public interface IAuthRepository
    {
         Task<User> Register(User user, string password);
         Task<User> Login(string userName, string password);
         Task<bool> UserExist(string userName);

    }
}