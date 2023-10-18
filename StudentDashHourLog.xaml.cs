namespace ClinicalCoordinationApplication;

public partial class StudentDashHourLog : ContentPage
{
        public DateTime SelectedDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Comments { get; set; }

        public StudentDashHourLog()
        {
            SelectedDate = DateTime.Now;
            StartTime = TimeSpan.FromHours(DateTime.Now.Hour);
            EndTime = TimeSpan.FromHours(DateTime.Now.Hour + 1);
            Comments = string.Empty;

            InitializeComponent();
            BindingContext = this;
        }
    }