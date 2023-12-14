using System;
using System.Collections.ObjectModel;
using ClinicalCoordinationApplication.Model.Reports;

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
        Student GetStudentInfo(string email);
        Coordinator GetCoordinatorInfo(string email);
        ObservableCollection<ReportItem> GetDirectorReports();
        ObservableCollection<ReportSubmission> GetReportSubmissions(string reportName);
        ObservableCollection<ReportItem> GetAllReports();
        AddReportError AddReport(ReportItem reportItem);
        DeleteReportError DeleteReport(string reportName);
        AddReportSubmissionError AddReportSubmission(ReportSubmission reportSubmission);
        bool FindCoordinatorByEmail(string email);
        public ObservableCollection<ReportItem> GetCoordinatorReports(string userEmail);
        ObservableCollection<Clinical> GetStudentClinicalHours(string email);
        void UpdateCurrentClinical(string email, string currentClinical);

    }
}

