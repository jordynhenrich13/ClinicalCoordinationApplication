using ClinicalCoordinationApplication.Model;

namespace ClinicalCoordinationApplication;

public partial class StudentDashMain : ContentPage
{
	BusinessLogic businessLogic = new BusinessLogic();
	public StudentDashMain()
	{
		InitializeComponent();

		this.BindingContext = new Clinical(businessLogic.LoggedInUserName);
	}

	public void LogClinicalHours_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new StudentDashHourLog());
	}
}