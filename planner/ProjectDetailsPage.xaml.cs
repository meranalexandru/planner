using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Threading.Tasks;

namespace planner
{
    public partial class ProjectDetailsPage : ContentPage
    {
        private string _profilePicturePath;
        private Project _project;

        public ProjectDetailsPage(Project project = null)
        {
            InitializeComponent();

            _project = project ?? new Project();

            // Populate fields if editing an existing project
            ProjectNameEntry.Text = _project.Name;
            ProjectDescriptionEditor.Text = _project.Description;
            StartDatePicker.Date = _project.StartDate != DateTime.MinValue ? _project.StartDate : DateTime.Now;
            EndDatePicker.Date = _project.EndDate != DateTime.MinValue ? _project.EndDate : DateTime.Now;

            if (!string.IsNullOrEmpty(_project.ProfilePicturePath))
            {
                _profilePicturePath = _project.ProfilePicturePath;
                ProfilePictureImage.Source = ImageSource.FromFile(_profilePicturePath);
            }
        }

        private async void OnUploadProfilePictureClicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Select a project profile picture"
            });

            if (result != null)
            {
                var newFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), result.FileName);
                using (var stream = await result.OpenReadAsync())
                using (var newStream = File.Create(newFile))
                {
                    await stream.CopyToAsync(newStream);
                }
                _profilePicturePath = newFile;
                ProfilePictureImage.Source = ImageSource.FromFile(_profilePicturePath);
            }
        }

        private async void OnSaveProjectClicked(object sender, EventArgs e)
        {
            _project.Name = ProjectNameEntry.Text;
            _project.Description = ProjectDescriptionEditor.Text;
            _project.StartDate = StartDatePicker.Date;
            _project.EndDate = EndDatePicker.Date;
            _project.ProfilePicturePath = _profilePicturePath;

            if (_project.ID == 0)
            {
                await App.Database.SaveProjectAsync(_project);
            }
            else
            {
                await App.Database.UpdateProjectAsync(_project);
            }

            await Navigation.PopAsync();
        }
    }
}
