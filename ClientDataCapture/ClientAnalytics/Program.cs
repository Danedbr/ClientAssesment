using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ClientAnalyticsDisplayApp
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

        public async Task<IEnumerable<Client>> GetClientsAsync()
        {
            var clients = new List<Client>();
            var query = "SELECT Name, DateRegistered, Location, NumberOfUsers FROM Clients";

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                clients.Add(new Client
                {
                    Name = reader.GetString("Name"),
                    DateRegistered = reader.GetDateTime("DateRegistered"),
                    Location = reader.GetString("Location"),
                    NumberOfUsers = reader.GetInt32("NumberOfUsers")
                });
            }

            return clients;
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
