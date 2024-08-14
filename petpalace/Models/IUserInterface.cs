namespace petpalace.Models
{
    public interface IUserInterface
    {
        UserEntity Login(string username, string password);
        void Signup(UserEntity user);
        bool UserExists(string username);
    }
}
