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
        //MauiProgram.BusinessLogic.DeleteProfile(); //DEBUGGING PURPOSES
        Navigation.PushAsync(new CreateAnAccount());
    }
    public void SignInClicked(object sender, EventArgs e)
    {
        string email = EmailENT.Text;
        string password = PasswordEnt.Text;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            // Display an error message for empty fields.
            DisplayAlert("Error", "Please enter email and password.", "OK");
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
                // Navigates to AppShell (Allows for Flyout Menu!)
                Application.Current.MainPage = new AppShell();

                // Clear the input fields.
                EmailENT.Text = "";
                PasswordEnt.Text = "";
            }
        }
    }


    public void SkipSignIn(object sender, EventArgs e)
	{
        // Navigates to AppShell (Allows for Flyout Menu!)
        Application.Current.MainPage = new AppShell();
    }
}