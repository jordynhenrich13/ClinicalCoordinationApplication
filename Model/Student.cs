using System;
//using Android.Locations;
using System.Net.Mail;
//using Android.Media;
using System.ComponentModel;

namespace ClinicalCoordinationApplication.Model
{
    [Serializable()]
    public class Student : INotifyPropertyChanged
	{
        private string firstName;
        private string lastName;

        private string studentID;
        private string graduationDate;
        private string emailAddress;
        private string phoneNumber;

        private string address;
        private List<Clinical> clinicals;
        //private string password;

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
        // For searching for the student in coordinator dashboard
        public string SearchName
        {
            get { return $"{FirstName} {LastName}"; }
        }

        public string StudentID {
            get { return studentID; }
            set
            {
                studentID = value;
                OnPropertyChanged(nameof(studentID));
            }
        }
        public string GraduationDate {
            get { return graduationDate; }
            set
            {
                graduationDate = value;
                OnPropertyChanged(nameof(graduationDate));
            }
        }
        public string EmailAddress {
            get { return emailAddress; }
            set
            {
                emailAddress = value;
                OnPropertyChanged(nameof(emailAddress));
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


        public string PhoneNumber {
            get { return phoneNumber; }
            set
            {
                phoneNumber = value;
                OnPropertyChanged(nameof(phoneNumber));
            }
        }

        public string Address {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged(nameof(address));
            }
        }

        public List<Clinical> Clinicals {
            get { return clinicals; }
            set
            {
                clinicals = value;
                OnPropertyChanged(nameof(clinicals));
            }
        }

        public Student(string firstName, string lastName, string studentID, string graduationDate, string emailAddress, string phoneNumber, string address, List<Clinical> clinicals)
		{
            this.firstName = firstName;
            this.lastName = lastName;
            this.studentID = studentID;
            this.graduationDate = graduationDate;
            this.emailAddress = emailAddress;
            this.phoneNumber = phoneNumber;
            this.address = address;
            this.clinicals = clinicals;
		}

        public Student(string firstName, string lastName, string emailAddress)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.emailAddress= emailAddress;
            //this.password = password;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}

