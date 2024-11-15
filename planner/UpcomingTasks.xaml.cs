using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace planner
{
    public partial class UpcomingTasks : ContentPage
    {
        private const string ApiUrl = "https://api.todoist.com/rest/v2/tasks";
        private const string TodoistToken = "47dc5e927c4b905402f45c441ec7f87837047b9c"; // Replace with your token

        public UpcomingTasks()
        {
            InitializeComponent();
            LoadTasksForNextWeek();
        }

        private async void LoadTasksForNextWeek()
        {
            try
            {
                var tasks = await GetTasksForNextWeekAsync();
                TasksListView.ItemsSource = tasks;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load tasks: {ex.Message}", "OK");
            }
        }

        private async Task<List<TodoistTask>> GetTasksForNextWeekAsync()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TodoistToken);
                var response = await client.GetAsync(ApiUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var tasks = JsonConvert.DeserializeObject<List<TodoistTask>>(content);

                var today = DateTime.Today;
                var nextWeek = today.AddDays(7);

                // Filter tasks with due dates within the next 7 days
                return tasks.FindAll(task =>
                    task.due != null &&
                    DateTime.TryParse(task.due.date, out var dueDate) &&
                    dueDate.Date >= today && dueDate.Date <= nextWeek);
            }
        }

        private async void OnCompleteTaskClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is TodoistTask task)
            {
                bool isCompleted = await CompleteTaskAsync(task.id);
                if (isCompleted)
                {
                    var tasks = (List<TodoistTask>)TasksListView.ItemsSource;
                    tasks.Remove(task);
                    TasksListView.ItemsSource = null;
                    TasksListView.ItemsSource = tasks;
                }
                else
                {
                    await DisplayAlert("Error", "Failed to complete task in Todoist.", "OK");
                }
            }
        }

        private async Task<bool> CompleteTaskAsync(string taskId)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TodoistToken);

                var closeTaskUrl = $"{ApiUrl}/{taskId}/close";
                var response = await client.PostAsync(closeTaskUrl, null);

                return response.IsSuccessStatusCode;
            }
        }
    }

}
