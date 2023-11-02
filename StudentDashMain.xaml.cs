namespace ClinicalCoordinationApplication;

public partial class StudentDashMain : ContentPage
{
	public StudentDashMain()
	{
		InitializeComponent();
	}

	public void LogClinicalHours_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new StudentDashHourLog());
	}
}