using System;
using ClinicalCoordinationApplication.Model;

namespace ClinicalCoordinationApplication;

public partial class SignIn : ContentPage
{
    // Student OR Coordinator
    private readonly string userType;

    public SignIn()
    {
        InitializeComponent();
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
            SignInError validSignIn = MauiProgram.BusinessLogic.StudentSignIn(email, password);

            if (validSignIn == SignInError.InvalidEmailOrPassword)
            {
                // Display an error message for invalid email or password.
                DisplayAlert("Error", "Invalid email or password.", "OK");
            }
            else
            {
                // Replaces MainPage at the root level with the FlyoutMenuPage
                ((App)Application.Current).MainPage = new MainPage("Student");

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
            SignInError validSignIn = MauiProgram.BusinessLogic.CoordinatorSignIn(email, password);

            if (validSignIn == SignInError.InvalidEmailOrPassword)
            {
                // Display an error message for invalid email or password.
                DisplayAlert("Error", "Invalid email or password.", "OK");
            }
            else
            {
                // Replaces MainPage at the root level with the FlyoutMenuPage
                ((App)Application.Current).MainPage = new MainPage("Coordinator");

                // Clear the input fields.
                EmailENT.Text = "";
                PasswordEnt.Text = "";
            }
        }
    }


    public void SkipSignIn(object sender, EventArgs e)
	{
        // Replaces MainPage at the root level with the FlyoutMenuPage
        ((App)Application.Current).MainPage = new MainPage("Student");
    }
}