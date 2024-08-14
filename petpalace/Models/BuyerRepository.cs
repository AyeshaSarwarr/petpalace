using Microsoft.Data.SqlClient;

namespace petpalace.Models
{
    public class BuyerRepository : IBuyerInterface
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public BuyerEntity Login(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Buyers.*, Users.Username, Users.Password FROM Buyers " +
                               "INNER JOIN Users ON Buyers.UserId = Users.UserId " +
                               "WHERE Users.Username = @Username AND Users.Password = @Password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                BuyerEntity buyer = null;
                if (reader.Read())
                {
                    buyer = new BuyerEntity
                    {
                        UserId = (int)reader["UserId"],
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        FullName = reader["FullName"].ToString(),
                        Email = reader["Email"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString()
                    };
                }
                reader.Close();
                return buyer;
            }
        }

        public void Signup(BuyerEntity buyer)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string userQuery = "INSERT INTO Users (Username, Password, Role) VALUES (@Username, @Password, @Role)";
                SqlCommand userCommand = new SqlCommand(userQuery, connection);
                userCommand.Parameters.AddWithValue("@Username", buyer.Username);
                userCommand.Parameters.AddWithValue("@Password", buyer.Password);
                userCommand.Parameters.AddWithValue("@Role", "Buyer");
                userCommand.ExecuteNonQuery();

                string userIdQuery = "SELECT UserId FROM Users WHERE Username = @Username";
                SqlCommand userIdCommand = new SqlCommand(userIdQuery, connection);
                userIdCommand.Parameters.AddWithValue("@Username", buyer.Username);
                int userId = (int)userIdCommand.ExecuteScalar();

                string buyerQuery = "INSERT INTO Buyers (UserId, FullName, Email, PhoneNumber) VALUES (@UserId, @FullName, @Email, @PhoneNumber)";
                SqlCommand buyerCommand = new SqlCommand(buyerQuery, connection);
                buyerCommand.Parameters.AddWithValue("@UserId", userId);
                buyerCommand.Parameters.AddWithValue("@FullName", buyer.FullName);
                buyerCommand.Parameters.AddWithValue("@Email", buyer.Email);
                buyerCommand.Parameters.AddWithValue("@PhoneNumber", buyer.PhoneNumber);
                buyerCommand.ExecuteNonQuery();
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
