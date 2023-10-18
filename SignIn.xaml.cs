using System;

namespace ClinicalCoordinationApplication;

public partial class SignIn : ContentPage
{
    // Student OR Coordinator
    private string userType; 

	public SignIn(string userType)
	{
		InitializeComponent();
		// Initialize type of user based on MainPage button selection
		this.userType = userType;
	}

	public void CreateAnAccount(object sender, EventArgs e)
	{
		Navigation.PushAsync(new CreateAnAccount(this.userType));
	}

	public void SkipSignIn(object sender, EventArgs e)
	{
		if (this.userType == "coordinator")
		{
			((App)Application.Current).MainPage = new CoordinatorDashboard();
		} else
		{
            ((App)Application.Current).MainPage = new StudentDashMain();
        }
	}
}