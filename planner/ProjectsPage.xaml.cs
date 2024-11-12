using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System;

namespace planner
{
    public partial class ProjectsPage : ContentPage
    {
        public ProjectsPage()
        {
            InitializeComponent();
            LoadProjects();
        }

        private async void LoadProjects()
        {
            var projects = await App.Database.GetProjectsAsync();
            ProjectsCollectionView.ItemsSource = projects;
        }

        private async void OnAddProjectClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProjectDetailsPage());
        }

        private async void OnProjectSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0)
                return;

            // Get the selected project
            var selectedProject = e.CurrentSelection[0] as Project;
            if (selectedProject != null)
            {
                // Navigate to ProjectDetailsPage with the selected project for editing
                await Navigation.PushAsync(new ProjectDetailsPage(selectedProject));
            }

            // Clear selection (optional, to avoid it remaining selected)
            ProjectsCollectionView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadProjects(); // Refresh projects when page appears
        }
    }
}
