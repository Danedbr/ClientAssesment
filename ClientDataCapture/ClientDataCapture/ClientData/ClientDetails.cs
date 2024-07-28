using MySql.Data.MySqlClient;

namespace ClientDataCapture.ClientData
{
    public class ClientRepository
    {
        private readonly string _connectionString;

        public ClientRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddClientAsync(Client client)
        {
            var query = "INSERT INTO Clients (Name, DateRegistered, Location, NumberOfUsers) VALUES (@Name, @DateRegistered, @Location, @NumberOfUsers)";

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", client.Name);
            command.Parameters.AddWithValue("@DateRegistered", client.DateRegistered);
            command.Parameters.AddWithValue("@Location", client.Location);
            command.Parameters.AddWithValue("@NumberOfUsers", client.NumberOfUsers);

            await command.ExecuteNonQueryAsync();
        }
    }

    public class Client
    {
        public string Name { get; set; }
        public DateTime DateRegistered { get; set; }
        public string Location { get; set; }
        public int NumberOfUsers { get; set; }
    }
}
