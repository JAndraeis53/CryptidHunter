﻿using System;
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
                        SELECT p.Id, p.title, p.body, up.UserProfileId
                        FROM  Post p
                            LEFT JOIN UserProfile up ON p.UserProfile = up.Id
                        ORDER BY UserName ASC";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Post> posts = new List<Post>();

                    while (reader.Read())
                    {
                        Post post = new Post
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            title = reader.GetString(reader.GetOrdinal("title")),
                            body = reader.GetString(reader.GetOrdinal("body")),
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
        public Post GetPostById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT Id, title, body
                                    FROM Post
                                    WHERE Id = @Id";

                    cmd.Parameters.AddWithValue("@id", id);

                    Post post = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        post = new Post
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            title = reader.GetString(reader.GetOrdinal("title")),
                            body = reader.GetString(reader.GetOrdinal("body")),
                        };
                    }
                    reader.Close();

                    return post;
                }
            }
        }
        public void Add(Post post)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        INSERT INTO
                                        Post (title, body, UserProfileId) 
                                        OUTPUT INSERTED.ID
                                        VALUES(@title, @body, @UserProfileId)";

                    cmd.Parameters.AddWithValue("@title", post.title);
                    cmd.Parameters.AddWithValue("@body", post.body);
                    cmd.Parameters.AddWithValue("@UserProfileId", post.UserProfileId);

                    post.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
    }
}
