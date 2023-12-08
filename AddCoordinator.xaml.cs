using ClinicalCoordinationApplication.Model;

namespace ClinicalCoordinationApplication;

public partial class AddCoordinator : ContentPage
{
	public AddCoordinator()
	{
		InitializeComponent();
	}
	public void AddCoordinatorClicked(object sender, EventArgs e)
	{
		string email = EmailENT.Text;

		if (string.IsNullOrWhiteSpace(email))
		{
			// Display an error message for empty fields.
			DisplayAlert("Error", "Please enter email.", "OK");
		}
		else
		{
            AddCoordinatorError result = MauiProgram.BusinessLogic.AddCoordinator(email);

            if (result == AddCoordinatorError.NoError)
            {
                DisplayAlert("Success", "Coordinator added.", "OK");
            }
            else
            {
                DisplayAlert("Error", "Please enter a valid email.", "OK");
            }
        }
	}
}