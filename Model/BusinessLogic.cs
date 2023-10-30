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
            //call db instead of null
            Account account = null;
            if (account == null)
            {
                return SignInError.InvalidEmailOrPassword;
            }
            if (password.CompareTo(account.Password) != 0) 
            {
                return SignInError.InvalidEmailOrPassword;
            }
            //sign in

            return SignInError.NoError;
        }
        public CreateAccountError CreateAccount(string email, string password, string firstName, string lastName)
        {
            if (email.IndexOf("@uwosh.edu") == -1)
            {
                return CreateAccountError.InvalidEmail;
            }
            if (password.Length < 8)
            {
                return CreateAccountError.InvalidPassword;
            }
            //makes an account
            Account account = new(email, password, firstName, lastName, "Student");
            return CreateAccountError.NoError;
        }
        public EditAccountError EditAccount()
        {

            return EditAccountError.NoError;
        }
	}
}

