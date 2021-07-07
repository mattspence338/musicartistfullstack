using Microsoft.Extensions.Configuration;
using music_artist_full_stack.Models;
using music_artist_full_stack.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace music_artist_full_stack.Repositories
{
    public class PostTypeRepository : BaseRepository, IPostTypeRepository
    {
        public PostTypeRepository(IConfiguration config) : base(config) { }

        public List<PostType> GetAllPostTypes()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Type]
                        FROM PostType";

                    var reader = cmd.ExecuteReader();

                    var postTypes = new List<PostType>();
                    while (reader.Read())
                    {
                        var postTypeId = DbUtils.GetInt(reader, "Id");
                        var postType = postTypes.FirstOrDefault(pt => pt.Id == postTypeId);

                        if (postType == null)
                        {
                            postType = new PostType()
                            {
                                Id = postTypeId,
                                Type = DbUtils.GetString(reader, "Type"),
                            };

                            postTypes.Add(postType);
                        }
                    }
                    reader.Close();

                    return postTypes;
                }
            }
        }
    }
}
