using ClinicalCoordinationApplication.Model;

namespace ClinicalCoordinationApplication;

public partial class EditAccount : ContentPage
{
	public EditAccount()
	{
		InitializeComponent();
	}

    public void EditAccount_Clicked(object sender, EventArgs e)
    {
		if (string.IsNullOrWhiteSpace(EmailENT.Text) && string.IsNullOrWhiteSpace(first_nameENT.Text) && string.IsNullOrWhiteSpace(last_nameENT.Text))
		{
            DisplayAlert("Error", "Make sure at least one field has an entry.", "OK");
        }
		else
		{
            EditAccountError result = MauiProgram.BusinessLogic.EditAccount(EmailENT.Text, first_nameENT.Text, last_nameENT.Text);
            if (result == EditAccountError.NoError)
            {
                DisplayAlert("Success", "Account has been edited.", "OK");
            }
            else if (result == EditAccountError.InvalidFirstName)
            {
                DisplayAlert("Error", "First name cannot be longer than 50 characters.", "OK");
            }
            else if (result == EditAccountError.InvalidLastName)
            {
                DisplayAlert("Error", "Last name cannot be longer than 50 characters.", "OK");
            }
            else if (result == EditAccountError.InvalidEmail)
            {
                DisplayAlert("Error", "Email must be an UWO email address (@uwosh.edu).", "OK");
            }
            else if (result == EditAccountError.EmailAlreadyUsed)
            {
                DisplayAlert("Error", "Email address has already been used.", "OK");
            }
        }
    }
}