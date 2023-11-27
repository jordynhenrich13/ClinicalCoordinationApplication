using System;
using System.Collections.ObjectModel;

namespace ClinicalCoordinationApplication.Model
{
	public interface IDatabase
	{
        Student StudentSignIn(string email, string password);
        CreateAccountError CreateStudentAccount(string email, string password, string firstName, string LastName);
        CreateAccountError CreateCoordinatorAccount(string email, string password, string firstName, string LastName);
        Account GetAccount(string email);
        Coordinator CoordinatorSignIn(string email, string password);
        ObservableCollection<Student> SelectAllStudents();
        ObservableCollection<Student> FindStudent(string search);
        ObservableCollection<Student> Students { get; }
        public string GetUserType();

        public void DeleteProfile();
        AddWorkedHoursError AddHoursWorked(String clinical, DateTime dateTime, TimeSpan clinicalHoursWorked, string notes);

    }
}

