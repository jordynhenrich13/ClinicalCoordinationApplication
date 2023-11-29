using System;
using System.ComponentModel;

namespace ClinicalCoordinationApplication
{
    [Serializable()]
    public class Clinical : INotifyPropertyChanged
    {
        private string clinicalName;
        private int clinicalNumber;
        private string description;
        private int requiredHours;
        private int durationInWeeks;
        private Clinic clinic;
        private bool hasPreceptor;
        private Preceptor preceptor;
        private string studentEmail;
        private string currentClinical;
        private string clinicalStatus;

        public int ClinicalNumber
        {
            get { return clinicalNumber; }
            set
            {
                clinicalNumber = value;
                OnPropertyChanged(nameof(clinicalNumber));
            }
        }

        public string ClinicalStatus
        {
            get { return clinicalStatus; }
            set
            {
                clinicalStatus = value;
                OnPropertyChanged(nameof(clinicalStatus));
            }
        }

        public string ClinicalName
        {
            get { return clinicalName; }
            set
            {
                clinicalName = value;
                OnPropertyChanged(nameof(clinicalName));
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(nameof(description));
            }
        }

        public int RequiredHours
        {
            get { return requiredHours; }
            set
            {
                requiredHours = value;
                OnPropertyChanged(nameof(requiredHours));
            }
        }

        public int DurationInWeeks
        {
            get { return durationInWeeks; }
            set
            {
                durationInWeeks = value;
                OnPropertyChanged(nameof(durationInWeeks));
            }
        }

        public Clinic Clinic
        {
            get { return clinic; }
            set
            {
                clinic = value;
                OnPropertyChanged(nameof(clinic));
            }
        }

        public bool HasPreceptor
        {
            get { return hasPreceptor; }
            set
            {
                hasPreceptor = value;
                OnPropertyChanged(nameof(hasPreceptor));
            }
        }

        public Preceptor Preceptor
        {
            get { return preceptor; }
            set
            {
                preceptor = value;
                OnPropertyChanged(nameof(preceptor));
            }
        }

        public string StudentEmail
        {
            get { return studentEmail; }
            set
            {
                studentEmail = value;
                OnPropertyChanged(nameof(studentEmail));
            }
        }

        public Clinical(string clinicalName, int clinicalNumber, string description, int requiredHours, int durationInWeeks, Clinic clinic, bool hasPreceptor, Preceptor preceptor)
        {
            this.clinicalName = clinicalName;
            this.clinicalNumber = clinicalNumber;
            this.description = description;
            this.requiredHours = requiredHours;
            this.durationInWeeks = durationInWeeks;
            this.clinic = clinic;
            this.hasPreceptor = hasPreceptor;
            this.preceptor = preceptor;
        }

        public Clinical(String StudentEmail)
        {
            this.studentEmail = StudentEmail;
            clinicalStatus = "Not Started";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

