using Microsoft.Maui.Controls; // For MAUI components
using System; // For DateTime

namespace planner
{
    public partial class TaskDetailsPage : ContentPage
    {
        private TaskItem _task;
        private bool _isNewTask;

        public TaskDetailsPage(TaskItem task = null)
        {
            InitializeComponent();

            _isNewTask = task == null;
            _task = task ?? new TaskItem();

            // Initialize fields with existing task data or defaults
            TaskNameEntry.Text = _task.Name;
            DueDatePicker.Date = DateTime.TryParse(_task.DueDate, out var dueDate) ? dueDate : DateTime.Now;
            DescriptionEditor.Text = _task.Description;
            PriorityPicker.SelectedItem = _task.Priority > 0 ? _task.Priority : (object)null;
            IsContinuousSwitch.IsToggled = _task.IsContinuous;
        }

        private async void OnSaveTaskClicked(object sender, EventArgs e)
        {
            _task.Name = TaskNameEntry.Text;
            _task.DueDate = DueDatePicker.Date.ToString("yyyy-MM-dd");
            _task.Description = DescriptionEditor.Text;
            _task.Priority = (int)(PriorityPicker.SelectedItem ?? 0);
            _task.IsContinuous = IsContinuousSwitch.IsToggled;

            if (_isNewTask)
                await App.Database.SaveTaskAsync(_task);
            else
                await App.Database.UpdateTaskAsync(_task);

            await Navigation.PopAsync(); // Go back to the previous page
        }
    }
}
