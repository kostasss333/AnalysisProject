using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entitites;

namespace DAL
{
    public interface IUser
    {
        Task<string> CreateUser(User us, string password);
        Task<bool> SignIn(string username, string password);
        Task SignOut();
        Task<User> GetLoggedUser(string username);
        Task<Writer> GetWriterUser(string id);
        Task CreateSystemUser(User us);
        Task CreateWriter(Writer writer);
        Task<List<User>> GetAllUsers();
        bool IsANewName(User us);
        User ReturnUserById(string id);
    }
}