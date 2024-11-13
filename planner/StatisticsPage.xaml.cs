using Microsoft.Maui.Controls;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
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

                // Prepare chart entries for each day in the upcoming week
                var chartEntries = new List<ChartEntry>();
                var today = DateTime.Today;

                for (int i = 0; i <= 6; i++)
                {
                    var day = today.AddDays(i);
                    var taskCount = tasksByDueDate.ContainsKey(day) ? tasksByDueDate[day] : 0;

                    chartEntries.Add(new ChartEntry(taskCount)
                    {
                        Label = day.ToString("ddd"),
                        ValueLabel = taskCount.ToString(),
                        Color = SKColor.Parse("#3498db")
                    });
                }

                // Assign the entries to the bar chart
                TaskDueDateChart.Chart = new BarChart
                {
                    Entries = chartEntries,
                    LabelTextSize = 30,
                    ValueLabelOrientation = Orientation.Vertical,
                    LabelOrientation = Orientation.Horizontal,
                    MaxValue = (float)(chartEntries.Max(e => e.Value) + 1) // Adjust max for visual clarity
                };
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load chart data: {ex.Message}", "OK");
            }
        }
    }
}
