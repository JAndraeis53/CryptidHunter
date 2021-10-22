using Microsoft.Data.SqlClient;
using CryptidHunter.Models;
using System.Collections.Generic;

namespace CryptidHunter.Repositories
{
    public interface IPostRepository
    {
        List<Post> GetAllPost();
        Post GetPostById(int id);
        void AddPost(Post post);
        void UpdatePost(Post post);
        void DeletePost(int id);
    }
}