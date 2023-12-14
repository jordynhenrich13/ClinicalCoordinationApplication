using ClinicalCoordinationApplication.Model;
using CommunityToolkit.Maui.Storage;
using Microsoft.Maui.ApplicationModel.Communication;

namespace ClinicalCoordinationApplication
{
    public partial class AppShell : Shell
    {
        //Account obj = new Account(null, null, null);
        //BusinessLogic bl;
        //string userType = "";
        public AppShell()
        {
            InitializeComponent();

            //bl = new();

            //obj = bl.GetUserType();
            //userType = obj.Role;

            string userType = Preferences.Get("user_type", "Unknown");

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
                Items.Add(new EditAccount());
                SignIn signInItem = new SignIn();
                signInItem.Title = "Log Out";
                Items.Add(signInItem);
            }

            if (userType == "Coordinator")
            {
                Items.Add(new EditAccount());
                Items.Add(new AddCoordinator());
                Items.Add(new CoordinatorReportsDashboard(FileSaver.Default));
                SignIn signInItem = new SignIn();
                signInItem.Title = "Log Out";
                Items.Add(signInItem);
            }

            if (userType == "Director")
            {
                Items.Add(new EditAccount());
                Items.Add(new AddCoordinator());
                Items.Add(new CoordinatorReportsDashboard(FileSaver.Default));
                Items.Add(new DirectorReportsDashboard(FileSaver.Default));
                SignIn signInItem = new SignIn();
                signInItem.Title = "Log Out";
                Items.Add(signInItem);
            }

            if (string.IsNullOrEmpty(userType))
            {
                Items.Add(new SignIn());
            }
        }
    }
}