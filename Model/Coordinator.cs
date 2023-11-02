using System;
//using Android.Locations;
using System.Net.Mail;
//using Android.Media;
using System.ComponentModel;

namespace ClinicalCoordinationApplication.Model
{
    [Serializable()]
    public class Coordinator : INotifyPropertyChanged
    {
        private string firstName;
        private string lastName;

        private string employeeID;
        private string emailAddress;
        private string phoneNumber;

        private string address;
        private string password;

        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged(nameof(firstName));
            }
        }
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged(nameof(lastName));
            }
        }

        public string EmployeeID
        {
            get { return employeeID; }
            set
            {
                employeeID = value;
                OnPropertyChanged(nameof(employeeID));
            }
        }

        public string EmailAddress
        {
            get { return emailAddress; }
            set
            {
                emailAddress = value;
                OnPropertyChanged(nameof(emailAddress));
            }
        }
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                phoneNumber = value;
                OnPropertyChanged(nameof(phoneNumber));
            }
        }

        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged(nameof(address));
            }
        }


        public Coordinator(string firstName, string lastName, string employeeID,  string emailAddress, string phoneNumber, string address)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.employeeID = employeeID;
            this.emailAddress = emailAddress;
            this.phoneNumber = phoneNumber;
            this.address = address;
        }

        public Coordinator(string firstName, string lastName, string emailAddress, string password)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.emailAddress = emailAddress;
            this.password = password;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}

