using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace planner
{
    public partial class StatisticsPage : ContentPage
    {
        public StatisticsPage()
        {
            InitializeComponent();
            LoadDueDateChart();
        }

        private async void LoadDueDateChart()
        {
            try
            {
                // Retrieve tasks grouped by due date from the database
                var tasksByDueDate = await App.Database.GetTasksByDueDateAsync();

                // Prepare chart data for each day in the upcoming week
                var today = DateTime.Today;
                var chartData = new List<int>();

                for (int i = 0; i <= 6; i++)
                {
                    var day = today.AddDays(i);
                    var taskCount = tasksByDueDate.ContainsKey(day) ? tasksByDueDate[day] : 0;
                    chartData.Add(taskCount);
                }

                Console.WriteLine("Loaded Chart Data: " + string.Join(", ", chartData));

                // Use static data for testing if chartData is empty
                TaskDueDateChart.Drawable = chartData.Count > 0
                    ? new TaskDueDateChartDrawable(chartData)
                    : new TaskDueDateChartDrawable(new List<int> { 5, 10, 3, 7, 2, 6, 4 });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load chart data: {ex.Message}", "OK");
            }
        }
    }
}
