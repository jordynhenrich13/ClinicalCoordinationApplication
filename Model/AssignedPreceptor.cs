using System;
using System.ComponentModel;
using System.Net.Mail;

namespace ClinicalCoordinationApplication
{
    [Serializable()]
    public class AssignedPreceptor : INotifyPropertyChanged
    {
        private string firstName;
        private string lastName;
        private Clinic clinic;
        private string preceptorDepartment;
        private string email;
        private string phoneNumber;

        public AssignedPreceptor(string firstName, string lastName, Clinic clinic, string preceptorDepartment, string email, string phoneNumber)
        {
            this.firstName = firstName;
            this.lastName = lastName;

            this.clinic = clinic;
            this.preceptorDepartment = preceptorDepartment;

            this.email = email;
            this.phoneNumber = phoneNumber;

        }

        public string FirstName {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged(nameof(firstName));
            }
        }
        public string LastName {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged(nameof(lastName));
            }
        }

        public Clinic Clinic {
            get { return clinic; }
            set
            {
                clinic = value;
                OnPropertyChanged(nameof(clinic));
            }
        }

        public string PreceptorDepartment {
            get { return preceptorDepartment; }
            set
            {
                preceptorDepartment = value;
                OnPropertyChanged(nameof(preceptorDepartment));
            }
        }

        public string Email {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged(nameof(email));
            }
        }

        public string PhoneNumber {
            get { return phoneNumber; }
            set
            {
                phoneNumber = value;
                OnPropertyChanged(nameof(phoneNumber));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

