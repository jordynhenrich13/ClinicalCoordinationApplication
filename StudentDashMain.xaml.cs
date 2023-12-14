using ClinicalCoordinationApplication.Model;

namespace ClinicalCoordinationApplication;

public partial class StudentDashMain : ContentPage
{
    BusinessLogic businessLogic = new BusinessLogic();
    Account obja = new Account(null, null, null);
    BusinessLogic bl;
    public StudentDashMain()
    {
        InitializeComponent();
        bl = new();
       // obja = bl.GetUserType();
        // BindingContext = new Clinical("henrij13@uwosh.edu");
        Clinical obj = MauiProgram.BusinessLogic.GetCLinicalInfo(obja.Email);
        if (obj != null)
        {
            _Lable.Text = obj.ClinicalName;
           // _Lable1.Text = obj.clinicalsite;
          //  _Lable2.Text = obj.hoursworked.ToString();
           // _Lable3.Text = obj.name;
            //_Lable4.Text = obj.total.ToString();
            _Email.Text = obja.Email;
        }
    }

    // Add this code within the StudentDashMain class in StudentDashMain.xaml.cs
    public void UpdateClinical_Clicked(object sender, EventArgs e)
    {
        businessLogic.UpdateCurrentClinical(obja.Email, _Lable.Text);
    }
    private void NavigateToStudentClinical1(object sender, EventArgs e)
    {
        Navigation.PushAsync(new StudentClinical1());
    }

    private void NavigateToStudentClinical2(object sender, EventArgs e)
    {
        Navigation.PushAsync(new StudentClinical2());
    }

    // Add this code within the StudentDashMain class in StudentDashMain.xaml.cs
    private void NavigateToStudentClinical3(object sender, EventArgs e)
    {
        Navigation.PushAsync(new StudentClinical3());
    }

    private void NavigateToStudentClinical4(object sender, EventArgs e)
    {
        Navigation.PushAsync(new StudentClinical4());
    }

    // Add this code within the StudentDashMain class in StudentDashMain.xaml.cs
    private void NavigateToStudentClinical5(object sender, EventArgs e)
    {
        Navigation.PushAsync(new StudentClinical5());
    }
    // Add this code within the StudentDashMain class in StudentDashMain.xaml.cs
    private void NavigateToStudentClinical6(object sender, EventArgs e)
    {
        Navigation.PushAsync(new StudentClinical5());
    }


    public void ViewClinicalHours_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new StudentClinicalHours());
    }
}