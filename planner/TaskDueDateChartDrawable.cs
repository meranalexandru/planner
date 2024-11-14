using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace planner
{
    public class TaskDueDateChartDrawable : IDrawable
    {
        private readonly List<int> _data;
        private readonly List<string> _dayLabels;

        public TaskDueDateChartDrawable(List<int> data)
        {
            _data = data;
            _dayLabels = Enumerable.Range(0, 7)
                                   .Select(i => DateTime.Today.AddDays(i).ToString("ddd"))
                                   .ToList();
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            // Set padding and calculate drawable width
            float padding = 20;
            float drawableWidth = dirtyRect.Width - 2 * padding;

            // Calculate dynamic bar width and spacing based on available width
            float barWidth = drawableWidth / (_data.Count * 1.5f); // Adjust 1.5 to control spacing
            float barSpacing = barWidth / 2;

            // Set chart height and max bar height based on available height
            float chartHeight = dirtyRect.Height - 2 * padding;
            float maxBarHeight = chartHeight * 0.7f; // 70% of available height for bars

            // Calculate font sizes relative to the available height
            float labelFontSize = chartHeight * 0.05f;
            float valueFontSize = chartHeight * 0.06f;

            var xPosition = padding;
            var maxDataValue = _data.Max() > 0 ? _data.Max() : 1; // Prevent division by zero

            // Set the background color for visibility
            canvas.FillColor = Colors.LightGray;
            canvas.FillRectangle(dirtyRect);

            // Draw each bar with labels for task count and day
            for (int i = 0; i < _data.Count; i++)
            {
                var taskCount = _data[i];
                var dayLabel = _dayLabels[i];
                var barHeight = (float)(taskCount / (double)maxDataValue * maxBarHeight);

                // Draw the bar
                canvas.FillColor = Colors.CornflowerBlue;
                canvas.FillRectangle(xPosition, chartHeight - barHeight, barWidth, barHeight);

                // Draw task count above the bar
                canvas.FontColor = Colors.Black;
                canvas.FontSize = valueFontSize;
                canvas.DrawString(taskCount.ToString(), xPosition + (barWidth / 2), chartHeight - barHeight - 15, HorizontalAlignment.Center);

                // Draw day label below the bar
                canvas.FontSize = labelFontSize;
                canvas.DrawString(dayLabel, xPosition + (barWidth / 2), chartHeight + 15, HorizontalAlignment.Center);

                // Move x position for the next bar
                xPosition += barWidth + barSpacing;
            }
        }
    }
}
