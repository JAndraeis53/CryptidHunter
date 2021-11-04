using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using CryptidHunter.Models;

namespace CryptidHunter.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly IConfiguration _config;

        public PostRepository(IConfiguration config)
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

        public List<Post> GetAllPost()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT p.Id, p.title, p.body, p.UserProfileId
                        FROM  Post p
                            LEFT JOIN UserProfile up ON p.UserProfileId = up.Id
                        ORDER BY UserName ASC";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Post> posts = new List<Post>();

                    while (reader.Read())
                    {
                        Post post = new Post
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("title")),
                            Body = reader.GetString(reader.GetOrdinal("body")),
                            UserProfile = new UserProfile
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserProfileId"))
                            }

                        };

                        posts.Add(post);
                    }

                    reader.Close();
                    return posts;
                }
            }
        }
        public Post GetPostById(int id, int upid)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT Post.Id, Post.title, Post.body, Post.UserProfileId, f.Id as FavoriteId
                                    FROM Post
                                    LEFT JOIN Favorite f ON f.PostId = Post.Id
                                    WHERE Post.Id = @Id 
                                    AND (f.UserProfileId = @UserProfileId
                                    OR f.UserProfileId IS NULL)";

                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@UserProfileId", upid);

                    Post post = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        post = new Post
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("title")),
                            Body = reader.GetString(reader.GetOrdinal("body")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                        };

                        if (!reader.IsDBNull(reader.GetOrdinal("FavoriteId")))
                        {
                            post.Favorite = new Favorite
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("FavoriteId")),
                            };
                        }
                    }
                    reader.Close();

                    return post;
                }
            }
        }
        public void AddPost(Post post)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        INSERT INTO
                                        Post (Title, Body, UserProfileId) 
                                        OUTPUT INSERTED.ID
                                        VALUES(@title, @body, @UserProfileId);
                                        ";

                    cmd.Parameters.AddWithValue("@title", post.Title);
                    cmd.Parameters.AddWithValue("@body", post.Body);
                    cmd.Parameters.AddWithValue("@UserProfileId", post.UserProfileId);

                    post.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
        public void UpdatePost(Post post)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Post
                            SET
                                Title = @title,
                                Body = @body,
                                UserProfileId = @UserProfileId
                            WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", post.Id);
                    cmd.Parameters.AddWithValue("@title", post.Title);
                    cmd.Parameters.AddWithValue("@body", post.Body);
                    cmd.Parameters.AddWithValue("@UserProfileId", post.UserProfileId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeletePost(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Post
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
