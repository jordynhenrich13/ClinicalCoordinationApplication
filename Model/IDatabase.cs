using System;
using System.Collections.ObjectModel;

namespace ClinicalCoordinationApplication.Model
{
    public interface IDatabase
    {
        void SignIn(string email);
        bool CreateStudentAccount(string email, string password, string firstName, string lastName);
        bool AddCoordinator(string email);
        Account GetAccount(string email);
        Clinical GetDashBoardClinicalInformation(string email);
        ObservableCollection<Student> SelectAllStudents();
        ObservableCollection<Student> FindStudent(string search);
        ObservableCollection<Student> Students { get; }
        public Account GetUserType();
        public void DeleteProfile();
        AddWorkedHoursError AddHoursWorked(String clinical, DateTime dateTime, double clinicalHoursWorked, string notes, string studentEmail, DateTime insertRecordDTM);
        Clinical GetLatestClinicalSubmission(string email);
    }
}

