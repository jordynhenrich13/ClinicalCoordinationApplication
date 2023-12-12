using System;
using System.Collections.ObjectModel;
using BCrypt.Net;

namespace ClinicalCoordinationApplication.Model
{
	public class BusinessLogic : IBusinessLogic
	{
        private IDatabase database { get; set; }

        public ObservableCollection<Student> Students { get { return database.Students; } }
        /*
        public ObservableCollection<Clinic> Clinics { get { return database.} }

        public ObservableCollection<Clinical> Clinicals { get { return database.} }

        public ObservableCollection<AssignedPreceptor> Preceptors { get { return database.} }
        */
        //temp implementations so no errors
        ObservableCollection<Clinic> IBusinessLogic.Clinics => throw new NotImplementedException();

        ObservableCollection<Clinical> IBusinessLogic.Clinicals => throw new NotImplementedException();

        ObservableCollection<AssignedPreceptor> IBusinessLogic.Preceptors => throw new NotImplementedException();

        public string LoggedInUserType { get; set; } // TODO: RETURNS NULL!
        public string LoggedInUserName { get; set; } // TODO: RETURNS NULL!
        // Probably needs refactoring at a database level as this use of informai is not recommende
        public BusinessLogic()
		{
            database = new Database();
        }

        public string GetUserType()
        {
            return database.GetUserType();
        }

        public void DeleteProfile()
        {
            database.DeleteProfile();
        }

        public SignInError SignIn(string email, string password)
        {
            Account account = database.GetAccount(email);
            if (account == null)
            {
                return SignInError.InvalidEmailOrPassword;
            }
            if (!BCrypt.Net.BCrypt.Verify(password, account.Password))
            {
                return SignInError.InvalidEmailOrPassword;
            }
            LoggedInUserType = account.Role;
            database.SignIn(email);
            return SignInError.NoError;
        }

        public CreateAccountError CreateStudentAccount(string email, string password, string firstName, string lastName)
        {
            Account account = database.GetAccount(email);
            if (account != null)
            {
                return CreateAccountError.EmailAlreadyUsed;
            }
            if ((email.Length < 10 || email.Length > 100) || !email.Contains("@uwosh.edu"))
            {
                return CreateAccountError.InvalidEmail;
            }
            if (password.Length < 8 || password.Length > 50)
            {
                return CreateAccountError.InvalidPassword;
            }
            if (firstName.Length < 1 || firstName.Length > 50)
            {
                return CreateAccountError.InvalidFirstName;
            }
            if (lastName.Length < 1 || lastName.Length > 50)
            {
                return CreateAccountError.InvalidLastName;
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            database.CreateStudentAccount(email, hashedPassword, firstName, lastName);
            return CreateAccountError.NoError;
        }

        public AddCoordinatorError AddCoordinator(string email)
        {
            Account account = database.GetAccount(email);
            if (account == null)
            {
                return AddCoordinatorError.InvalidEmail;
            }
            database.AddCoordinator(email);
            return AddCoordinatorError.NoError;
        }
        public EditAccountError EditAccount(string email, string password, string firstName, string lastName)
        {
            //call db for account with email
            Account account = database.GetAccount(email);
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
            //db stuff
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return EditAccountError.NoError;
        }
        public AddWorkedHoursError AddWorkedHours(String clinical, DateTime date, TimeSpan startTime, TimeSpan endTime, String notes)
        {
            TimeSpan duration = endTime - startTime;
            database.AddHoursWorked(clinical, date, duration, notes);
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

        public void GetAllStudents()
        {
            database.SelectAllStudents();
        }

        public FindStudentError FindStudent(string search)
        {
            if (search.Length > 50)
            {
                return FindStudentError.SearchTooLong;
            }
            if (search.Contains("%") || search.Contains("_"))
            {
                return FindStudentError.InvalidChar;
            }
            database.FindStudent(search);
            return FindStudentError.NoError;
        }
    }
}

