using System.Collections.ObjectModel; // For ObservableCollection
using Microsoft.Maui.Controls; // For MAUI components
using System; // For DateTime

namespace planner
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<TaskItem> TodayTasks { get; set; } = new ObservableCollection<TaskItem>();

        private bool _isSwiping = false; // Flag to differentiate between swipe and tap

        public MainPage()
        {
            InitializeComponent();
            LoadTasks();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadTasks(); // Refresh tasks each time the page appears
        }

        private async void LoadTasks()
        {
            var tasks = await App.Database.GetTasksAsync();
            TodayTasks.Clear();
            foreach (var task in tasks)
            {
                TodayTasks.Add(task);
            }
            TasksCollectionView.ItemsSource = TodayTasks;
        }

        private async void OnAddTaskClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TaskDetailsPage());
        }

        private async void OnTaskSelected(object sender, SelectionChangedEventArgs e)
        {
            if (_isSwiping)
            {
                // Reset the selection and return early if swiping
                TasksCollectionView.SelectedItem = null;
                return;
            }

            if (e.CurrentSelection.FirstOrDefault() is TaskItem selectedTask)
            {
                await Navigation.PushAsync(new TaskDetailsPage(selectedTask));
            }
        }

        private async void OnDeleteTaskInvoked(object sender, EventArgs e)
        {
            _isSwiping = true; // Set flag to prevent SelectionChanged from firing
            if (sender is SwipeItem swipeItem && swipeItem.CommandParameter is TaskItem taskToDelete)
            {
                await App.Database.DeleteTaskAsync(taskToDelete);
                TodayTasks.Remove(taskToDelete);
            }
            _isSwiping = false; // Reset flag after swipe operation is complete
        }

        private async void OnDeleteTaskButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is TaskItem taskToDelete)
            {
                await App.Database.DeleteTaskAsync(taskToDelete);
                TodayTasks.Remove(taskToDelete);
            }
        }
    }
}
