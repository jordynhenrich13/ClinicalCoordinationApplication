using System.ComponentModel;
namespace ClinicalCoordinationApplication.Model.Reports
{
    public class ReportSubmission : INotifyPropertyChanged
    {
        /// <summary>
        /// See constructor for more details
        /// </summary>
        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set
            {
                if (fileName != value)
                {
                    fileName = value;
                    OnPropertyChanged(nameof(FileName));
                }
            }
        }

        /// <summary>
        /// See constructor for more details
        /// </summary>
        private DateTime submissionDate;
        public DateTime SubmissionDate
        {
            get { return submissionDate; }
            set
            {
                if (submissionDate != value)
                {
                    submissionDate = value;
                    OnPropertyChanged(nameof(SubmissionDate));
                }
            }
        }

        /// <summary>
        /// See constructor for more details
        /// </summary>
        private Stream reportStream;
        public Stream ReportStream
        {
            get { return reportStream; }
            set
            {
                if (reportStream != value)
                {
                    reportStream = value;
                    OnPropertyChanged(nameof(ReportStream));
                }
            }
        }

        /// <summary>
        /// See constructor for more details
        /// </summary>
        private string uploadedBy;
        public string UploadedBy
        {
            get { return uploadedBy; }
            set
            {
                if (uploadedBy != value)
                {
                    uploadedBy = value;
                    OnPropertyChanged(nameof(UploadedBy));
                }
            }
        }

        /// <summary>
        /// See constructor for more details
        /// </summary>
        private string reportName;
        public string ReportName
        {
            get { return reportName; }
            set
            {
                if (reportName != value)
                {
                    reportName = value;
                    OnPropertyChanged(nameof(ReportName));
                }
            }
        }

        /// <summary>
        /// Creates an object representing a completed report that is submitted
        /// to the nursing director by the nursing coordinators. Intended to be
        /// displayed in a CollectionView and stored in an ObservableCollection
        /// </summary>
        /// <param name="fileName">Name of the uploaded file (the completed report)</param>
        /// <param name="submissionDate">The date/time that the report was submitted</param>
        public ReportSubmission(string fileName, Stream reportStream, string uploadedBy, DateTime submissionDate, string reportName)
        {
            FileName = fileName;
            ReportStream = reportStream;
            UploadedBy = uploadedBy;
            SubmissionDate = submissionDate;
            ReportName = reportName;
        }

        /// <summary>
        /// Executes when a ReportItem property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}