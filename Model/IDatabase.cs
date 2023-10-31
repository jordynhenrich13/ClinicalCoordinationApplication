using System;
namespace ClinicalCoordinationApplication.Model
{
	public interface IDatabase
	{
        string SignIn(string email, string password);
        CreateAccountError CreateAccount(string email, string password, string firstName, string LastName);

    }
}

