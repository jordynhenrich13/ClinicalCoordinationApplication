using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.VisualBasic;

namespace ClinicalCoordinationApplication.Model.Reports
{
    public class ReportItem : INotifyPropertyChanged
    {
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
        /* TODO: Change to DateTime later & add validation */
        private DateTime dueDate;
        public DateTime DueDate
        {
            get { return dueDate; }
            set
            {
                if (dueDate != value)
                {
                    dueDate = value;
                    OnPropertyChanged(nameof(DueDate));
                }
            }
        }

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
        /// </summary>
        /// See constructor for more details
        private DateTime uploadDate;
        public DateTime UploadDate
        {
            get { return uploadDate; }
            set
            {
                if (uploadDate != value)
                {
                    uploadDate = value;
                    OnPropertyChanged(nameof(UploadDate));
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
        private string[] coordinatorEmails;
        public string[] CoordinatorEmails
        {
            get { return coordinatorEmails; }
            set
            {
                if (coordinatorEmails != value)
                {
                    coordinatorEmails = value;
                    OnPropertyChanged(nameof(CoordinatorEmails));
                }
            }
        }

        /// <summary>
        /// See constructor for more details
        /// </summary>
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

        /// <summary>
        /// Creates an object representing a report that is sent from the nursing
        /// director of an incomplete report that needs to be finished by the
        /// coordinators and returned by a specified date. Intended to be displayed in a
        /// CollectionView and stored in an ObservableCollection
        /// </summary>
        /// <param name="reportName">Human-readable name of the report</param>
        /// <param name="uploadedBy">Name of individual who uploaded the file</param>
        /// <param name="dueDate">Date/time that the completed submission is due by</param>
        /// <param name="fileName">Name of the uploaded file (the report to
        ///                        be completed and submitted)</param>
        /// <param name="uploadDate">The date/time that the report was uploaded</param>
        /// <param name="reportStream">The stream contents of the report</param>
        /// <param name="coordinatorEmails">Email addressess of coordinators to send report to</param>
        public ReportItem(string reportName,
                          string fileName,
                          Stream reportStream,
                          string uploadedBy,
                          DateTime uploadDate,
                          DateTime dueDate,
                          string[] coordinatorEmails,
                          ObservableCollection<ReportSubmission> submissions
            )
        {
            ReportName = reportName;
            FileName = fileName;
            ReportStream = reportStream;
            UploadedBy = uploadedBy;
            UploadDate = uploadDate;
            DueDate = dueDate;
            CoordinatorEmails = coordinatorEmails;
            Submissions = submissions;
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