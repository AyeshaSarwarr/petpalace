using Microsoft.Data.SqlClient;

namespace petpalace.Models
{
    public class SellerRepository : ISellerInterface
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public SellerEntity Login(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Sellers.*, Users.Username, Users.Password FROM Sellers " +
                               "INNER JOIN Users ON Sellers.UserId = Users.UserId " +
                               "WHERE Users.Username = @Username AND Users.Password = @Password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                SellerEntity seller = null;
                if (reader.Read())
                {
                    seller = new SellerEntity
                    {
                        UserId = (int)reader["UserId"],
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        BusinessName = reader["BusinessName"].ToString(),
                        BusinessAddress = reader["BusinessAddress"].ToString(),
                        ContactNumber = reader["ContactNumber"].ToString(),
                        Website = reader["Website"].ToString()
                    };
                }
                reader.Close();
                return seller;
            }
        }

        public void Signup(SellerEntity seller)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string userQuery = "INSERT INTO Users (Username, Password, Role) VALUES (@Username, @Password, @Role)";
                SqlCommand userCommand = new SqlCommand(userQuery, connection);
                userCommand.Parameters.AddWithValue("@Username", seller.Username);
                userCommand.Parameters.AddWithValue("@Password", seller.Password);
                userCommand.Parameters.AddWithValue("@Role", "Seller");
                userCommand.ExecuteNonQuery();

                string userIdQuery = "SELECT UserId FROM Users WHERE Username = @Username";
                SqlCommand userIdCommand = new SqlCommand(userIdQuery, connection);
                userIdCommand.Parameters.AddWithValue("@Username", seller.Username);
                int userId = (int)userIdCommand.ExecuteScalar();

                string sellerQuery = "INSERT INTO Sellers (UserId, BusinessName, BusinessAddress, ContactNumber, Website) VALUES (@UserId, @BusinessName, @BusinessAddress, @ContactNumber, @Website)";
                SqlCommand sellerCommand = new SqlCommand(sellerQuery, connection);
                sellerCommand.Parameters.AddWithValue("@UserId", userId);
                sellerCommand.Parameters.AddWithValue("@BusinessName", seller.BusinessName);
                sellerCommand.Parameters.AddWithValue("@BusinessAddress", seller.BusinessAddress);
                sellerCommand.Parameters.AddWithValue("@ContactNumber", seller.ContactNumber);
                sellerCommand.Parameters.AddWithValue("@Website", seller.Website);
                sellerCommand.ExecuteNonQuery();
            }
        }

        public bool UserExists(string username)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(1) FROM Users WHERE Username = @Username";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);

                connection.Open();
                int userCount = (int)command.ExecuteScalar();
                return userCount > 0;
            }
        }
    }
}
