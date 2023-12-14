using System;
using System.ComponentModel;
using System.Net.Mail;

namespace ClinicalCoordinationApplication
{
    [Serializable()]
    // The AssignedPreceptor class represents an entity with information about an assigned preceptor.
    // It implements INotifyPropertyChanged to notify subscribers when property values change.
    public class AssignedPreceptor : INotifyPropertyChanged
    {
        // Private fields to store information about the assigned preceptor.
        private string firstName;
        private string lastName;
        private Clinic clinic;
        private string preceptorDepartment;
        private string email;
        private string phoneNumber;

        // Constructor to initialize an AssignedPreceptor object with various properties.
        public AssignedPreceptor(string firstName, string lastName, Clinic clinic, string preceptorDepartment, string email, string phoneNumber)
        {
            // Assign values to the private fields.
            this.firstName = firstName;
            this.lastName = lastName;
            this.clinic = clinic;
            this.preceptorDepartment = preceptorDepartment;
            this.email = email;
            this.phoneNumber = phoneNumber;
        }

        // Public property for accessing and modifying the FirstName field.
        public string FirstName
        {
            get { return firstName; }
            set
            {
                // Set the value and raise the PropertyChanged event.
                firstName = value;
                OnPropertyChanged(nameof(firstName));
            }
        }

        // Public property for accessing and modifying the LastName field.
        public string LastName
        {
            get { return lastName; }
            set
            {
                // Set the value and raise the PropertyChanged event.
                lastName = value;
                OnPropertyChanged(nameof(lastName));
            }
        }

        // Public property for accessing and modifying the Clinic field.
        public Clinic Clinic
        {
            get { return clinic; }
            set
            {
                // Set the value and raise the PropertyChanged event.
                clinic = value;
                OnPropertyChanged(nameof(clinic));
            }
        }

        // Public property for accessing and modifying the PreceptorDepartment field.
        public string PreceptorDepartment
        {
            get { return preceptorDepartment; }
            set
            {
                // Set the value and raise the PropertyChanged event.
                preceptorDepartment = value;
                OnPropertyChanged(nameof(preceptorDepartment));
            }
        }

        // Public property for accessing and modifying the Email field.
        public string Email
        {
            get { return email; }
            set
            {
                // Set the value and raise the PropertyChanged event.
                email = value;
                OnPropertyChanged(nameof(email));
            }
        }

        // Public property for accessing and modifying the PhoneNumber field.
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                // Set the value and raise the PropertyChanged event.
                phoneNumber = value;
                OnPropertyChanged(nameof(phoneNumber));
            }
        }

        // Event to notify subscribers when a property changes.
        public event PropertyChangedEventHandler PropertyChanged;

        // Method to raise the PropertyChanged event.
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}

