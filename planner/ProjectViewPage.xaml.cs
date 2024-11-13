using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace planner
{
    public partial class ProjectViewPage : ContentPage
    {
        private readonly Project _project;
        public ObservableCollection<TaskItem> Tasks { get; set; } = new ObservableCollection<TaskItem>();

        public ProjectViewPage(Project project)
        {
            InitializeComponent();
            _project = project;
            BindingContext = this;
            LoadTasks();
        }

        private async void LoadTasks()
        {
            // Fetch tasks for this project and sort by priority
            var tasks = await App.Database.GetTasksForProjectAsync(_project.ID);
            var sortedTasks = tasks.OrderBy(t => t.Priority).ToList();

            // Clear and reload tasks
            Tasks.Clear();
            foreach (var task in sortedTasks)
            {
                task.PropertyChanged += OnTaskPropertyChanged;
                Tasks.Add(task);
            }
        }

        private async void OnTaskPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is TaskItem modifiedTask)
            {
                // Save task modification to the database when any key property changes
                await App.Database.UpdateTaskAsync(modifiedTask);

                // Refresh the main page by sending a message
                MessagingCenter.Send(this, "RefreshMainPage");
            }
        }

        private async void OnDeleteTaskClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is TaskItem taskToDelete)
            {
                bool confirmDelete = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this task?", "Yes", "No");
                if (confirmDelete)
                {
                    Tasks.Remove(taskToDelete); // Remove from UI
                    await App.Database.DeleteTaskAsync(taskToDelete); // Remove from database

                    // Send message to refresh main page
                    MessagingCenter.Send(this, "RefreshMainPage");
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            foreach (var task in Tasks)
            {
                task.PropertyChanged -= OnTaskPropertyChanged;
            }
        }
    }
}
