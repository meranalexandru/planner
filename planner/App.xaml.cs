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

            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Planner.db3");
            Database = new DatabaseHelper(dbPath);

            MainPage = new NavigationPage(new MainPage());
        }
    }
}
