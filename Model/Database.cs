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
using static Android.Provider.ContactsContract.CommonDataKinds;


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

    public string GetUserType()
    {
        string role = null;
        Account account = GetAccount(userId);
        if (account != null)
        {
            role = account.Role;
        }
        return role;
    }

    public void SavePreceptorToDatabase(PreceptorViewModel preceptor)
    {
        try
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO Preceptor (Title, Name, Facility, Email, Phone) " +
                              "VALUES (@Title, @Name, @Facility, @Email, @Phone)";

            cmd.Parameters.AddWithValue("Title", preceptor.Title);
            cmd.Parameters.AddWithValue("Name", preceptor.Name);
            cmd.Parameters.AddWithValue("Facility", preceptor.Facility);
            cmd.Parameters.AddWithValue("Email", preceptor.Email);
            cmd.Parameters.AddWithValue("Phone", preceptor.Phone);

            var numAffected = cmd.ExecuteNonQuery();

            // You may want to check numAffected to ensure the data was successfully inserted
            if (numAffected > 0)
            {
                Console.WriteLine("Preceptor information saved to the database");
            }
            else
            {
                Console.WriteLine("Failed to save preceptor information to the database");
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions, log errors, etc.
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public PreceptorViewModel LoadPreceptorInformation(string studentEmail)
    {
        using var conn = new NpgsqlConnection(connString);
        conn.Open();

        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "SELECT Title, Name, Facility, Email, Phone FROM Preceptor WHERE StudentEmail = @StudentEmail";
        cmd.Parameters.AddWithValue("StudentEmail", NpgsqlDbType.Varchar, studentEmail);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            // Assuming PreceptorViewModel has a constructor that takes these values
            return new PreceptorViewModel
            {
                Title = reader.GetString(0),
                Name = reader.GetString(1),
                Facility = reader.GetString(2),
                Email = reader.GetString(3),
                Phone = reader.GetString(4)
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

    public AddWorkedHoursError AddHoursWorked(String clinical, DateTime dateWorked, TimeSpan clinicalHoursWorked, string notes)
    {
        try
        {
            int primaryKey = 003;
            string studentEmail = "henrij13@uwosh.edu";
            using var conn = new NpgsqlConnection(connString); // conn, short for connection, is a connection to the database
            conn.Open(); // open the connection ... now we are connected!
            var cmd = new NpgsqlCommand(); // create the sql commaned
            cmd.Connection = conn; // commands need a connection, an actual command to execute
            cmd.CommandText = "INSERT INTO clinical (clinicalID, studentemail, clinicalname, dateWorked, hoursworked, notes) " +
                " VALUES (@primaryKey, @studentEmail, @clinical, @dateWorked, @clinicalHoursWorked, @notes)";
            cmd.Parameters.AddWithValue("primaryKey", primaryKey);
            cmd.Parameters.AddWithValue("studentEmail", studentEmail);
            cmd.Parameters.AddWithValue("clinical", clinical);
            cmd.Parameters.AddWithValue("dateWorked", dateWorked.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("clinicalHoursWorked", (int)clinicalHoursWorked.TotalHours);
            cmd.Parameters.AddWithValue("notes", notes);
            var numAffected = cmd.ExecuteNonQuery();

            return AddWorkedHoursError.NoError;
        } catch (Exception ex)
        {
            return AddWorkedHoursError.InvalidNumber;
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

    /// <summary>
    /// Used to alter profiles for debugging purposes
    /// </summary>
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


    public CreateAccountError CreateStudentAccount(string email, string password, string firstName, string LastName)
    {
        try
        {
            using var conn = new NpgsqlConnection(connString); // conn, short for connection, is a connection to the database

            conn.Open(); // open the connection ... now we are connected!
            var cmd = new NpgsqlCommand(); // create the sql commaned
            cmd.Connection = conn; // commands need a connection, an actual command to execute
            cmd.CommandText = "INSERT INTO Student (FirstName, Lastname, Email) VALUES (@FirstName, @Lastname, @Email)";
            cmd.Parameters.AddWithValue("FirstName", firstName);
            cmd.Parameters.AddWithValue("lastName", LastName);
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
            return CreateAccountError.InvalidEmail;
        }
        return CreateAccountError.NoError;
    }

    public CreateAccountError CreateCoordinatorAccount(string email, string password, string firstName, string LastName)
    {
        try
        {
            using var conn = new NpgsqlConnection(connString); // conn, short for connection, is a connection to the database

            conn.Open(); // open the connection ... now we are connected!
            var cmd = new NpgsqlCommand(); // create the sql commaned
            cmd.Connection = conn; // commands need a connection, an actual command to execute
            cmd.CommandText = "INSERT INTO ClinicalCoordinator (FirstName, Lastname, Email) VALUES (@FirstName, @Lastname, @Email)";
            cmd.Parameters.AddWithValue("FirstName", firstName);
            cmd.Parameters.AddWithValue("lastName", LastName);
            cmd.Parameters.AddWithValue("Email", email);
            cmd.ExecuteNonQuery(); // used for INSERT, UPDATE & DELETE statements - returns # of affected rows 

            using var conn2 = new NpgsqlConnection(connString);
            conn2.Open();
            var cmd2 = new NpgsqlCommand();
            cmd2.Connection = conn2;
            cmd2.CommandText = "INSERT INTO Account (email, password, role) VALUES (@email, @password, @role)";
            cmd2.Parameters.AddWithValue("email", email);
            cmd2.Parameters.AddWithValue("password", password);
            cmd2.Parameters.AddWithValue("role", "Coordinator");
            cmd2.ExecuteNonQuery();
        }
        catch (Npgsql.PostgresException pe)
        {
            Console.WriteLine("Insert failed, {0}", pe);
            return CreateAccountError.InvalidEmail;
        }
        return CreateAccountError.NoError;
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
    public ObservableCollection<Student> FindStudent(string search)
    {
        try
        {
            students.Clear();
            var conn = new NpgsqlConnection(GetConnectionString());
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT firstname, lastname, email FROM Student WHERE firstname LIKE @search + '%' OR lastname LIKE @search + '%'", conn);
            cmd.Parameters.AddWithValue("search", search);
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
            var cmd = new NpgsqlCommand();
            cmd.Connection = conn;

            // Query Text
            cmd.CommandText = @"DELETE *
                                FROM Reports
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
    /// TODO: This has not yet been tested or implemented in the UI
    /// </summary>
    /// <param name="reportSubmission">TODO</param>
    /// <returns>TODO</returns>
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
    /// <returns></returns>
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
            cmd.CommandText = @"SELECT *
                                FROM reportsubmission
                                WHERE reportName = @ReportName";

            // Execute query
            cmd.ExecuteNonQuery();

            using (var reader = cmd.ExecuteReader())
            {
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
}