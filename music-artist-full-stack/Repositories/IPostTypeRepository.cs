using music_artist_full_stack.Models;
using System.Collections.Generic;

namespace music_artist_full_stack.Repositories
{
    public interface IPostTypeRepository
    {
        List<PostType> GetAllPostTypes();
    }
}