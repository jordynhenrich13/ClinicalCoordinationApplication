using System;
using System.Collections.ObjectModel;
using ClinicalCoordinationApplication.Model.Reports;

namespace ClinicalCoordinationApplication.Model
{
	public interface IDatabase
	{
        void SignIn(string email);
        CreateAccountError CreateStudentAccount(string email, string password, string firstName, string LastName);
        CreateAccountError CreateCoordinatorAccount(string email, string password, string firstName, string LastName);
        Account GetAccount(string email);
        ObservableCollection<Student> SelectAllStudents();
        ObservableCollection<Student> FindStudent(string search);
        ObservableCollection<Student> Students { get; }
        public string GetUserType();
        public void DeleteProfile();
        AddWorkedHoursError AddHoursWorked(String clinical, DateTime dateTime, TimeSpan clinicalHoursWorked, string notes);
        ObservableCollection<ReportItem> GetDirectorReports();
        ObservableCollection<ReportSubmission> GetReportSubmissions(string reportName);
        ObservableCollection<ReportItem> GetAllReports();
        AddReportError AddReport(ReportItem reportItem);
        DeleteReportError DeleteReport(string reportName);
        AddReportSubmissionError AddReportSubmission(ReportSubmission reportSubmission);
        bool FindCoordinatorByEmail(string email);
        public ObservableCollection<ReportItem> GetCoordinatorReports(string userEmail);
        Student GetStudentInfo(string email);
        Coordinator GetCoordinatorInfo(string email);
    }
}

