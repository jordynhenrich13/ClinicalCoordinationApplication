using ClinicalCoordinationApplication.Model;
using Microsoft.Maui.Controls;

namespace ClinicalCoordinationApplication
{
    public partial class MainPage : FlyoutPage
    {

        public MainPage()
        {
            InitializeComponent();
            Flyout = new FlyoutMenuPage();
        }

        public MainPage(string userRole)
        {
            InitializeComponent();

            if (userRole == "Coordinator")
            {
                Detail = new NavigationPage(new CoordinatorDashboard());
            }
            else
            {
                Detail = new NavigationPage(new StudentDashMain());
            }

            Flyout = new FlyoutMenuPage();
        }


        void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Code for the FlyoutMenuNavigation
            var item = e.CurrentSelection.FirstOrDefault() as FlyoutPageItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));

                switch (item.Title)
                {
                    case "Student Dashboard":
                        Detail = new NavigationPage(new StudentDashMain());
                        break;

                    case "Coordinator Dashboard":
                        Detail = new NavigationPage(new CoordinatorDashboard());
                        break;

                    case "Coordinator Reports":
                        Detail = new NavigationPage(new CoordinatorReportsDashboard());
                        break;

                    case "User Profile":
                        Detail = new NavigationPage(new UserProfile());
                        break;
                }

                if (!((IFlyoutPageController)this).ShouldShowSplitMode)
                    IsPresented = false;
            }
        }
    }
}