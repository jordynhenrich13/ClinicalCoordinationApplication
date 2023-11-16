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
        private string email;
        private string phoneNumber;

        private string address;
        //private string password;

        public Coordinator(string firstName, string lastName, string employeeID, string email, string phoneNumber, string address)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.employeeID = employeeID;
            this.email = email;
            this.phoneNumber = phoneNumber;
            this.address = address;
        }

        public Coordinator(string firstName, string lastName, string email)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            //this.password = password;
        }

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

        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged(nameof(email));
            }
        }

        //public string Password
        //{
        //    get { return password; }
        //    set
        //    {
        //        password = value;
        //        OnPropertyChanged(nameof(password));
        //    }
        //}

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
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}

