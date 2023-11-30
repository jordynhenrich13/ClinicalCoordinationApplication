using ClinicalCoordinationApplication.Model;

namespace ClinicalCoordinationApplication;

public partial class StudentDashMain : ContentPage
{
	BusinessLogic businessLogic = new BusinessLogic();
	public StudentDashMain()
	{
		InitializeComponent();

		BindingContext = new Clinical("henrij13@uwosh.edu");
	}

	public void LogClinicalHours_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new StudentDashHourLog());
	}
}