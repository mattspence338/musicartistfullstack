using music_artist_full_stack.Models;

namespace music_artist_full_stack.Repositories
{
    public interface IUserRepository
    {
        void AddUser(User user);
        User GetByFirebaseUserId(string firebaseUserId);
        void EditUser(User user);
        void DeleteUser(int userId);
    }
}