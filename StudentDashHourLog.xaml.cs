using ClinicalCoordinationApplication.Model;

namespace ClinicalCoordinationApplication;

public partial class StudentDashHourLog : ContentPage
{
        public DateTime SelectedDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Comments { get; set; }

        BusinessLogic businessLogic = new BusinessLogic();

        public StudentDashHourLog()
        {
            SelectedDate = DateTime.Now;
            StartTime = TimeSpan.FromHours(DateTime.Now.Hour);
            EndTime = TimeSpan.FromHours(DateTime.Now.Hour + 1);
            //Comments = string.Empty;

            InitializeComponent();
            BindingContext = this;
        }

    private void ConfirmHours_Clicked(object sender, EventArgs e)
    {
        
        businessLogic.AddWorkedHours(clinicalPicker.SelectedItem?.ToString(), SelectedDate, startTimePicker.Time, endTimePicker.Time, commentsEditor.Text);
        Navigation.PushAsync(new StudentDashSuccess());
    }
}