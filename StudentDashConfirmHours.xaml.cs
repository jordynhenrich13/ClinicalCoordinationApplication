namespace ClinicalCoordinationApplication;

public partial class StudentDashConfirmHours : ContentPage
{
	public StudentDashConfirmHours()
	{
		InitializeComponent();
	}

	private void SubmitHours_Clicked(object sender, EventArgs e)
	{
		//call business logic to save in DB
		Navigation.PushAsync(new StudentDashSuccess());
	}
}