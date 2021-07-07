using Microsoft.Extensions.Configuration;
using music_artist_full_stack.Models;
using music_artist_full_stack.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace music_artist_full_stack.Repositories
{
    public class PostRepository : BaseRepository, IPostRepository
    {
        public PostRepository(IConfiguration config) : base(config) { }

        public List<Post> GetAllPublishedPosts()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT p.Id as PostId, p.UserId as PostUserId, p.PostTypeId as CurrentPostTypeId, p.Title, p.Description,
                    p.DateCreated as PostDate, p.Url,

                    u.Id as UserId, u.FirstName, u.LastName, u.FullName, u.Email, u.PhoneNumber, 
                    u.Age, u.UserTypeId, u.ProfilePhoto, u.DateCreated as UserDate, 
                    
                    pt.Id as PostTypeId, pt.Type

                    FROM Post p
                    
                    LEFT JOIN [User] u ON p.UserId = u.Id
                    LEFT JOIN PostType pt ON p.PostTypeId = pt.Id";

                    var reader = cmd.ExecuteReader();

                    var posts = new List<Post>();
                    while (reader.Read())
                    {
                        var postId = DbUtils.GetInt(reader, "PostId");
                        var post = posts.FirstOrDefault(p => p.Id == postId);

                        if (post == null)
                        {
                            post = new Post()
                            {
                                Id = postId,
                                UserId = DbUtils.GetInt(reader, "PostUserId"),
                                User = new User()
                                {
                                    Id = DbUtils.GetInt(reader, "PostUserId"),
                                    FirstName = DbUtils.GetString(reader, "FirstName"),
                                    LastName = DbUtils.GetString(reader, "LastName"),
                                    Email = DbUtils.GetString(reader, "Email"),
                                    PhoneNumber = DbUtils.GetString(reader, "PhoneNumber"),
                                    Age = DbUtils.GetInt(reader, "Age"),
                                    UserTypeId = DbUtils.GetInt(reader, "UserTypeId"),
                                    ProfilePhoto = DbUtils.GetString(reader, "ProfilePhoto"),
                                    DateCreated = DbUtils.GetDateTime(reader, "UserDate")
                                },
                                PostTypeId = DbUtils.GetInt(reader, "CurrentPostTypeId"),
                                PostType = new PostType()
                                {
                                    Id = DbUtils.GetInt(reader, "CurrentPostTypeId"),
                                    Type = DbUtils.GetString(reader, "Type")
                                },
                                Title = DbUtils.GetString(reader, "Title"),
                                Description = DbUtils.GetString(reader, "Description"),
                                DateCreated = DbUtils.GetDateTime(reader, "PostDate"),
                                Url = DbUtils.GetString(reader, "Url")
                            };

                            posts.Add(post);
                        }
                    }
                    reader.Close();

                    return posts;
                }
            }
        }

        public void AddPost(Post post)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Post (
                            UserId, PostTypeId, Title, Description,
                            DateCreated, Url)
                        OUTPUT INSERTED.ID
                        VALUES (
                            @UserId, @PostTypeId, @Title, @Description, @DateCreated,
                            @Url)";

                    cmd.Parameters.AddWithValue("@UserId", post.UserId);
                    cmd.Parameters.AddWithValue("@PostTypeId", post.PostTypeId);
                    cmd.Parameters.AddWithValue("@Title", post.Title);
                    cmd.Parameters.AddWithValue("@Description", post.Description);
                    cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Url", post.Url);

                    post.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void DeletePost(int postId)
        {
            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Comment
                             WHERE PostID = @id;

                            DELETE FROM Post
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", postId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void EditPost(Post post)
        {
            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Post
                            SET 
                                PostTypeId = @PostTypeId, 
                                Title = @Title, 
                                Description = @Description, 
                                DateCreated = @DateCreated,
                                Url = @Url
                            WHERE Id = @Id";

                    cmd.Parameters.AddWithValue("@PostTypeId", post.PostTypeId);
                    cmd.Parameters.AddWithValue("@Title", post.Title);
                    cmd.Parameters.AddWithValue("@Description", post.Description);
                    cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Url", post.Url);
                    cmd.Parameters.AddWithValue("@Id", post.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Post GetPostByIdWithComments(int postId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                   SELECT p.Id as PostId, p.UserId as PostUserId, p.PostTypeId as UserPostTypeId, p.Title, p.Description,
                    p.DateCreated as PostDate, p.Url,

                    u.Id as UserId, u.FirstName, u.LastName, u.FullName, u.Email, u.PhoneNumber, 
                    u.Age, u.UserTypeId, u.ProfilePhoto, u.DateCreated as UserDate, 
                    
                    pt.Id as PostTypeId, pt.Type,

                    com.Id AS CommentId, com.Content, com.UserId as CommentUserId, 
                    com.PostId, com.CreateDateTime as CommentDate

                    FROM Post p
                    
                    LEFT JOIN Comment com ON p.Id = com.PostId
                    LEFT JOIN [User] u ON com.UserId = u.Id
                    LEFT JOIN PostType pt ON p.PostTypeId = pt.Id

                    WHERE p.Id = @Id";

                    DbUtils.AddParameter(cmd, "@Id", postId);

                    var reader = cmd.ExecuteReader();

                    Post post = null;
                    while (reader.Read())
                    {
                        if (post == null)
                        {
                            post = new Post()
                            {
                                Id = postId,
                                Title = DbUtils.GetString(reader, "Title"),
                                Description = DbUtils.GetString(reader, "Description"),
                                DateCreated = DbUtils.GetDateTime(reader, "PostDate"),
                                Url = DbUtils.GetString(reader, "Url"),
                                PostTypeId = DbUtils.GetInt(reader, "PostTypeId"),
                                PostType = new PostType()
                                {
                                    Id = DbUtils.GetInt(reader, "PostTypeId"),
                                    Type = DbUtils.GetString(reader, "Type")
                                },
                                Comments = new List<Comment>()
                            };
                        }
                        if (DbUtils.IsNotDbNull(reader, "CommentId"))
                        {
                            post.Comments.Add(new Comment()
                            {
                                Id = DbUtils.GetInt(reader, "CommentId"),
                                Content = DbUtils.GetString(reader, "Content"),
                                PostId = postId,
                                UserId = DbUtils.GetInt(reader, "CommentUserId"),
                                User = new User()
                                {
                                    Id = DbUtils.GetInt(reader, "CommentUserId"),
                                    FirstName = DbUtils.GetString(reader, "FirstName"),
                                    LastName = DbUtils.GetString(reader, "LastName"),
                                    Email = DbUtils.GetString(reader, "Email"),
                                    PhoneNumber = DbUtils.GetString(reader, "PhoneNumber"),
                                    Age = DbUtils.GetInt(reader, "Age"),
                                    UserTypeId = DbUtils.GetInt(reader, "UserTypeId"),
                                    ProfilePhoto = DbUtils.GetString(reader, "ProfilePhoto"),
                                    DateCreated = DbUtils.GetDateTime(reader, "UserDate")
                                },
                                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CommentDate"))
                            });
                        }

                    }

                    reader.Close();

                    return post;
                }

            }
        }

        public Post GetSinglePostById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT p.Id as PostId, p.UserId as PostUserId, p.PostTypeId as CurrentPostTypeId, p.Title, p.Description,
                    p.DateCreated as PostDate, p.Url,

                    u.Id as UserId, u.FirstName, u.LastName, u.FullName, u.Email, u.PhoneNumber, 
                    u.Age, u.UserTypeId, u.ProfilePhoto, u.DateCreated as UserDate, 
                    
                    pt.Id as PostTypeId, pt.Type

                    FROM Post p
                    
                    LEFT JOIN [User] u ON p.UserId = u.Id
                    LEFT JOIN PostType pt ON p.PostTypeId = pt.Id

                    WHERE p.Id = @Id";

                    cmd.Parameters.AddWithValue("@Id", id);
                    var reader = cmd.ExecuteReader();

                    Post post = null;

                    if (reader.Read())
                    {
                        post = new Post()
                        {
                            Id = id,
                            UserId = DbUtils.GetInt(reader, "PostUserId"),
                            User = new User()
                            {
                                Id = DbUtils.GetInt(reader, "PostUserId"),
                                FirstName = DbUtils.GetString(reader, "FirstName"),
                                LastName = DbUtils.GetString(reader, "LastName"),
                                Email = DbUtils.GetString(reader, "Email"),
                                PhoneNumber = DbUtils.GetString(reader, "PhoneNumber"),
                                Age = DbUtils.GetInt(reader, "Age"),
                                UserTypeId = DbUtils.GetInt(reader, "UserTypeId"),
                                ProfilePhoto = DbUtils.GetString(reader, "ProfilePhoto"),
                                DateCreated = DbUtils.GetDateTime(reader, "UserDate")
                            },
                            PostTypeId = DbUtils.GetInt(reader, "CurrentPostTypeId"),
                            PostType = new PostType()
                            {
                                Id = DbUtils.GetInt(reader, "CurrentPostTypeId"),
                                Type = DbUtils.GetString(reader, "Type")
                            },
                            Title = DbUtils.GetString(reader, "Title"),
                            Description = DbUtils.GetString(reader, "Description"),
                            DateCreated = DbUtils.GetDateTime(reader, "PostDate"),
                            Url = DbUtils.GetString(reader, "Url")
                        };
                    }

                    reader.Close();

                    return post;
                }
            }
        }
    }
}

