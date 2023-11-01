namespace ClinicalCoordinationApplication
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private void OpenStudentView(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignIn());
        }

        private void OpenCoordinatorView(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignIn());
        }

        private void OpenCoordinatorReportsView(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CoordinatorReportsDashboard());
        }

        private void OpenDirectorReportsView(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DirectorReportsDashboard());
        }
    }
}