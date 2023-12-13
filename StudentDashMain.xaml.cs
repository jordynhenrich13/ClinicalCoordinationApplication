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
        obja = bl.GetUserType();
        // BindingContext = new Clinical("henrij13@uwosh.edu");
        Clinical obj = MauiProgram.BusinessLogic.GetCLinicalInfo(obja.Email);
        if (obj != null)
        {
            _Lable.Text = obj.ClinicalName;
           // _Lable1.Text = obj.clinicalsite;
            _Lable2.Text = obj.hoursworked.ToString();
           // _Lable3.Text = obj.name;
            //_Lable4.Text = obj.total.ToString();
            _Email.Text = obja.Email;
        }
    }

    public void LogClinicalHours_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new StudentDashHourLog());
    }

    public void ViewClinicalHours_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new StudentDashConfirmHours());
    }
}