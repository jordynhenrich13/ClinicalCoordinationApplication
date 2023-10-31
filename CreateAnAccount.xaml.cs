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

    }
}