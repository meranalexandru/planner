using System.IO; // For file path management
using Microsoft.Maui.Controls; // For MAUI components

namespace planner
{
    public partial class App : Application
    {
        public static DatabaseHelper Database { get; private set; }

        public App()
        {
            InitializeComponent();

            // Set up the SQLite database path and initialize DatabaseHelper
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Planner.db3");
            Database = new DatabaseHelper(dbPath);

            // Set the FlyoutMenuPage as the main page
            MainPage = new FlyoutMenuPage();
        }
    }
}
