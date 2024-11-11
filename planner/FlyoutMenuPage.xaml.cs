using Microsoft.Maui.Controls;
using System;

namespace planner
{
    public partial class FlyoutMenuPage : FlyoutPage
    {
        public string TodayIconText { get; set; }

        public FlyoutMenuPage()
        {
            InitializeComponent();

            // Set the current day number for the "Today" button
            TodayIconText = DateTime.Now.Day.ToString();
            BindingContext = this;
        }

        private async void OnTodayClicked(object sender, EventArgs e)
        {
            await NavigateToPageAsync(new MainPage());
        }

        private async void OnProjectsClicked(object sender, EventArgs e)
        {
            await NavigateToPageAsync(new ProjectsPage());
        }

        private async Task NavigateToPageAsync(Page page)
        {
            // Set up navigation with a NavigationPage
            var navigationPage = new NavigationPage(page);
            navigationPage.Popped += OnNavigationPopped; // Attach event for navigation complete
            Detail = navigationPage;

            // Wait for a brief delay to allow page rendering to start
            await Task.Delay(100);

            // Now, safely close the sidebar
        //    IsPresented = false;
        }

        private void OnNavigationPopped(object sender, NavigationEventArgs e)
        {
            // Ensure we detach event after navigation to prevent potential memory leaks
            if (sender is NavigationPage navigationPage)
            {
                navigationPage.Popped -= OnNavigationPopped;
            }
        }
    }
}
