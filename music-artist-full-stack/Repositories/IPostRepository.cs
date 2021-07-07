using music_artist_full_stack.Models;
using System.Collections.Generic;

namespace music_artist_full_stack.Repositories
{
    public interface IPostRepository
    {
        List<Post> GetAllPublishedPosts();
        void AddPost(Post post);
        void DeletePost(int PostId);
        void EditPost(Post post);
        Post GetPostByIdWithComments(int postId);
        Post GetSinglePostById(int id);

    }
}