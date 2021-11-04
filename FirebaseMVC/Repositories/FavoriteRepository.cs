using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptidHunter.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CryptidHunter.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly IConfiguration _config;

        public FavoriteRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Favorite> GetAllFavorites()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT f.Id, f.UserProfileId, f.PostId
                        FROM  Favorite f
                            LEFT JOIN UserProfile up ON f.UserProfileId = up.Id
                            LEFT JOIN Post p ON f.PostId = p.Id
                        ORDER BY UserName ASC";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Favorite> favorites = new List<Favorite>();

                    while (reader.Read())
                    {
                        Favorite favorite = new Favorite
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                        };

                        favorites.Add(favorite);
                    }

                    reader.Close();
                    return favorites;
                }
            }
        }

        public void AddFavorite(Favorite favorite)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        INSERT INTO
                                        Favorite (PostId, UserProfileId) 
                                        OUTPUT INSERTED.ID
                                        VALUES(@PostId, @UserProfileId);
                                        ";

                    cmd.Parameters.AddWithValue("@PostId", favorite.PostId);
                    cmd.Parameters.AddWithValue("@UserProfileId", favorite.UserProfileId);

                    favorite.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void DeleteFavorite(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Favorite
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}