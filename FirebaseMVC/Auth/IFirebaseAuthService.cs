using System.Threading.Tasks;
using CryptidHunter.Auth.Models;

namespace CryptidHunter.Auth
{
    public interface IFirebaseAuthService
    {
        Task<FirebaseUser> Login(Credentials credentials);
        Task<FirebaseUser> Register(Registration registration);
    }
}