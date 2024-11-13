using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace planner
{
    public partial class TodoistIntegrationPage : ContentPage
    {
        private const string ApiUrl = "https://api.todoist.com/rest/v2/tasks";
        private const string TodoistToken = ""; // Replace with your Todoist API token

        public TodoistIntegrationPage()
        {
            InitializeComponent();
            LoadTodayTasks();
        }

        private async void LoadTodayTasks()
        {
            try
            {
                var tasks = await GetTodayTasksAsync();
                TasksListView.ItemsSource = tasks;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load tasks: {ex.Message}", "OK");
            }
        }

        private async Task<List<TodoistTask>> GetTodayTasksAsync()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TodoistToken);
                var response = await client.GetAsync(ApiUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var tasks = JsonConvert.DeserializeObject<List<TodoistTask>>(content);

                var today = DateTime.Today;
                return tasks.FindAll(task =>
                    task.due != null &&
                    DateTime.TryParse(task.due.date, out var dueDate) &&
                    dueDate.Date == today);
            }
        }

        private async void OnCompleteTaskClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is TodoistTask task)
            {
                bool isCompleted = await CompleteTaskAsync(task.id);
                if (isCompleted)
                {
                    // Update UI by removing completed task
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

    public class TodoistTask
    {
        public string id { get; set; }
        public string content { get; set; }
        public string description { get; set; }
        public DueDate due { get; set; }
        public string url { get; set; }
    }

    public class DueDate
    {
        public string date { get; set; }
        public bool is_recurring { get; set; }
        public string datetime { get; set; }
        public string timezone { get; set; }
    }
}
