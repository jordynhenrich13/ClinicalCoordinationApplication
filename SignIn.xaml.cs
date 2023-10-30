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
		{ // This should be a navigation, because if they want to sign in, they need the option to go back. 
			// The way it was, set the coordinator dashboard to the main page, with no way of signing in.
			Navigation.PushAsync(new CoordinatorDashboard());
		} else
		{
            ((App)Application.Current).MainPage = new StudentDashMain();
        }
	}
}