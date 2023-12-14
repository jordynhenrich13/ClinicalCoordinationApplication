using System;
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

        public ObservableCollection<Clinical> clinicalList;
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

        public Account GetUserAccount()
        {
            return database.GetUserAccount();
        }

        public void DeleteProfile()
        {
            database.DeleteProfile();
        }

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
            if (!database.CreateStudentAccount(email, hashedPassword, firstName, lastName))
            {
                return CreateAccountError.DBError;
            }

            // Account created successfully
            return CreateAccountError.NoError;
        }

        public AddCoordinatorError AddCoordinator(string email)
        {
            // Account exists for the email entered
            Account account = database.GetAccount(email);
            if (account == null)
            {
                return AddCoordinatorError.InvalidEmail;
            }
            if (!database.AddCoordinator(email))
            {
                return AddCoordinatorError.DBError;
            }
            return AddCoordinatorError.NoError;
        }
        public EditAccountError EditAccount(string email, string firstName, string lastName)
        {
            Account accountToEdit = database.GetAccount(database.UserId);
            if (!string.IsNullOrWhiteSpace(email))
            {
                Account account = database.GetAccount(email);
                if (account != null)
                {
                    return EditAccountError.EmailAlreadyUsed;
                }
                if (!email.Contains("@uwosh.edu") || (email.Length < 10 || email.Length > 150))
                {
                    return EditAccountError.InvalidEmail;
                }
            }
            else
            {
                email = database.UserId;
            }
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                if (firstName.Length < 1 || firstName.Length > 50)
                {
                    return EditAccountError.InvalidFirstName;
                }
            }
           else
            {
                firstName = "";
            }
            if (!string.IsNullOrWhiteSpace(lastName))
            {
                if (lastName.Length < 1 || lastName.Length > 50)
                {
                    return EditAccountError.InvalidLastName;
                }
            }
            else
            {
                lastName = "";
            }
            if (accountToEdit.Role == "Coordinator" || accountToEdit.Role == "Director")
            {
                if (!database.EditCoordinatorAccount(email, firstName, lastName)) 
                {
                    return EditAccountError.DBError;
                }
            }
            else
            {
                if (!database.EditStudentAccount(email, firstName, lastName))
                {
                    return EditAccountError.DBError;
                }
            }
            return EditAccountError.NoError;
        }

        public AddWorkedHoursError AddWorkedHours(String clinical, DateTime date, TimeSpan startTime, TimeSpan endTime, String notes, string email, DateTime recordInserted)
        {
            TimeSpan duration = endTime - startTime;
            double hoursWorked = duration.TotalHours;

            database.AddHoursWorked(clinical, date, hoursWorked, notes, email, recordInserted);

            return AddWorkedHoursError.NoError;
        }

        public EditWorkedHoursError EditWorkedHours()
        {

            return EditWorkedHoursError.NoError;
        }

        public DeleteWorkedHoursError DeleteWorkedHours()
        {

            return DeleteWorkedHoursError.NoError;
        }

        public UpdateClinicalInfoError UpdateClinicalInfo()
        {

            return UpdateClinicalInfoError.NoError;
        }

        public AddClinicalNoteError AddClinicalNote()
        {

            return AddClinicalNoteError.NoError;
        }

        public EditClinicalNoteError EditClinicalNote()
        {

            return EditClinicalNoteError.NoError;
        }

        public DeleteClinicalNoteError DeleteClinicalNote()
        {

            return DeleteClinicalNoteError.NoError;
        }

        public FindPreviousClinicsError FindPreviousClinics()
        {

            return FindPreviousClinicsError.NoError;
        }

        public FindNewClinicError FindNewClinic()
        {

            return FindNewClinicError.NoError;
        }

        public void GetAllStudents()
        {
            database.SelectAllStudents();
        }

        public FindStudentError FindStudent(string search)
        {
            if (search.Length > 50)
            {
                return FindStudentError.SearchTooLong;
            }
            if (search.Contains("%") || search.Contains("_"))
            {
                return FindStudentError.InvalidChar;
            }
            database.FindStudent(search);
            return FindStudentError.NoError;
        }

        public Clinical GetCLinicalInfo(string email)
        {
            Clinical clinical = database.GetDashBoardClinicalInformation(email);
            return clinical;
        }

        public Clinical GetLatestCLinicalSubmission(string email)
        {
            Clinical clinical = database.GetLatestClinicalSubmission(email);
            return clinical;
        }
        public ObservableCollection<Clinical> GetStudentClinicalHours(string email)
        {
            return database.GetStudentClinicalHours(email);
        }

        public void UpdateCurrentClinical(string email, string currentClinical)
        {
            database.UpdateCurrentClinical(email, currentClinical);
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
        public AddReportError AddReport(string reportName,
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

            // Check if report name is already in the DB
            ObservableCollection<ReportItem> allReports = database.GetAllReports();
            foreach(ReportItem report in allReports)
            {
                if (report.ReportName == reportItem.ReportName) return AddReportError.DuplicateReportNameError;
            }

            // Add report to the observable collection
            AllReports.Add(reportItem);

            // Return database AddReportError
            return database.AddReport(reportItem);
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
        /// <param name="submissionDate">Date/time the report was submitted</param>
        public void AddReportSubmission(ReportSubmission submission)
        {
            database.AddReportSubmission(submission);

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
        /// A toast that states that a report already exists with the same name
        /// </summary>
        /// <param name="reportName">The report name</param>
        /// <returns>Toast</returns>
        public async Task ShowDuplicateReportNameToast(string reportName)
        {
            // Generate a cancellation token
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            string text = $"A report with the name {reportName} already exists in the database. Please change the report name.";
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
        /// Gets all submissions for a report
        /// </summary>
        /// <param name="reportName">name of report</param>
        /// <returns>collection of submissions</returns>
        public ObservableCollection<ReportSubmission> GetReportSubmissions(string reportName)
        {
            return database.GetReportSubmissions(reportName);
        }

        /// <summary>
        /// Checks if coordinator exists in the DB
        /// </summary>
        /// <param name="email">Coordinator's email</param>
        /// <returns>true or false if found</returns>
        public bool FindCoordinatorByEmail(string email)
        {
            bool result = database.FindCoordinatorByEmail(email);
            if (result == false) return false; // User not found
            return true;
        }
    }
}