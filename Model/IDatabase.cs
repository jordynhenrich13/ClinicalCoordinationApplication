using System;
namespace ClinicalCoordinationApplication.Model
{
	public interface IDatabase
	{
        string SignIn(string email, string password);
        CreateAccountError CreateStudentAccount(string email, string password, string firstName, string LastName);
        CreateAccountError CreateCoordinatorAccount(string email, string password, string firstName, string LastName);


    }
}

