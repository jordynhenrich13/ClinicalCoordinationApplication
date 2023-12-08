using ClinicalCoordinationApplication.Model;

namespace ClinicalCoordinationApplication;

public partial class CreateAnAccount : ContentPage
{
    private static string userType = "Student";

	public CreateAnAccount()
	{
        InitializeComponent();
    }
    public void SignInHere_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
    private void CreateAccount_Clicked(object sender, EventArgs e)
    {
        CreateAccountError result = MauiProgram.BusinessLogic.CreateStudentAccount(EmailENT.Text, PasswordEnt.Text, first_nameENT.Text, last_nameENT.Text);
        
        if (result == CreateAccountError.NoError)
        {
            // Account creation was successful. Now, navigate to the sign-in page.
            // You should implement the navigation logic here. The exact method
            // may depend on the framework you are using (e.g., Xamarin.Forms, Maui).

            Application.Current.MainPage = new NavigationPage(new SignIn());
        }
        else if (result == CreateAccountError.InvalidFirstName)
        {
            DisplayAlert("Error", "First name cannot be longer than 50 characters.", "OK");
        }
        else if (result == CreateAccountError.InvalidLastName)
        {
            DisplayAlert("Error", "Last name cannot be longer than 50 characters.", "OK");
        }
        else if (result == CreateAccountError.InvalidEmail)
        {
            DisplayAlert("Error", "Email must be an UWO email address (@uwosh.edu).", "OK");
        }
        else if (result == CreateAccountError.EmailAlreadyUsed)
        {
            DisplayAlert("Error", "Email address has already been used.", "OK");
        }
        else if (result == CreateAccountError.InvalidPassword)
        {
            DisplayAlert("Error", "Password must be between 8 and 50 characters.", "OK");
        }
    }

}