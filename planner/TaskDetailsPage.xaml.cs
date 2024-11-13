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
            _task.Name = TaskNameEntry.Text;
            _task.DueDate = DueDatePicker.Date; // Assign DateTime directly
            _task.Description = DescriptionEditor.Text;
            _task.Priority = (int)(PriorityPicker.SelectedItem ?? 0);
            _task.IsContinuous = IsContinuousSwitch.IsToggled;

            // Assign selected project to the task
            var selectedProject = _projects.FirstOrDefault(p => p.Name == (string)ProjectPicker.SelectedItem);
            _task.ProjectId = selectedProject?.ID ?? 0;

            if (_isNewTask)
                await App.Database.SaveTaskAsync(_task);
            else
                await App.Database.UpdateTaskAsync(_task);

            await Navigation.PopAsync();
        }
    }
}
