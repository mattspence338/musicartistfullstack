using music_artist_full_stack.Models;

namespace music_artist_full_stack.Repositories
{
    public interface ICommentRepository
    {
        void AddComment(Comment comment);
        void DeleteComment(int commentId);
        void EditComment(Comment comment);
        Comment GetSingleCommentById(int id);
    }
}