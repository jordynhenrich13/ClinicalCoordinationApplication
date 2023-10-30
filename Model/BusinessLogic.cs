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
        public SignInError SignIn(String email, String password)
        {


            return SignInError.NoError;
        }
        public CreateAccountError CreateAccount()
        {

            return CreateAccountError.NoError;
        }
        public EditAccountError EditAccount()
        {

            return EditAccountError.NoError;
        }
	}
}

