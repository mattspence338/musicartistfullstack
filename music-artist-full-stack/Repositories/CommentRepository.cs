using Microsoft.Extensions.Configuration;
using music_artist_full_stack.Models;
using music_artist_full_stack.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace music_artist_full_stack.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public CommentRepository(IConfiguration config) : base(config) { }

        public void AddComment(Comment comment)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Comment (
                            Content, UserId, PostId, CreateDateTime)
                        OUTPUT INSERTED.ID
                        VALUES (
                            @Content, @UserId, @PostId, @CreateDateTime)";

                    cmd.Parameters.AddWithValue("@Content", comment.Content);
                    cmd.Parameters.AddWithValue("@UserId", comment.UserId);
                    cmd.Parameters.AddWithValue("@PostId", comment.PostId);
                    cmd.Parameters.AddWithValue("@CreateDateTime", DateTime.Now);

                    comment.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void EditComment(Comment comment)
        {
            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Comment
                            SET 
                                Content = @Content, 
                                UserId = @UserId, 
                                PostId = @PostId, 
                                CreateDateTime = @CreateDateTime
                            WHERE Id = @Id";

                    cmd.Parameters.AddWithValue("@Content", comment.Content);
                    cmd.Parameters.AddWithValue("@UserId", comment.UserId);
                    cmd.Parameters.AddWithValue("@PostId", comment.PostId);
                    cmd.Parameters.AddWithValue("@CreateDateTime", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Id", comment.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteComment(int commentId)
        {
            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Comment
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", commentId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Comment GetSingleCommentById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT com.Id AS CommentId, com.Content, com.UserId as CommentUserId, 
                    com.PostId, com.CreateDateTime as CommentDate,

                    u.Id as UserId, u.FirstName, u.LastName, u.FullName, u.Email, u.PhoneNumber, 
                    u.Age, u.UserTypeId, u.ProfilePhoto, u.DateCreated as UserDate 

                    FROM Comment com
                    
                    LEFT JOIN [User] u ON com.UserId = u.Id

                    WHERE com.Id = @Id";

                    cmd.Parameters.AddWithValue("@Id", id);
                    var reader = cmd.ExecuteReader();

                    Comment comment = null;

                    while (reader.Read())
                    {
                        if (comment == null)
                        {
                            comment = new Comment()
                            {
                                Id = id,
                                Content = DbUtils.GetString(reader, "Content"),
                                PostId = DbUtils.GetInt(reader, "PostId"),
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
                            };
                        }
                    }
                    reader.Close();

                    return comment;
                }
            }
        }
    }
}

