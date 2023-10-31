using System;
namespace ClinicalCoordinationApplication.Model
{
	public interface IDatabase
	{
        string SignIn(string email, string password);

    }
}

