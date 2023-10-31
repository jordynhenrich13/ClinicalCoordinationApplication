using System;
using System.Collections.ObjectModel;

namespace ClinicalCoordinationApplication.Model
{
	public class BusinessLogic : IBusinessLogic
	{
        private IDatabase database { get; set; }

        //temp implementations so no errors
        ObservableCollection<Student> IBusinessLogic.Students => throw new NotImplementedException();

        ObservableCollection<Clinic> IBusinessLogic.Clinics => throw new NotImplementedException();

        ObservableCollection<Clinical> IBusinessLogic.Clinicals => throw new NotImplementedException();

        ObservableCollection<AssignedPreceptor> IBusinessLogic.Preceptors => throw new NotImplementedException();

        /*
        public ObservableCollection<Student> Students { get { return database.} }

        public ObservableCollection<Clinic> Clinics { get { return database.} }

        public ObservableCollection<Clinical> Clinicals { get { return database.} }

        public ObservableCollection<AssignedPreceptor> Preceptors { get { return database.} }
        */
        public BusinessLogic()
		{
            database = new Database();
        }
        public SignInError SignIn(string email, string password)
        {
            //call db for account with email
            Account account = null;
            if (account == null)
            {
                return SignInError.InvalidEmailOrPassword;
            }
            if (password.CompareTo(account.Password) != 0)
            {
                return SignInError.InvalidEmailOrPassword;
            }
            //signin

            return SignInError.NoError;
        }
        public CreateAccountError CreateAccount(string email, string password, string firstName, string lastName)
        {
            //call db for account with email
            Account account = null;
            if (account != null)
            {
                return CreateAccountError.EmailAlreadyUsed;
            }
            if (!email.Contains("@uwosh.edu") || (email.Length < 10 || email.Length > 150))
            {
                return CreateAccountError.InvalidEmail;
            }
            if (password.Length < 8 || password.Length > 50)
            {
                return CreateAccountError.InvalidPassword;
            }
            //makes an account
            database.CreateAccount(email, password, firstName, lastName);
            //Account account = new(email, password, firstName, lastName, "Student");
            return CreateAccountError.NoError;
        }
        public EditAccountError EditAccount(string email, string password, string firstName, string lastName)
        {
            //call db for account with email
            Account account = null;
            if (account != null)
            {
                return EditAccountError.EmailAlreadyUsed;
            }
            if (!email.Contains("@uwosh.edu") || (email.Length < 10 || email.Length > 150))
            {
                return EditAccountError.InvalidEmail;
            }
            if (password.Length < 8 || password.Length > 50)
            {
                return EditAccountError.InvalidPassword;
            }
            if (firstName.Length < 1 || firstName.Length > 50)
            {
                return EditAccountError.InvalidFirstName;
            }
            if (lastName.Length < 1 || lastName.Length > 50)
            {
                return EditAccountError.InvalidLastName;
            }
            //Account with edits
            Account editedAccount = new(email, password, firstName, lastName, "Student");
            //db stuff
            return EditAccountError.NoError;
        }
        public AddWorkedHoursError AddWorkedHours()
        {

            return AddWorkedHoursError.NoError;
        }
        public EditWorkedHoursError EditWorkedHours()
        {

            return EditWorkedHoursError.NoError;
        }
        public DeleteWorkedHoursError DeleteWorkedHours()
        {

            return DeleteWorkedHoursError.NoError;
        }
        public UpdateClinicalInfoError UpdateClinicalInfo()
        {
            
            return UpdateClinicalInfoError.NoError;
        }
        public AddClinicalNoteError AddClinicalNote()
        {
            
            return AddClinicalNoteError.NoError;
        }
        public EditClinicalNoteError EditClinicalNote()
        {
            
            return EditClinicalNoteError.NoError;
        }
        public DeleteClinicalNoteError DeleteClinicalNote()
        {
            
            return DeleteClinicalNoteError.NoError;
        }
        public FindPreviousClinicsError FindPreviousClinics()
        {
            
            return FindPreviousClinicsError.NoError;
        }

        public FindNewClinicError FindNewClinic()
        {
            
            return FindNewClinicError.NoError;
        }
    }
}

