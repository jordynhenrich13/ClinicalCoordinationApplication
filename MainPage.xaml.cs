namespace ClinicalCoordinationApplication
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private void openStudentView(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignIn("student"));
        }

        private void openCoordinatorView(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignIn("coordinator"));
        }
    }
}