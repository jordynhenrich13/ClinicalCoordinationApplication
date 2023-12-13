namespace ClinicalCoordinationApplication;

public partial class StudentDashSuccess : ContentPage
{
	public StudentDashSuccess()
	{
		InitializeComponent();
	}

	private void ReturnToDashboard_Clicked(object sender, EventArgs e)
	{
        Page studentDashMain = new StudentDashMain();
        Application.Current.MainPage = new NavigationPage(studentDashMain);
    }

    private void ViewSubmission_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new StudentDashConfirmHours());
    }
}