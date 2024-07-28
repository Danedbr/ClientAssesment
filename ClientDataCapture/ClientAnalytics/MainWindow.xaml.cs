using Microsoft.Extensions.Configuration;
using System.Windows;

namespace ClientAnalyticsDisplayApp
{
    public partial class MainWindow : Window
    {
        private readonly ClientRepository _clientRepository;

        public MainWindow()
        {
            InitializeComponent();

            // Set up configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("MySqlConnection");
            _clientRepository = new ClientRepository(connectionString);
        }

        private async void LoadClients_Click(object sender, RoutedEventArgs e)
        {
            var clients = await _clientRepository.GetClientsAsync();
            ClientsDataGrid.ItemsSource = clients;
        }
    }
}
