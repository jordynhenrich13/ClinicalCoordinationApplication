using ClinicalCoordinationApplication.Model;

namespace ClinicalCoordinationApplication;

public partial class CreateAnAccount : ContentPage
{
    private static string userType;

	public CreateAnAccount()
	{
        InitializeComponent();
	}

	public void Student_Button_Clicked()
	{
		MauiProgram.BusinessLogic.CreateStudentAccount(EmailENT.ToString(), PasswordEnt.ToString(), first_nameENT.ToString(), last_nameENT.ToString());

		EmailENT.Text = "";
		PasswordEnt.Text = "";
		first_nameENT.Text = "";
		last_nameENT.Text = "";

	}

    public void OnUserTypeButtonChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender == CoordinatorRadioButton && e.Value)
        {
            userType = "Coordinator";
        }
        else if (sender == StudentRadioButton && e.Value)
        {
            userType = "Student";
        }
    }

    public void SignInHere_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new SignIn());
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        string firstName = first_nameENT.Text;
        string lastName = last_nameENT.Text;
        string email = EmailENT.Text;
        string password = PasswordEnt.Text;

        Database database = new Database();

        CreateAccountError result = CreateAccountError.NoError;

        if (userType != null)
        {
            if (userType == "Student")
            {
                result = database.CreateStudentAccount(email, password, firstName, lastName);
            }
            else if (userType == "Coordinator")
            {
                result = database.CreateCoordinatorAccount(email, password, firstName, lastName);
            }
        }

        if (result == CreateAccountError.NoError)
        {
            // Account creation was successful. Now, navigate to the sign-in page.
            // You should implement the navigation logic here. The exact method
            // may depend on the framework you are using (e.g., Xamarin.Forms, Maui).

            Application.Current.MainPage = new NavigationPage(new SignIn());
        }
        else
        {
            // Handle the error and provide feedback to the user.
            DisplayAlert("Error", "Could not create an account.", "OK");
        }
    }

}