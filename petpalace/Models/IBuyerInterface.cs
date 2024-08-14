namespace petpalace.Models
{
    public interface IBuyerInterface
    {
        BuyerEntity Login(string username, string password);
        void Signup(BuyerEntity buyer);
        bool UserExists(string username);
    }
}
