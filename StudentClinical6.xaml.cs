namespace ClinicalCoordinationApplication;
using ClinicalCoordinationApplication.Model;

public partial class StudentClinical6 : ContentPage
{
    public DateTime SelectedDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Comments { get; set; }
    Account obja = new Account(null, null, null);
    BusinessLogic businessLogic = new BusinessLogic();

    public StudentClinical6()
    {
        SelectedDate = DateTime.Now;
        StartTime = TimeSpan.FromHours(DateTime.Now.Hour);
        EndTime = TimeSpan.FromHours(DateTime.Now.Hour + 1);
        //Comments = string.Empty;
        obja = businessLogic.GetUserType();
        InitializeComponent();
    }

    private void ConfirmHours_Clicked(object sender, EventArgs e)
    {

        businessLogic.AddWorkedHours(clinicalEditor.Text, SelectedDate, startTimePicker.Time, endTimePicker.Time, commentsEditor.Text, obja.Email, DateTime.Now);
        Navigation.PushAsync(new StudentDashSuccess());
    }
}