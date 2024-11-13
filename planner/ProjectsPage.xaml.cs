using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

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

        private async void OnViewProjectClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Project projectToView)
            {
                // Navigate to the ProjectViewPage with the selected project
                await Navigation.PushAsync(new ProjectViewPage(projectToView));
            }
        }

        private async void OnAddProjectClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProjectDetailsPage());
        }

        private async void OnEditProjectClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Project projectToEdit)
            {
                await Navigation.PushAsync(new ProjectDetailsPage(projectToEdit));
            }
        }

        private async void OnDeleteProjectClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Project projectToDelete)
            {
                bool confirmDelete = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this project?", "Yes", "No");
                if (confirmDelete)
                {
                    await App.Database.DeleteProjectAsync(projectToDelete);
                    LoadProjects(); // Refresh the project list
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadProjects(); // Refresh projects when page appears
        }
    }
}
