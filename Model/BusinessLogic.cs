﻿using System;
using System.Collections.ObjectModel;
using BCrypt.Net;
using ClinicalCoordinationApplication.Model.Reports;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace ClinicalCoordinationApplication.Model
{
    public class BusinessLogic : IBusinessLogic
    {
        private IDatabase database { get; set; }

        public ObservableCollection<Student> Students { get { return database.Students; } }
        /*
        public ObservableCollection<Clinic> Clinics { get { return database.} }

        public ObservableCollection<Clinical> Clinicals { get { return database.} }

        public ObservableCollection<AssignedPreceptor> Preceptors { get { return database.} }
        */
        //temp implementations so no errors
        ObservableCollection<Clinic> IBusinessLogic.Clinics => throw new NotImplementedException();

        ObservableCollection<Clinical> IBusinessLogic.Clinicals => throw new NotImplementedException();

        ObservableCollection<AssignedPreceptor> IBusinessLogic.Preceptors => throw new NotImplementedException();

        // Probably needs refactoring at a database level as this use of informai is not recommende
        public BusinessLogic()
        {
            database = new Database();
            AllReports = database.GetAllReports();
            CoordinatorReports = database.GetCoordinatorReports(Preferences.Get("user_email", "Unknown"));
        }

        /// <summary>
        /// Returns the logged in user's role
        /// </summary>
        /// <returns>A string representing the user's type. Either Student, Coordinator, or Director</returns>
        public string GetUserType()
        {
            return database.GetUserType();
        }

        /// <summary>
        /// Deletes the user's profile
        /// </summary>
        public void DeleteProfile()
        {
            database.DeleteProfile();
        }

        /// <summary>
        /// Signs the user into the app.
        /// </summary>
        /// <param name="email">The user's email</param>
        /// <param name="password">The user's password</param>
        /// <returns>Error on sign in status</returns>
        public SignInError SignIn(string email, string password)
        {
            Account account = database.GetAccount(email);
            if (account == null)
            {
                return SignInError.InvalidEmailOrPassword;
            }
            if (!BCrypt.Net.BCrypt.Verify(password, account.Password))
            {
                return SignInError.InvalidEmailOrPassword;
            }

            // Store user type in preferences
            Preferences.Set("user_type", account.Role);

            // Sign user in (in the DB)
            database.SignIn(email);

            // Assign the rest of the user preferences
            if (account.Role == "Student")
            {
                Student studentInfo = database.GetStudentInfo(email);
                Preferences.Set("user_full_name", studentInfo.FirstName + " " + studentInfo.LastName);
                Preferences.Set("user_first_name", studentInfo.FirstName);
                Preferences.Set("user_last_name", studentInfo.LastName);
                Preferences.Set("user_email", studentInfo.Email);
            } else if (account.Role == "Coordinator" || account.Role == "Director")
            {
                Coordinator coordinatorInfo = database.GetCoordinatorInfo(email);
                Preferences.Set("user_full_name", coordinatorInfo.FirstName + " " + coordinatorInfo.LastName);
                Preferences.Set("user_first_name", coordinatorInfo.FirstName);
                Preferences.Set("user_last_name", coordinatorInfo.LastName);
                Preferences.Set("user_email", coordinatorInfo.Email);
            }

            // Login completed successfully!
            return SignInError.NoError;
        }

        /// <summary>
        /// Creates a student account by calling the database to add to the student table
        /// </summary>
        /// <param name="email">Student's email</param>
        /// <param name="password">Student's password</param>
        /// <param name="firstName">Student's first name</param>
        /// <param name="lastName">Student's last name</param>
        /// <returns></returns>
        public CreateAccountError CreateStudentAccount(string email, string password, string firstName, string lastName)
        {
            // Account exists for the email entered
            Account account = database.GetAccount(email);
            if (account != null)
            {
                return CreateAccountError.EmailAlreadyUsed;
            }

            // Email length is invalid
            if ((email.Length < 10 || email.Length > 100) || !email.Contains("@uwosh.edu"))
            {
                return CreateAccountError.InvalidEmail;
            }

            // Password length is invalid
            if (password.Length < 8 || password.Length > 50)
            {
                return CreateAccountError.InvalidPassword;
            }

            // First name length is invalid
            if (firstName.Length < 1 || firstName.Length > 50)
            {
                return CreateAccountError.InvalidFirstName;
            }

            // Last name length is invalid
            if (lastName.Length < 1 || lastName.Length > 50)
            {
                return CreateAccountError.InvalidLastName;
            }

            // Hash the user's password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Create a student account
            database.CreateStudentAccount(email, hashedPassword, firstName, lastName);

            // Account created successfully
            return CreateAccountError.NoError;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public CreateAccountError CreateCoordinatorAccount(string email, string password, string firstName, string lastName)
        {
            // Account exists for the email entered
            Account account = database.GetAccount(email);
            if (account != null)
            {
                return CreateAccountError.EmailAlreadyUsed;
            }

            // Email length is invalid
            if ((email.Length < 10 || email.Length > 150) || !email.Contains("@uwosh.edu"))
            {
                return CreateAccountError.InvalidEmail;
            }

            // Password length is invalid
            if (password.Length < 8 || password.Length > 50)
            {
                return CreateAccountError.InvalidPassword;
            }

            // First name length is invalid
            if (firstName.Length < 1 || firstName.Length > 50)
            {
                return CreateAccountError.InvalidFirstName;
            }

            // Last name length is invalid
            if (lastName.Length < 1 || lastName.Length > 50)
            {
                return CreateAccountError.InvalidLastName;
            }

            // Hash the passord
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Create a coordinator account
            database.CreateCoordinatorAccount(email, hashedPassword, firstName, lastName);

            // Account created successfully
            return CreateAccountError.NoError;
        }

        /// <summary>
        /// Edits a user's account information.
        /// </summary>
        /// <param name="email">TODO</param>
        /// <param name="password">TODO</param>
        /// <param name="firstName">TODO</param>
        /// <param name="lastName">TODO</param>
        /// <returns></returns>
        public EditAccountError EditAccount(string email, string password, string firstName, string lastName)
        {
            //call db for account with email
            Account account = database.GetAccount(email);
            if (account != null)
            {
                return EditAccountError.EmailAlreadyUsed;
            }
            if (!email.Contains("@uwosh.edu") || (email.Length < 10 || email.Length > 150))
            {
                return EditAccountError.InvalidEmail;
            }
            if (password.Length < 8 || password.Length > 50)
            {
                return EditAccountError.InvalidPassword;
            }
            if (firstName.Length < 1 || firstName.Length > 50)
            {
                return EditAccountError.InvalidFirstName;
            }
            if (lastName.Length < 1 || lastName.Length > 50)
            {
                return EditAccountError.InvalidLastName;
            }

            // Hash the password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Update account successful!
            return EditAccountError.NoError;
        }

        /// <summary>
        /// Adds a student's worked hours to the database.
        /// </summary>
        /// <param name="clinical">TODO</param>
        /// <param name="date">TODO</param>
        /// <param name="startTime">TODO</param>
        /// <param name="endTime">TODO</param>
        /// <param name="notes">TODO</param>
        /// <returns></returns>
        public AddWorkedHoursError AddWorkedHours(String clinical, DateTime date, TimeSpan startTime, TimeSpan endTime, String notes)
        {
            TimeSpan duration = endTime - startTime;
            database.AddHoursWorked(clinical, date, duration, notes);
            return AddWorkedHoursError.NoError;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public EditWorkedHoursError EditWorkedHours()
        {

            return EditWorkedHoursError.NoError;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public DeleteWorkedHoursError DeleteWorkedHours()
        {

            return DeleteWorkedHoursError.NoError;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public UpdateClinicalInfoError UpdateClinicalInfo()
        {

            return UpdateClinicalInfoError.NoError;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public AddClinicalNoteError AddClinicalNote()
        {

            return AddClinicalNoteError.NoError;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public EditClinicalNoteError EditClinicalNote()
        {

            return EditClinicalNoteError.NoError;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public DeleteClinicalNoteError DeleteClinicalNote()
        {

            return DeleteClinicalNoteError.NoError;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public FindPreviousClinicsError FindPreviousClinics()
        {

            return FindPreviousClinicsError.NoError;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public FindNewClinicError FindNewClinic()
        {

            return FindNewClinicError.NoError;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public FindStudentError FindStudent(string search)
        {
            if (search.Length > 50)
            {
                return FindStudentError.SearchTooLong;
            }
            database.FindStudent(search);
            return FindStudentError.NoError;
        }

        /******************************
         * REPORT PAGE VARS & METHODS *
         *****************************/
        public ObservableCollection<ReportItem> AllReports;
        public ObservableCollection<ReportItem> CoordinatorReports;

        /// <summary>
        /// Uploads a report with the specified parameters.
        /// </summary>
        /// <param name="reportToUpload">Stream of the report to upload</param>
        /// <param name="reportName">Name of the report to upload</param>
        /// <param name="dueDate">Date the completed reports are due by</param>
        /// <param name="sendTo">Which coordinators to send the report to</param>
        public void AddReport(string reportName,
                              string fileName,
                              Stream reportStream,
                              string uploadedBy,
                              DateTime uploadDate,
                              DateTime dueDate, string[] sendTo)
        {
            // Create an collection for future submissions
            ObservableCollection<ReportSubmission> submissions = new();

            // Create a new report item with the specified information
            ReportItem reportItem = new(reportName, fileName, reportStream, uploadedBy, dueDate, uploadDate, sendTo, submissions);

            // Add report to the observable collection
            AllReports.Add(reportItem);

            database.AddReport(reportItem);

            //return database.AddReport(reportItem);
        }

        /// <summary>
        /// Returns reports for a specified coordinator from the DB
        /// </summary>
        /// <param name="email">Email of the coordinator to search reports for</param>
        /// <returns>Observable collec</returns>
        public ObservableCollection<ReportItem> GetCoordinatorReports(string email)
        {
            return database.GetCoordinatorReports(email);
        }

        /// <summary>
        /// Deletes a report using a specified name as the identifier.
        /// </summary>
        /// <param name="reportName">Name of the report to be deleted</param>
        public void DeleteReport(string reportName)
        {
            database.DeleteReport(reportName);
            
        }

        /// <summary>
        /// Uploads a completed report. To be stored as a submission under the
        /// corresponding report item.
        /// </summary>
        /// <param name="reportName">File name of the completed report</param>
        /// <param name="submissionDate">Date/time the report was submitted</param>
        public void AddReportSubmission(string fileName,
                                        Stream reportStream,
                                        DateTime submissionDate,
                                        string uploadedBy,
                                        string reportName)
        {
            // Create new report submission object
            ReportSubmission reportSubmission = new(fileName, reportStream, uploadedBy, submissionDate, reportName);

            // Add report to the observable collection
            //var report = AllReports.FirstOrDefault(r => r.ReportName == reportName);
            //if (report != null) report.Submissions.Add(reportSubmission);

            // TODO: Add submitted report to the database
            database.AddReportSubmission(reportSubmission);

        }

        public ObservableCollection<ReportItem> GetDirectorReports()
        {
            return database.GetDirectorReports();
        }

        /// <summary>
        /// Toast that shows when a user cancels a task.
        /// </summary>
        /// <param name="action">What task was cancelled</param>
        /// <returns>A task to show the toast</returns>
        public async Task ShowCancellationToast(string action)
        {
            // Generate a cancellation token
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            string text = $"{action} was cancelled";
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(text, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
        }

        /// <summary>
        /// Shows a Toast that a file was downloaded.
        /// </summary>
        /// <param name="downloadName">The name of the file downloaded</param>
        public async void ShowDownloadToast(string downloadName)
        {
            // Generate a cancellation token
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            string text = $"{downloadName} was downloaded";
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(text, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
        }

        public async Task ShowNoSubmissionsToast()
        {
            // Generate a cancellation token
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            string text = "No submissions found";
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(text, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="reportName">TODO</param>
        /// <returns>TODO</returns>
        public ObservableCollection<ReportSubmission> GetReportSubmissions(string reportName)
        {
            return database.GetReportSubmissions(reportName);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="email">TODO</param>
        /// <returns>TODO</returns>
        public bool FindCoordinatorByEmail(string email)
        {
            bool result = database.FindCoordinatorByEmail(email);
            if (result == false) return false; // User not found
            return true;
        }
    }
}