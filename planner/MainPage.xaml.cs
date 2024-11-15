using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using System;

namespace planner
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<TaskItem> TodayTasks { get; set; } = new ObservableCollection<TaskItem>();

        public MainPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<ProjectViewPage>(this, "RefreshMainPage", (sender) =>
            {
                LoadTodayPendingTasks(); // Reload tasks on the main page
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadTodayPendingTasks(); // Refresh tasks each time the page appears
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<ProjectViewPage>(this, "RefreshMainPage");
        }

        private async void LoadTodayPendingTasks()
        {
            var tasks = await App.Database.GetTodayPendingTasksAsync(); // Load only today's pending tasks
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

        private async void OnMarkAsDoneClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is TaskItem taskToMarkDone)
            {
                await App.Database.MarkTaskAsDoneAsync(taskToMarkDone);
                TodayTasks.Remove(taskToMarkDone); // Remove task from the list after marking it as done
            }
        }

        private async void OnEditTaskClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is TaskItem taskToEdit)
            {
                await Navigation.PushAsync(new TaskDetailsPage(taskToEdit)); // Navigate to TaskDetailsPage with the selected task
            }
        }

        private async void OnDeleteTaskClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is TaskItem taskToDelete)
            {
                var confirm = await DisplayAlert("Delete Task", "Are you sure you want to delete this task?", "Yes", "No");
                if (confirm)
                {
                    await App.Database.DeleteTaskAsync(taskToDelete); // Delete from database
                    TodayTasks.Remove(taskToDelete); // Remove from ObservableCollection
                }
            }
        }
    }
}
