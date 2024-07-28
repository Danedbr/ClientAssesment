using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace ClientDataCapture
{
    class Program
    {
        private static IConfiguration Configuration { get; set; }

        static async Task Main(string[] args)
        {
            // Set up configuration to read from appsettings.json
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            // Retrieve connection string
            var connectionString = Configuration.GetConnectionString("MySqlConnection");

            Console.WriteLine("Client Data Capture");

            // Collect client data from the user
            Console.Write("Enter Client Name: ");
            var name = Console.ReadLine();

            Console.Write("Enter Date Registered (YYYY-MM-DD): ");
            var dateRegisteredInput = Console.ReadLine();
            var dateRegistered = DateTime.Parse(dateRegisteredInput);

            Console.Write("Enter Location: ");
            var location = Console.ReadLine();

            Console.Write("Enter Number of Users: ");
            var numberOfUsersInput = Console.ReadLine();
            var numberOfUsers = int.TryParse(numberOfUsersInput, out var users) ? users : 0;

            // Create a new client object
            var client = new Client
            {
                Name = name,
                DateRegistered = dateRegistered,
                Location = location,
                NumberOfUsers = numberOfUsers
            };

            try
            {
                await AddClientAsync(client, connectionString);
                Console.WriteLine("Client added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static async Task AddClientAsync(Client client, string connectionString)
        {
            var query = "INSERT INTO clients (Name, DateRegistered, Location, NumberOfUsers) VALUES (@Name, @DateRegistered, @Location, @NumberOfUsers)";

            using var connection = new MySqlConnection(connectionString);
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
