using ClinicalCoordinationApplication.Model;
using Microsoft.Maui.ApplicationModel.Communication;
//using Intents;
//using Intents;
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
//using Windows.Networking;


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
            cmd.Parameters.AddWithValue("PreceptorEmail", preceptor.Email);
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

    public PreceptorViewModel LoadPreceptorInformation(string studentEmail, int clinicalPageNumber)
    {
        using var conn = new NpgsqlConnection(connString);
        conn.Open();

        using var cmdInsert = new NpgsqlCommand();
        cmdInsert.Connection = conn;

        // Modify the query to insert emails from Student table into Preceptor table
        cmdInsert.CommandText = "INSERT INTO Preceptor (studentemail) SELECT email FROM Student";

        // Execute the insertion query
        var numInserted = cmdInsert.ExecuteNonQuery();

        // Check if the insertion was successful
        if (numInserted > 0)
        {
            Console.WriteLine($"Inserted {numInserted} student emails into the Preceptor table");
        }
        else
        {
            Console.WriteLine("Failed to insert student emails into the Preceptor table");
        }

        // Rest of your code to retrieve preceptor information...

        using var cmdRetrieve = new NpgsqlCommand();
        cmdRetrieve.Connection = conn;
        cmdRetrieve.CommandText = "SELECT Title, Name, Facility, Phone, PreceptorEmail " +
                                  "FROM Preceptor " +
                                  "WHERE studentemail = @StudentEmail";
        cmdRetrieve.Parameters.AddWithValue("StudentEmail", NpgsqlDbType.Varchar, studentEmail);

        using var reader = cmdRetrieve.ExecuteReader();

        if (reader.Read())
        {
            // Assuming PreceptorViewModel has a constructor that takes these values
            return new PreceptorViewModel
            {
                Title = reader.GetString(0),
                Name = reader.GetString(1),
                Facility = reader.GetString(2),
                Phone = reader.GetString(3),
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

