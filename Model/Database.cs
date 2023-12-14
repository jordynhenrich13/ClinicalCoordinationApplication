using ClinicalCoordinationApplication.Model;
using ClinicalCoordinationApplication.Model.Reports;
using Microsoft.Maui.ApplicationModel.Communication;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using System.Collections;
using System.Net.WebSockets;
//using Windows.Networking;
using static Android.Provider.ContactsContract.CommonDataKinds;
using System.Data;

namespace ClinicalCoordinationApplication;
public class Database : IDatabase
{
    private string connString;
    // Constructor initializes and sets up the database.
    // It also ensures that the file exists or creates it if not.

    public static string userId { get; set; }
    ObservableCollection<Student> students = new();

    public string CurrentlySignedInStudentEmail { get; private set; }

    // This is for the binding context for the student's name to be binded in the coordinator dashboard and display their first and last name
    private string studentName;

    public Database()
    {
        connString = GetConnectionString();
        //CreateTables(connString);
        SelectAllStudents();
    }

    public ObservableCollection<Student> Students
    {
        get { return students; }
        set
        {
            if (students != value)
            {
                students = value;
                OnPropertyChanged(nameof(Students));
            }
        }
    }

    public string StudentName
    {
        get { return studentName; }
        set
        {
            if (studentName != value)
            {
                studentName = value;
                // Notify property changed to update the UI
                OnPropertyChanged(nameof(StudentName));
            }
        }
    }

    public void SignIn(string email)
    {
        userId = email;
    }

 
    public Account GetUserType()
    {
        string role = null;
        Account account = GetAccount(userId);
        if (account != null)
        {
            role = account.Role;
        }
        return account;
    }

    public void SavePreceptorToDatabase(PreceptorViewModel preceptor)
    {
        try
        {
            using var conn = new NpgsqlConnection(connString);
            using var transaction = conn.BeginTransaction();

            conn.Open();

            using var cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO Preceptor (Title, Name, Facility, Email, Phone) " +
                              "VALUES (@Title, @Name, @Facility, @Email, @Phone)";

            cmd.Parameters.AddWithValue("Title", preceptor.Title);
            cmd.Parameters.AddWithValue("Name", preceptor.Name);
            cmd.Parameters.AddWithValue("Facility", preceptor.Facility);
            cmd.Parameters.AddWithValue("PreceptorEmail", preceptor.Email);
            cmd.Parameters.AddWithValue("Phone", preceptor.Phone);

            var numAffected = cmd.ExecuteNonQuery();
            transaction.Commit();


            // You may want to check numAffected to ensure the data was successfully inserted
            if (numAffected > 0)
            {
                Console.WriteLine("Preceptor information saved to the database");
            }
            else
            {
                transaction.Rollback();

                Console.WriteLine("Failed to save preceptor information to the database");
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions, log errors, etc.
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public PreceptorViewModel LoadPreceptorInformation(string studentEmail, int clinicalPageNumber)
    {
        using var conn = new NpgsqlConnection(connString);
        conn.Open();

        using var cmdRetrieve = new NpgsqlCommand();
        cmdRetrieve.Connection = conn;
        cmdRetrieve.CommandText = "SELECT Title, Name, Facility, Phone, PreceptorEmail " +
                                  "FROM Preceptor " +
                                  "WHERE studentemail = @StudentEmail";
        cmdRetrieve.Parameters.AddWithValue("StudentEmail", NpgsqlDbType.Varchar, studentEmail != null ? (object)studentEmail : DBNull.Value);

        using var reader = cmdRetrieve.ExecuteReader();

        if (reader.Read())
        {
            // Assuming PreceptorViewModel has a constructor that takes these values
            return new PreceptorViewModel
            {
                Title = reader.IsDBNull(0) ? null : reader.GetString(0),
                Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                Facility = reader.IsDBNull(2) ? null : reader.GetString(2),
                Phone = reader.IsDBNull(3) ? null : reader.GetString(3),
                PreceptorEmail = reader.IsDBNull(4) ? null : reader.GetString(4),
                ClinicalPageNumber = clinicalPageNumber
            };
        }
        // Return null if no preceptor information is found
        return null;
    }

    private Student QueryStudentData(string userId)
    {
        using var conn = new NpgsqlConnection(connString);
        conn.Open();
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "SELECT FirstName, Lastname, Email FROM Student WHERE Email = @Email";
        cmd.Parameters.AddWithValue("Email", NpgsqlDbType.Varchar, userId);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            if (!reader.IsDBNull(0))
            {
                return new Student(reader.GetString(0), reader.GetString(1), reader.GetString(2));
            }
        }

        return null;
    }

    public AddWorkedHoursError AddHoursWorked(String clinical, DateTime dateWorked, double clinicalHoursWorked, string notes, string studentEmail, DateTime recordInsertedDTM)
    {
        try
        {
            // string studentEmail = "henrij13@uwosh.edu";

            // Retrieve the latest clinical ID from the database and increment it manually
            int latestClinicalId = GetLatestClinicalIdFromDatabase();
            int nextClinicalId = latestClinicalId + 1;

            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            var cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO clinical (clinicalid, studentemail, clinicalname, dateWorked, hoursworked, notes, recordinserteddtm) " +
                              "VALUES (@clinicalid, @studentEmail, @clinical, @dateWorked, @clinicalHoursWorked, @notes, @recordInsertedDTM)";

            cmd.Parameters.AddWithValue("clinicalid", nextClinicalId.ToString("D3"));  // Format as 3-digit string
            cmd.Parameters.AddWithValue("studentEmail", studentEmail);
            cmd.Parameters.AddWithValue("clinical", clinical);
            cmd.Parameters.AddWithValue("dateWorked", dateWorked.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("clinicalHoursWorked", clinicalHoursWorked);
            cmd.Parameters.AddWithValue("notes", notes);
            cmd.Parameters.AddWithValue("recordInsertedDTM", recordInsertedDTM.ToString("yyyy-MM-ddTHH:mm:ss"));

            var numAffected = cmd.ExecuteNonQuery();

            return AddWorkedHoursError.NoError;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return AddWorkedHoursError.InvalidNumber;
        }
    }

    // Helper method to get the latest clinical ID from the database
    private int GetLatestClinicalIdFromDatabase()
    {
        try
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT MAX(clinicalid) FROM clinical", conn);
            var result = cmd.ExecuteScalar();

            if (result != null && int.TryParse(result.ToString(), out var maxId))
            {
                return maxId;
            }

            // If there are no records yet, start from 1
            return 0;
        }
        catch (Exception ex)
        {
            // Handle the exception appropriately (log, throw, etc.)
            Console.WriteLine(ex);
            return 0;
        }
    }

    private Coordinator QueryCoordinatorData(string userId)
    {
        using var conn = new NpgsqlConnection(connString);
        conn.Open();
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "SELECT FirstName, Lastname, Email FROM ClinicalCoordinator WHERE Email = @Email";
        cmd.Parameters.AddWithValue("Email", NpgsqlDbType.Varchar, userId);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            if (!reader.IsDBNull(0))
            {
                return new Coordinator(reader.GetString(0), reader.GetString(1), reader.GetString(2));
            }
        }

        return null;
    }


    public void DeleteProfile()
    {
        var conn = new NpgsqlConnection(connString);
        conn.Open();
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "DELETE FROM ClinicalCoordinator WHERE Email = @Email";
        cmd.Parameters.AddWithValue("Email", "jansse18@uwosh.edu");
        using var reader = cmd.ExecuteReader();
    }

    public bool CreateStudentAccount(string email, string password, string firstName, string lastName)
    {
        try
        {
            using var conn = new NpgsqlConnection(connString); // conn, short for connection, is a connection to the database

            conn.Open(); // open the connection ... now we are connected!
            var cmd = new NpgsqlCommand(); // create the sql commaned
            cmd.Connection = conn; // commands need a connection, an actual command to execute
            cmd.CommandText = "INSERT INTO Student (FirstName, Lastname, Email) VALUES (@FirstName, @Lastname, @Email)";
            cmd.Parameters.AddWithValue("FirstName", firstName);
            cmd.Parameters.AddWithValue("lastName", lastName);
            cmd.Parameters.AddWithValue("Email", email);
            cmd.ExecuteNonQuery(); // used for INSERT, UPDATE & DELETE statements - returns # of affected rows 

            using var conn2 = new NpgsqlConnection(connString);
            conn2.Open();
            var cmd2 = new NpgsqlCommand();
            cmd2.Connection = conn2;
            cmd2.CommandText = "INSERT INTO Account (email, password, role) VALUES (@email, @password, @role)";
            cmd2.Parameters.AddWithValue("email", email);
            cmd2.Parameters.AddWithValue("password", password);
            cmd2.Parameters.AddWithValue("role", "Student");
            cmd2.ExecuteNonQuery();
        }
        catch (Npgsql.PostgresException pe)
        {
            Console.WriteLine("Insert failed, {0}", pe);
            return false;
        }
        return true;
    }

    public bool AddCoordinator(string email)
    {
        try
        {
            var conn = new NpgsqlConnection(GetConnectionString());
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT firstname, lastname FROM Student WHERE email = @email", conn);
            cmd.Parameters.AddWithValue("email", email);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    string firstName = reader.GetString(0);
                    string lastName = reader.GetString(1);

                    var conn2 = new NpgsqlConnection(GetConnectionString());
                    conn2.Open();
                    using var cmd2 = new NpgsqlCommand("DELETE FROM student WHERE email = @email", conn2);
                    cmd2.Parameters.AddWithValue("email", email);
                    cmd2.ExecuteNonQuery();
                    conn2.Close();

                    var conn3 = new NpgsqlConnection(GetConnectionString());
                    conn3.Open();
                    using var cmd3 = new NpgsqlCommand("UPDATE account SET role = @role WHERE email = @email", conn3);
                    cmd3.Parameters.AddWithValue("role", "Coordinator");
                    cmd3.Parameters.AddWithValue("email", email);
                    cmd3.ExecuteNonQuery();
                    conn3.Close();

                    var conn4 = new NpgsqlConnection(GetConnectionString());
                    conn4.Open();
                    using var cmd4 = new NpgsqlCommand("INSERT INTO ClinicalCoordinator (FirstName, Lastname, Email) VALUES (@FirstName, @Lastname, @Email)", conn4);
                    cmd4.Parameters.AddWithValue("FirstName", firstName);
                    cmd4.Parameters.AddWithValue("lastName", lastName);
                    cmd4.Parameters.AddWithValue("Email", email);
                    cmd4.ExecuteNonQuery();
                    return true;
                }
            }
        }
        catch (Npgsql.PostgresException ex)
        {
            Console.WriteLine(ex);
            return false;
        }
        return false;
    }

    public Coordinator GetCoordinatorInfo(string email)
    {
        try
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var cmd = new NpgsqlCommand();
            cmd.Connection = conn;

            // Query Text with parameter
            cmd.CommandText = @"SELECT *
                                FROM ClinicalCoordinator
                                WHERE email = @Email";

            // Add parameter and set its value
            cmd.Parameters.AddWithValue("@Email", email);

            using (var reader = cmd.ExecuteReader())
            {
                // Check if data was found in DB
                if (reader.HasRows)
                {
                    // Read in the data
                    while (reader.Read())
                    {
                        string firstname = reader["firstname"].ToString();
                        string lastname = reader["lastname"].ToString();
                        //var phonenumber = reader["phonenumber"]; TODO: Uncomment when needed

                        // Return a coordinator object
                        return new Coordinator(firstname, lastname, email);
                    }
                }
            }
        }
        catch (Npgsql.PostgresException pe)
        {
            // Catch errors
            Console.WriteLine("Error occurred in the database: {0}", pe);

        }

        // Could not find coordinator, return null
        Console.WriteLine("DB could not find specified coordinator in GetCoordinatorInfo(string email)");
        return null;
    }

    public Student GetStudentInfo(string email)
    {
        try
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var cmd = new NpgsqlCommand();
            cmd.Connection = conn;

            // Query Text with parameter
            cmd.CommandText = @"SELECT *
                                FROM Student
                                WHERE email = @Email";

            // Add parameter and set its value
            cmd.Parameters.AddWithValue("@Email", email);

            using (var reader = cmd.ExecuteReader())
            {
                // Check if data was found
                if (reader.HasRows)
                {
                    // Read in data
                    while (reader.Read())
                    {
                        string firstname = reader["firstname"].ToString();
                        string lastname = reader["lastname"].ToString();
                        //var phonenumber = reader["phonenumber"]; TODO: Uncomment when needed
                        //var address = reader["address"]; TODO: Uncomment when needed

                        // Return student object
                        return new Student(firstname, lastname, email);
                    }
                }
            }
        }
        catch (Npgsql.PostgresException pe)
        {
            // Catch any errors
            Console.WriteLine("Error occurred in the database: {0}", pe);

        }

        // No student was found, return null
        Console.WriteLine("DB could not find specified student in GetStudentInfo(string email)");
        return null;
    }

    // Builds a ConnectionString, which is used to connect to the database
    static String GetConnectionString()
    {
        var connStringBuilder = new NpgsqlConnectionStringBuilder();
        connStringBuilder.Host = "acid-mummy-13033.5xj.cockroachlabs.cloud";
        connStringBuilder.Port = 26257;
        connStringBuilder.SslMode = SslMode.VerifyFull;
        connStringBuilder.Username = "jordyn"; // won't hardcode this in your app
        connStringBuilder.Password = "zNJV69CZDF57UfwDZlHhQw"; // need to hardcode password
        connStringBuilder.Database = "defaultdb";
        connStringBuilder.ApplicationName = "whatever"; // ignored, apparently
        connStringBuilder.IncludeErrorDetail = true;
        return connStringBuilder.ConnectionString;
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <returns>TODO</returns>
    public ObservableCollection<Student> SelectAllStudents()
    {
        students.Clear();
        var conn = new NpgsqlConnection(GetConnectionString());
        conn.Open();

        using var cmd = new NpgsqlCommand("SELECT firstname, lastname, email FROM Student", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            String firstName = reader.GetString(0);
            String lastName = reader.GetString(1);
            String email = reader.GetString(2);
            Student student = new Student(firstName, lastName, email);
            students.Add(student);
        }

        return students;
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="search">TODO</param>
    /// <returns>TODO</returns>
    public ObservableCollection<Student> FindStudent(string search)
    {
        try
        {
            students.Clear();
            var conn = new NpgsqlConnection(GetConnectionString());
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT firstname, lastname, email FROM Student WHERE firstname ILIKE @search OR lastname ILIKE @search", conn);
            cmd.Parameters.AddWithValue("search", search + "%");
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                String firstName = reader.GetString(0);
                String lastName = reader.GetString(1);
                String email = reader.GetString(2);
                Student student = new Student(firstName, lastName, email);
                students.Add(student);
            }
        }
        catch (Npgsql.PostgresException ex)
        {
            students.Clear();
            return students;
        }
        return students;
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="email">TODO</param>
    /// <returns>TODO</returns>
    public Account GetAccount(string email)
    {
        var conn = new NpgsqlConnection(GetConnectionString());
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT email, password, role FROM Account WHERE email = @email", conn);
        cmd.Parameters.AddWithValue("email", email);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            if (!reader.IsDBNull(0))
            {
                return new Account(reader.GetString(0), reader.GetString(1), reader.GetString(2));
            }
        }
        return null;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="propertyName">TODO</param>
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /******************************
    * REPORT PAGE VARS & METHODS *
    *****************************/

    /// <summary>
    /// Adds report to the db
    /// </summary>
    /// <param name="report">The report to add</param>
    /// <returns>Error representing add status</returns>
    public AddReportError AddReport(ReportItem report)
    {
        // Add report to Reports table in the database
        try
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            var cmd = new NpgsqlCommand();
            cmd.Connection = conn;

            // Query Text
            cmd.CommandText = @"INSERT INTO Reports (reportName, fileName, reportStream, uploadedBy, uploadDate, dueDate, coordinatorEmails)
                                VALUES (@ReportName, @FileName, @ReportStream, @UploadedBy, @UploadDate, @DueDate, @CoordinatorEmails)";

            // Add parameters
            cmd.Parameters.AddWithValue("ReportName", report.ReportName);
            cmd.Parameters.AddWithValue("FileName", report.FileName);
            cmd.Parameters.AddWithValue("ReportStream", report.ReportStream);
            cmd.Parameters.AddWithValue("UploadedBy", report.UploadedBy);
            cmd.Parameters.AddWithValue("UploadDate", report.UploadDate);
            cmd.Parameters.AddWithValue("DueDate", report.DueDate);
            cmd.Parameters.AddWithValue("CoordinatorEmails", report.CoordinatorEmails);

            // Execute query
            cmd.ExecuteNonQuery();
        }
        catch (Npgsql.PostgresException pe)
        {
            Console.WriteLine("Error occured in database, {0}", pe);
            return AddReportError.DBAddError;
        }
        return AddReportError.NoError;
    }

    /// <summary>
    /// TODO: This has not yet been tested or implemented in the UI
    /// </summary>
    /// <param name="reportName">TODO</param>
    /// <returns>TODO</returns>
    public DeleteReportError DeleteReport(string reportName)
    {
        // Delete report from Reports table in the database
        try
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var cmd = new NpgsqlCommand();
            cmd.Connection = conn;

            // DELETE REPORT FROM ~~ REPORTS TABLE ~~
            cmd.CommandText = @"DELETE
                                FROM Reports
                                WHERE reportName = @ReportName";

            // Add parameters
            cmd.Parameters.AddWithValue("ReportName", reportName);

            // Execute query
            cmd.ExecuteNonQuery();


            // DELETE SUBMISSIONS FOR REPORT FROM ~~ SUBMISSIONS TABLE ~~
            cmd.CommandText = @"DELETE *
                                FROM ReportSubmission
                                WHERE reportName = @ReportName";

            // Add parameters
            cmd.Parameters.AddWithValue("ReportName", reportName);

            // Execute query
            cmd.ExecuteNonQuery();
            
        }
        catch (Npgsql.PostgresException pe)
        {
            Console.WriteLine("Error occured in database, {0}", pe);
            return DeleteReportError.DBDeleteError;
        }
        return DeleteReportError.NoError;
    }

    /// <summary>
    /// Adds submission from coordinator to the database
    /// </summary>
    /// <param name="reportSubmission">The submission to add to the database</param>
    /// <returns>Error representing the add status</returns>
    public AddReportSubmissionError AddReportSubmission(ReportSubmission reportSubmission)
    {
        // Add report submission to the ReportSubmissions table in the database
        try
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            var cmd = new NpgsqlCommand();
            cmd.Connection = conn;

            // Query Text
            cmd.CommandText = @"INSERT INTO ReportSubmission (fileName, submissionDate, reportName, uploadedBy, reportStream)
                                VALUES (@FileName, @SubmissionDate, @ReportName, @UploadedBy, @ReportStream)";

            // Add parameters
            cmd.Parameters.AddWithValue("FileName", reportSubmission.FileName);
            cmd.Parameters.AddWithValue("SubmissionDate", reportSubmission.SubmissionDate);
            cmd.Parameters.AddWithValue("ReportName", reportSubmission.ReportName);
            cmd.Parameters.AddWithValue("UploadedBy", reportSubmission.UploadedBy);
            cmd.Parameters.AddWithValue("ReportStream", reportSubmission.ReportStream);

            // Execute query
            cmd.ExecuteNonQuery();
        }
        catch (Npgsql.PostgresException pe)
        {
            Console.WriteLine("Error occured in database, {0}", pe);
            return AddReportSubmissionError.DBAddError;
        }
        return AddReportSubmissionError.NoError;
    }

    /// <summary>
    /// Returns the reports view for the DirectorAddReportDashboard
    /// </summary>
    /// <returns>Collection of reports</returns>
    public ObservableCollection<ReportItem> GetDirectorReports()
    {
        return GetAllReports();
    }

    /// <summary>
    /// Returns all reports in the database
    /// </summary>
    /// <returns>All reports in the database</returns>
    public ObservableCollection<ReportItem> GetAllReports()
    {
        ObservableCollection<ReportItem> reports = new();
        try
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            var cmd = new NpgsqlCommand();
            cmd.Connection = conn;

            // Query Text
            cmd.CommandText = @"SELECT *
                                FROM Reports";

            // Execute query
            cmd.ExecuteNonQuery();

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Read and contents of each line
                    string reportName = reader["reportName"].ToString();
                    string fileName = reader["fileName"].ToString();

                    //Stream reportStream = (Stream)reader["reportStream"];
                    byte[] reportStreamBytes = (byte[])reader["reportStream"];
                    Stream reportStream = new MemoryStream(reportStreamBytes);

                    string uploadedBy = reader["uploadedBy"].ToString();
                    DateTime uploadDate = (DateTime)reader["uploadDate"];
                    DateTime dueDate = (DateTime)reader["dueDate"];
                    string[] coordinatorEmails = (string[])reader["coordinatorEmails"];

                    // Get all submissions for the current report
                    var submissions = GetReportSubmissions(reportName);

                    // Create new report item and add to return collection
                    ReportItem report = new(reportName, fileName, reportStream, uploadedBy, uploadDate, dueDate, coordinatorEmails, submissions);
                    reports.Add(report);
                }
            }
        }
        catch (Npgsql.PostgresException pe)
        {
            Console.WriteLine("Error occured in database, {0}", pe);
            return null;
        }
        return reports;
    }

    /// <summary>
    /// Gets all reports assigned to the logged in coordinator
    /// </summary>
    /// <param name="userEmail">The logged in coordinator's email</param>
    /// <returns>TODO</returns>
    public ObservableCollection<ReportItem> GetCoordinatorReports(string userEmail)
    {
        // Collection of report items to be returned
        ObservableCollection<ReportItem> reports = new();

        // TODO: write description
        try
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            var cmd = new NpgsqlCommand();
            cmd.Connection = conn;

            // Director sees all reports
            if (Preferences.Get("user_type", "unknown") == "Director")
            {
                return GetAllReports();
            }

            // Query Text
            cmd.CommandText = @"SELECT *
                                FROM Reports
                                WHERE ARRAY_CONTAINS(coordinatorEmails, @UserEmail)"; // coordinatorEmails (varchar(255)[]) contains string userEmail

            // Add parameters
            cmd.Parameters.AddWithValue("UserEmail", userEmail);

            // Execute query
            cmd.ExecuteNonQuery();

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Read and contents of each line
                    string reportName = reader["reportName"].ToString();
                    string fileName = reader["fileName"].ToString();
                    Stream reportStream = (Stream)reader["reportStream"];
                    string uploadedBy = reader["uploadedBy"].ToString();
                    DateTime uploadDate = (DateTime)reader["uploadDate"];
                    DateTime dueDate = (DateTime)reader["dueDate"];
                    string[] coordinatorEmails = (string[])reader["coordinatorEmails"];

                    var submissions = GetReportSubmissions(reportName);

                    // Create new report item and add to return collection
                    ReportItem report = new(reportName, fileName, reportStream, uploadedBy, uploadDate, dueDate, coordinatorEmails, submissions);
                    reports.Add(report);
                }
            }
        }
        catch (Npgsql.PostgresException pe)
        {
            Console.WriteLine("Error occured in database, {0}", pe);
            return null;
        }
        return reports;
    }

    /// <summary>
    /// Gets the submissions for each report (this might be uneccesary code, need to refactor potentially)
    /// </summary>
    /// <param name="reportName">Name of the report to retrieve submissions for</param>
    /// <returns>Collection of reports</returns>
    public ObservableCollection<ReportSubmission> GetReportSubmissions(string reportName)
    {
        // Collection of report items to be returned
        ObservableCollection<ReportSubmission> submissions = new();

        try
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            var cmd = new NpgsqlCommand();
            cmd.Connection = conn;

            // Query Text
            cmd.CommandText = @"SELECT fileName, reportStream, uploadedBy, submissionDate
                            FROM ReportSubmission
                            WHERE reportname = @ReportName";

            // Execute query
            cmd.ExecuteNonQuery();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // Read and contents of each line
                string fileName = reader["fileName"].ToString();
                Stream reportStream = (Stream)reader["reportStream"];
                string uploadedBy = reader["uploadedBy"].ToString();
                DateTime submissionDate = (DateTime)reader["submissionDate"];

                // Create new report item and add to return collection
                ReportSubmission submission = new(fileName, reportStream, uploadedBy, submissionDate, reportName);
                submissions.Add(submission);
            }
        }
        catch (Npgsql.PostgresException pe)
        {
            Console.WriteLine("Error occured in database, {0}", pe);
            return null;
        }
        return submissions;
    }

    /// <summary>
    /// Searches for a coordinator in the DB using a given email
    /// </summary>
    /// <param name="email">The coordinator email to find</param>
    /// <returns>boolean true if found, false otherwise</returns>
    public bool FindCoordinatorByEmail(string email)
    {
        try
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var cmd = new NpgsqlCommand();
            cmd.Connection = conn;

            // Query Text with parameter
            cmd.CommandText = @"SELECT *
                                FROM ClinicalCoordinator
                                WHERE email = @Email";

            // Add parameter and set its value
            cmd.Parameters.AddWithValue("@Email", email);

            using (var reader = cmd.ExecuteReader())
            {
                // Check if any rows are returned
                if (reader.HasRows)
                {
                    // Coordinator found
                    return true;
                }
            }
        }
        catch (Npgsql.PostgresException pe)
        {
            Console.WriteLine("Error occurred in the database: {0}", pe);
            // Return false if there's an exception
            return false;
        }

        // Return false if no coordinator was found
        return false;
    }

    //static void CreateTables(string connString)
    //{
    //    using var conn = new NpgsqlConnection(connString); // a conn represents a connection to the database
    //    conn.Open(); // open the connection ... now we are connected!
    //    new NpgsqlCommand("CREATE TABLE IF NOT EXISTS ClinicalCoordinator (EmployeeID VARCHAR(25) PRIMARY KEY, FirstName VARCHAR(25), LastName VARCHAR(50), Email VARCHAR(150), Username VARCHAR(50), Password VARCHAR(50), PhoneNumber VARCHAR(25)", conn).ExecuteNonQuery();
    //    new NpgsqlCommand("CREATE TABLE IF NOT EXISTS Student (StudentID VARCHAR(25) PRIMARY KEY, FirstName VARCHAR(25), LastName VARCHAR(50), Email VARCHAR(150), Username VARCHAR(50), Password VARCHAR(50), PhoneNumber VARCHAR(25), Address VARCHAR(100)", conn).ExecuteNonQuery();
    //    new NpgsqlCommand("CREATE TABLE IF NOT EXISTS Clinic (ClinicID VARCHAR(25) PRIMARY KEY, ClinicName VARCHAR(100), PrimaryAddress VARCHAR(25), ContactPerson VARCHAR(50), ContactEmail VARCHAR(150), ContactPhone VARCHAR(50)", conn).ExecuteNonQuery();
    //    //new NpgsqlCommand("CREATE TABLE IF NOT EXISTS Clinic (ClinicID PRIMARY KEY, ClinicName  VARCHAR(100), PrimaryAddress VARCHAR(25), ContactPerson VARCHAR(50), ContactEmail VARCHAR(150), ContactPhone VARCHAR(50)", conn).ExecuteNonQuery();

    //}

    //public Clinical GetDashBoardClinicalInformation(string email)
    //{
    //    try
    //    {
    //        var conn = new NpgsqlConnection(GetConnectionString());
    //        conn.Open();
    //        using var cmd = new NpgsqlCommand("WITH TotalHours AS (SELECT cnl.studentemail, cnl.clinicalname, SUM(cnl.hoursworked) as total_hours FROM  clinical as cnl GROUP BY cnl.studentemail, cnl.clinicalname)SELECT  cnl.clinicalname,prc.name as preceptor_name,cn.clinicname, cnl.hoursworked as individual_hours,  th.total_hours FROM   clinical as cnl JOIN   preceptor as prc ON cnl.preceptoremail = prc.preceptoremail JOIN    clinic as cn ON prc.preceptoremail = cn.preceptoremail JOIN   TotalHours as th ON cnl.studentemail = th.studentemail AND cnl.clinicalname = th.clinicalname WHERE   cnl.studentemail = @email GROUP BY  cnl.clinicalname, prc.name, cn.clinicname, cnl.hoursworked, th.total_hours;", conn);
    //        cmd.Parameters.AddWithValue("email", email);
    //        using var reader = cmd.ExecuteReader();

    //        if (reader.Read())
    //        {
    //            if (!reader.IsDBNull(0))
    //            {
    //                return new Clinical(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4));
    //            }
    //        }
    //        return null;
    //    }
    //    catch (Exception ex)
    //    {

    //        throw;
    //    }

    //}

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="email">TODO</param>
    /// <returns>TODO</returns>
    public Clinical GetDashBoardClinicalInformation(string email)
    {
        try
        {
            var conn = new NpgsqlConnection(GetConnectionString());
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT studentemail, clinicalname, SUM(hoursworked) as total_hours FROM  clinical WHERE studentemail = @email GROUP BY studentemail, clinicalname", conn);
            cmd.Parameters.AddWithValue("email", email);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    return new Clinical(reader.GetString(0), reader.GetString(1), reader.GetDouble(2));
                }
            }
            return null;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public Clinical GetLatestClinicalSubmission(string email)
    {
        try
        {
            var conn = new NpgsqlConnection(GetConnectionString());
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT studentemail, clinicalname, hoursworked, dateworked, notes FROM  clinical WHERE studentemail = @email and clinicalid = (SELECT MAX(clinicalid) from Clinical) GROUP BY studentemail, clinicalname, hoursworked, dateworked, notes", conn);
            cmd.Parameters.AddWithValue("email", email);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    DateTime dateTimeValue = reader.GetDateTime(3);
                    string dateString = dateTimeValue.ToString("yyyy-MM-dd");
                    return new Clinical(reader.GetString(0), reader.GetString(1), reader.GetDouble(2), dateString, reader.GetString(4));
                }
            }
            return null;
        }
        catch (Exception ex)
        {

            throw;
        }
    }
}