using ClinicalCoordinationApplication.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClinicalCoordinationApplication;

public partial class StudentDashConfirmHours : ContentPage
{
    BusinessLogic businessLogic = new BusinessLogic();
    Account obja = new Account(null, null, null);
    BusinessLogic bl;
    public StudentDashConfirmHours()
    {
        InitializeComponent();
        bl = new();
        obja = bl.GetUserAccount();
        Clinical obj = MauiProgram.BusinessLogic.GetLatestCLinicalSubmission(obja.Email);
        if (obj != null)
        {
            ClinicalName.Text = obj.clinicalName;
            HoursWorked.Text = obj.hoursworked.ToString();
            DateWorked.Text = obj.dateWorked;
            Notes.Text = obj.notes;

        }


    }
}
