using ClinicalCoordinationApplication.Model;
using Microsoft.Maui.ApplicationModel.Communication;

namespace ClinicalCoordinationApplication
{
    public partial class AppShell : Shell
    {
        Account obj = new Account(null, null, null);
        BusinessLogic bl;
        string userType = "";
        public AppShell()
        {
            InitializeComponent();

            bl = new();

            userType = bl.GetUserType();

            // Displays the correct dashboard based on user type
            if (userType == "Coordinator" || userType == "Director")
            {
                dynamicShellContent.ContentTemplate = new DataTemplate(typeof(CoordinatorDashboard));
            }
            else
            {
                dynamicShellContent.ContentTemplate = new DataTemplate(typeof(StudentDashMain));

            }


            // Displays the relavent flyout menu items based on user type
            if (userType == "Student")
            {
                // No additional items needed in the flyout menu (at the moment)
            }

            if (userType == "Coordinator")
            {
                Items.Add(new AddCoordinator());
                Items.Add(new CoordinatorReportsDashboard());
            }

            if (userType == "Director")
            {
                Items.Add(new AddCoordinator());
                Items.Add(new CoordinatorReportsDashboard());
                Items.Add(new DirectorReportsDashboard());
            }

            if (string.IsNullOrEmpty(userType))
            {
                Items.Add(new SignIn());
            }
        }
    }
}