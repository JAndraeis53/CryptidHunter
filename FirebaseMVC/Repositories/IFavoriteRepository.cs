using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptidHunter.Models;

namespace CryptidHunter.Repositories
{
    public interface IFavoriteRepository
    {
        void AddFavorite(Favorite favorite);
        void DeleteFavorite(int id);
        List<Favorite> GetAllFavorites();
    }
}
