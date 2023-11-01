using ClinicalCoordinationApplication.Model;

namespace ClinicalCoordinationApplication;

public partial class CreateAnAccount : ContentPage
{
	private string userType;

	public CreateAnAccount(string userType)
	{
		InitializeComponent();
		this.userType = userType;
	}

	public void Student_Button_Clicked()
	{
		MauiProgram.BusinessLogic.CreateStudentAccount(EmailENT.ToString(), PasswordEnt.ToString(), first_nameENT.ToString(), last_nameENT.ToString());

		EmailENT.Text = "";
		PasswordEnt.Text = "";
		first_nameENT.Text = "";
		last_nameENT.Text = "";

	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        string firstName = first_nameENT.Text;
        string lastName = last_nameENT.Text;
        string email = EmailENT.Text;
        string password = PasswordEnt.Text;

        Database database = new Database();

        CreateAccountError result = database.CreateStudentAccount(email, password, firstName, lastName);
        Page signInPage = new SignIn("Student");

        if (result == CreateAccountError.NoError)
        {
            // Account creation was successful. Now, navigate to the sign-in page.
            // You should implement the navigation logic here. The exact method
            // may depend on the framework you are using (e.g., Xamarin.Forms, Maui).

            Application.Current.MainPage = new NavigationPage(signInPage);
        }
        else
        {
            // Handle the error and provide feedback to the user.
            DisplayAlert("Error", "Invalid email or password.", "OK");
        }
    }

}