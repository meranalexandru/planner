using Microsoft.Maui.Controls;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace planner
{
    public partial class TaskDetailsPage : ContentPage
    {
        private TaskItem _task;
        private bool _isNewTask;
        private List<Project> _projects;

        public TaskDetailsPage(TaskItem task = null)
        {
            InitializeComponent();

            _isNewTask = task == null;
            _task = task ?? new TaskItem();

            // Load project data and initialize UI fields
            LoadProjectsAsync();

            TaskNameEntry.Text = _task.Name;
            DueDatePicker.Date = _task.DueDate != default(DateTime) ? _task.DueDate : DateTime.Now;
            DescriptionEditor.Text = _task.Description;
            PriorityPicker.SelectedItem = _task.Priority > 0 ? _task.Priority : (object)null;
            IsContinuousSwitch.IsToggled = _task.IsContinuous;

            // Show edit/delete buttons for existing tasks
            EditButton.IsVisible = !_isNewTask;
            DeleteButton.IsVisible = !_isNewTask;
        }

        private async Task LoadProjectsAsync()
        {
            _projects = await App.Database.GetProjectsAsync();
            ProjectPicker.ItemsSource = _projects.Select(p => p.Name).ToList();

            // Set the selected project if the task already has a ProjectId
            if (_task.ProjectId != 0)
            {
                var project = _projects.FirstOrDefault(p => p.ID == _task.ProjectId);
                if (project != null)
                    ProjectPicker.SelectedItem = project.Name;
            }
        }

        private async void OnSaveTaskClicked(object sender, EventArgs e)
        {
            // Validate that the task has a name
            if (string.IsNullOrWhiteSpace(TaskNameEntry.Text))
            {
                await DisplayAlert("Validation Error", "Task name cannot be empty.", "OK");
                return;
            }

            // Validate that a project is assigned
            var selectedProject = _projects.FirstOrDefault(p => p.Name == (string)ProjectPicker.SelectedItem);
            if (selectedProject == null)
            {
                await DisplayAlert("Validation Error", "You must assign the task to a project.", "OK");
                return;
            }

            // Validate that the due date is today or in the future
            if (DueDatePicker.Date < DateTime.Today)
            {
                await DisplayAlert("Validation Error", "The due date must be today or a future date.", "OK");
                return;
            }

            // Update task properties
            _task.Name = TaskNameEntry.Text;
            _task.DueDate = DueDatePicker.Date;
            _task.Description = DescriptionEditor.Text;
            _task.Priority = (int)(PriorityPicker.SelectedItem ?? 0);
            _task.IsContinuous = IsContinuousSwitch.IsToggled;
            _task.ProjectId = selectedProject.ID;

            // Save or update the task
            if (_isNewTask)
                await App.Database.SaveTaskAsync(_task);
            else
                await App.Database.UpdateTaskAsync(_task);

            await Navigation.PopAsync();
        }


        private void OnEditTaskClicked(object sender, EventArgs e)
        {
            // Enable editing of fields
            TaskNameEntry.IsEnabled = true;
            DueDatePicker.IsEnabled = true;
            DescriptionEditor.IsEnabled = true;
            PriorityPicker.IsEnabled = true;
            IsContinuousSwitch.IsEnabled = true;
            ProjectPicker.IsEnabled = true;

            // Hide the edit button after enabling editing
            EditButton.IsVisible = false;
        }

        private async void OnDeleteTaskClicked(object sender, EventArgs e)
        {
            var confirm = await DisplayAlert("Delete Task", "Are you sure you want to delete this task?", "Yes", "No");
            if (confirm)
            {
                await App.Database.DeleteTaskAsync(_task);
                await Navigation.PopAsync();
            }
        }
    }
}
