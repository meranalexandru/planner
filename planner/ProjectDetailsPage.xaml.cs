using Microsoft.Maui.Controls;
using System;

namespace planner
{
    public partial class ProjectDetailsPage : ContentPage
    {
        private Project _project;
        private bool _isNewProject;

        public ProjectDetailsPage(Project project = null)
        {
            InitializeComponent();

            _isNewProject = project == null;
            _project = project ?? new Project();

            // Populate fields if editing an existing project
            ProjectNameEntry.Text = _project.Name;
            ProjectDescriptionEditor.Text = _project.Description;
        }

        private async void OnSaveProjectClicked(object sender, EventArgs e)
        {
            _project.Name = ProjectNameEntry.Text;
            _project.Description = ProjectDescriptionEditor.Text;

            if (_isNewProject)
                await App.Database.SaveProjectAsync(_project);
            else
                await App.Database.UpdateProjectAsync(_project);

            await Navigation.PopAsync(); // Return to ProjectsPage
        }
    }
}
