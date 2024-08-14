namespace petpalace.Models
{
    public interface ISellerInterface
    {
        SellerEntity Login(string username, string password);
        void Signup(SellerEntity seller);
        bool UserExists(string username);
    }
}
