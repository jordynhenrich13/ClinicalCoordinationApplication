using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ClinicalCoordinationApplication.Model.Reports
{
    public class ReportsView : INotifyPropertyChanged
    {
        // Initialize all reports as an Observable Collection
        private ObservableCollection<ReportItem> reportItems;
        public ObservableCollection<ReportItem> ReportItems
        {
            get { return reportItems; }
            set
            {
                if (reportItems != value)
                {
                    reportItems = value;
                    OnPropertyChanged(nameof(ReportItems));
                }
            }
        }

        private ObservableCollection<ReportSubmission> submissions;
        public ObservableCollection<ReportSubmission> Submissions
        {
            get { return submissions; }
            set
            {
                if (submissions != value)
                {
                    submissions = value;
                    OnPropertyChanged(nameof(Submissions));
                }
            }
        }

        public ReportsView(ObservableCollection<ReportItem> reports)
        {
            this.ReportItems = reports;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}