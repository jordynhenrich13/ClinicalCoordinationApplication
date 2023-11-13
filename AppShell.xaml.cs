using ClinicalCoordinationApplication.Model;

namespace ClinicalCoordinationApplication
{
    public partial class AppShell : Shell
    {
        string userType = "";
        BusinessLogic bl;

        public AppShell()
        {
            InitializeComponent();

            bl = new();

            userType = bl.GetUserType();

            // Displays the correct dashboard based on user type
            if (userType == "Coordinator" || userType == "Director")
            {
                dynamicShellContent.ContentTemplate = new DataTemplate(typeof(CoordinatorDashboard));
            } else
            {
                dynamicShellContent.ContentTemplate = new DataTemplate(typeof(StudentDashMain));
            }


            // Displays the relavent flyout menu items based on user type
            if (userType == "Student" || string.IsNullOrEmpty(userType))
            {
                Current.Items.Add(new StudentDashMain());  
            }

            if (userType == "Coordinator")
            {
                Items.Add(new CoordinatorReportsDashboard());
            }

            if (userType == "Director")
            {
                Items.Add(new CoordinatorReportsDashboard());
                Items.Add(new DirectorReportsDashboard());
            }
        }
    }
}