using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace planner
{
    public partial class CalendarPage : ContentPage
    {
        public ObservableCollection<TaskItem> UpcomingEvents { get; set; } = new ObservableCollection<TaskItem>();

        public CalendarPage()
        {
            InitializeComponent();
            BindingContext = this;
            LoadEvents();
        }

        private async void LoadEvents()
        {
            var tasks = await App.Database.GetTasksAsync();
            UpcomingEvents.Clear();
            foreach (var task in tasks)
            {
                UpcomingEvents.Add(task);
            }
        }

        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            var selectedDate = e.NewDate.ToString("yyyy-MM-dd");

            // Filter tasks for the selected date
            var eventsOnSelectedDate = UpcomingEvents.Where(task => task.DueDate == selectedDate).ToList();
            EventsListView.ItemsSource = eventsOnSelectedDate;
        }
    }
}
