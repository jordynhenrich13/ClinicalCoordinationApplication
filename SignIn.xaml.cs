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

    public void SignInClicked(object sender, EventArgs e)
    {
        
       SignInError validSignIn =  MauiProgram.BusinessLogic.SignIn(EmailENT.ToString(), PasswordEnt.ToString());


        //Navigation.PushAsync(new StudentDashMain());
        Page studentDashMain = new StudentDashMain();
        Application.Current.MainPage = new NavigationPage(studentDashMain);

        // Clear the input fields.
        EmailENT.Text = "";
        PasswordEnt.Text = "";

    }


    public void SkipSignIn(object sender, EventArgs e)
    {
        if (this.userType == "coordinator")
        {
            ((App)Application.Current).MainPage = new CoordinatorDashboard();
        }
        else
        {
            ((App)Application.Current).MainPage = new StudentDashMain();
        }
    }
}