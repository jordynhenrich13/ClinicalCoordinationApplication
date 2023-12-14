using System;
using System.Collections.ObjectModel;

namespace ClinicalCoordinationApplication.Model
{
    public interface IBusinessLogic
    {
        public ObservableCollection<Student> Students { get; }
        public ObservableCollection<Clinic> Clinics { get; }
        public ObservableCollection<Clinical> Clinicals { get; }
        public ObservableCollection<AssignedPreceptor> Preceptors { get; }
        SignInError SignIn(string email, string password);
        CreateAccountError CreateStudentAccount(string email, string password, string firstName, string lastName);
        AddCoordinatorError AddCoordinator(string email);
        EditAccountError EditAccount(string email, string firstName, string lastName);
        AddWorkedHoursError AddWorkedHours(String clinical, DateTime date, TimeSpan startTime, TimeSpan endTime, String notes, string email, DateTime recordInserted);
        EditWorkedHoursError EditWorkedHours();
        DeleteWorkedHoursError DeleteWorkedHours();
        UpdateClinicalInfoError UpdateClinicalInfo();
        AddClinicalNoteError AddClinicalNote();
        EditClinicalNoteError EditClinicalNote();
        DeleteClinicalNoteError DeleteClinicalNote();
        FindPreviousClinicsError FindPreviousClinics();
        FindNewClinicError FindNewClinic();
        void DeleteProfile();
        void GetAllStudents();
        FindStudentError FindStudent(string search);
        Clinical GetCLinicalInfo(string email);
        Clinical GetLatestCLinicalSubmission(string email);
        ObservableCollection<Clinical> GetStudentClinicalHours(string email);
        void UpdateCurrentClinical(string email, string currentClinical);

    }
}

