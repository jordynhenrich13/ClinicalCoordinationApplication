namespace ClinicalCoordinationApplication;

public partial class CreateAnAccount : ContentPage
{
	private string userType;

	public CreateAnAccount(string userType)
	{
		InitializeComponent();
		this.userType = userType;
	}
}