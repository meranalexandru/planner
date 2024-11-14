using Microsoft.Maui.Controls;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
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

                // Prepare chart data for each day in the upcoming week
                var today = DateTime.Today;
                var chartData = new List<int>();

                for (int i = 0; i <= 6; i++)
                {
                    var day = today.AddDays(i);
                    var taskCount = tasksByDueDate.ContainsKey(day) ? tasksByDueDate[day] : 0;
                    chartData.Add(taskCount);
                }

                // Set up the GraphicsView with a drawable for custom chart rendering
                TaskDueDateChart.Drawable = new TaskDueDateChartDrawable(chartData);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load chart data: {ex.Message}", "OK");
            }
        }
    }

    public class TaskDueDateChartDrawable : IDrawable
    {
        private readonly List<int> _data;

        public TaskDueDateChartDrawable(List<int> data)
        {
            _data = data;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            var barWidth = 30;
            var barSpacing = 20;
            var maxBarHeight = 200;
            var xPosition = 20;
            var maxDataValue = _data.Max();

            foreach (var value in _data)
            {
                var barHeight = (float)(value / (double)maxDataValue * maxBarHeight);
                canvas.FillColor = Color.FromRgb(52, 152, 219);
                canvas.FillRectangle(xPosition, maxBarHeight - barHeight, barWidth, barHeight);
                xPosition += barWidth + barSpacing;
            }
        }
    }
}
