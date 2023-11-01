using System;
using ClinicalCoordinationApplication.Model;

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
        BindingContext = MauiProgram.BusinessLogic;
    }

    public void CreateAnAccount(object sender, EventArgs e)
    {
        Navigation.PushAsync(new CreateAnAccount(this.userType));
    }

    public void StudentSignInClicked(object sender, EventArgs e)
    {
        string email = EmailENT.Text;
        string password = PasswordEnt.Text;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            // Display an error message for empty fields.
            DisplayAlert("Error", "Please enter both email and password.", "OK");
        }
        else
        {
            SignInError validSignIn = MauiProgram.BusinessLogic.SignIn(email, password);

            if (validSignIn == SignInError.InvalidEmailOrPassword)
            {
                // Display an error message for invalid email or password.
                DisplayAlert("Error", "Invalid email or password.", "OK");
            }
            else
            {
                // Successful sign-in logic here
                // ...

                Navigation.PushAsync(new StudentDashMain());

                // Clear the input fields.
                EmailENT.Text = "";
                PasswordEnt.Text = "";
            }
        }
    }

    public void CoordinatorSignInClicked(object sender, EventArgs e)
    {
        string email = EmailENT.Text;
        string password = PasswordEnt.Text;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            // Display an error message for empty fields.
            DisplayAlert("Error", "Please enter both email and password.", "OK");
        }
        else
        {
            SignInError validSignIn = MauiProgram.BusinessLogic.SignIn(email, password);

            if (validSignIn == SignInError.InvalidEmailOrPassword)
            {
                // Display an error message for invalid email or password.
                DisplayAlert("Error", "Invalid email or password.", "OK");
            }
            else
            {
                // Successful sign-in logic here
                // ...

                // Clear the input fields.
                EmailENT.Text = "";
                PasswordEnt.Text = "";
            }
        }
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