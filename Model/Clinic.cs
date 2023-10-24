using System;
using System.ComponentModel;

namespace ClinicalCoordinationApplication
{
    [Serializable()]
    public class Clinic : INotifyPropertyChanged
    {
        private string clinicName;
        private string clinicAddress;
        private string clinicHealthSystem;
        private bool hasContract;
        private bool previouslyAssigned;

        public string ClinicName
        {
            get { return clinicName; }
            set
            {
                clinicName = value;
                OnPropertyChanged(nameof(clinicName));
            }
        }

        public string ClinicAddress
        {
            get { return clinicAddress; }
            set
            {
                clinicAddress = value;
                OnPropertyChanged(nameof(clinicAddress));
            }
        }

        public String ClinicHealthSystem
        {
            get { return clinicHealthSystem; }
            set
            {
                clinicHealthSystem = value;
                OnPropertyChanged(nameof(clinicHealthSystem));
            }
        }

        public bool HasContract
        {
            get { return hasContract; }
            set
            {
                hasContract = value;
                OnPropertyChanged(nameof(hasContract));
            }
        }

        public bool PreviouslyAssigned
        {
            get { return previouslyAssigned; }
            set
            {
                previouslyAssigned = value;
                OnPropertyChanged(nameof(previouslyAssigned));
            }
        }

        public Clinic(string clinicName, string clinicAddress, string clinicHealthSystem, bool hasContract, bool previouslyAssigned)
        {
            this.clinicName = clinicName;
            this.clinicAddress = clinicAddress;
            this.clinicHealthSystem = clinicHealthSystem;
            this.hasContract = hasContract;
            this.previouslyAssigned = previouslyAssigned;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

