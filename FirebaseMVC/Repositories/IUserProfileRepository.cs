using CryptidHunter.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace CryptidHunter.Repositories
{
    public interface IUserProfileRepository
    {
        void Add(UserProfile userProfile);
        UserProfile GetByFirebaseUserId(string firebaseUserId);
        UserProfile GetUserById(int id);
        List<UserProfile> GetAllUsers();
    }
}