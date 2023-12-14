using ClinicalCoordinationApplication.Model;

namespace ClinicalCoordinationApplication;

public partial class StudentClinical1 : ContentPage
{
    public DateTime SelectedDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Comments { get; set; }
    Account obja = new Account(null, null, null);
    BusinessLogic businessLogic = new BusinessLogic();

    public StudentClinical1()
    {
        SelectedDate = DateTime.Now;
        StartTime = TimeSpan.FromHours(DateTime.Now.Hour);
        EndTime = TimeSpan.FromHours(DateTime.Now.Hour + 1);
        Comments = string.Empty;
        obja = businessLogic.GetUserAccount();
        InitializeComponent();
    }

    private void ConfirmHours_Clicked(object sender, EventArgs e)
    {

        businessLogic.AddWorkedHours(clinicalEditor.Text, SelectedDate, startTimePicker.Time, endTimePicker.Time, commentsEditor.Text, obja.Email, DateTime.Now);
        Navigation.PushAsync(new StudentDashSuccess());
    }
}