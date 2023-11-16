﻿using ClinicalCoordinationApplication.Model;
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


namespace ClinicalCoordinationApplication;
public class Database : IDatabase
{
    private string connString;
    // Constructor initializes and sets up the database.
    // It also ensures that the file exists or creates it if not.

    public static string userId { get; set; }
    ObservableCollection<Student> students = new();

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


    public string GetUserType()
    {
        if (userId != null)
        {
            Student studentFound = QueryStudentData(userId);
            Coordinator coordinatorFound = QueryCoordinatorData(userId);

            if (studentFound == null)
            {
                if (coordinatorFound == null)
                {
                    return "";
                }

                if (coordinatorFound.FirstName == "Erika")
                {
                    return "Director";
                }

                return "Coordinator";
            }
            else
            {
                return "Student";
            }
        }
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

    public Student StudentSignIn(string email, string password)
    {
        var conn = new NpgsqlConnection(connString);
        conn.Open();
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "SELECT firstname, lastname, email, password FROM Student WHERE Email = @email AND Password = @Password";
        cmd.Parameters.AddWithValue("Email", email);
        cmd.Parameters.AddWithValue("Password", password);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            userId = reader.GetString(2);
            // Set the student name property
            StudentName = $"{reader.GetString(0)} {reader.GetString(1)}";
            return new Student(reader.GetString(0), reader.GetString(1), reader.GetString(2));
        }
        else
        {
            Student nullStudent = null;
            return nullStudent;
        }
    }

    public Coordinator CoordinatorSignIn(string email, string password)
    {

        var conn = new NpgsqlConnection(connString);
        conn.Open();
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "SELECT FirstName, LastName, Email, Password FROM ClinicalCoordinator WHERE Email = @Email AND Password = @Password";
        cmd.Parameters.AddWithValue("Email", email);
        cmd.Parameters.AddWithValue("Password", password);

        using var reader = cmd.ExecuteReader(); // used for SELECT statement, returns a forward-only traversable object
        if (reader.Read())
        { // there should be only one row, so we don't need a while loop TODO: Sanity check
            userId = reader.GetString(2);
            return new Coordinator(reader.GetString(0), reader.GetString(1), reader.GetString(2));
        }
        else
        {
            Coordinator nullCoordinator = null;
            return nullCoordinator;
        }
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
            cmd.CommandText = "INSERT INTO Student (FirstName, Lastname, Email, Password) VALUES (@FirstName, @Lastname, @Email, @Password)";
            cmd.Parameters.AddWithValue("FirstName", firstName);
            cmd.Parameters.AddWithValue("lastName", LastName);
            cmd.Parameters.AddWithValue("Email", email);
            cmd.Parameters.AddWithValue("Password", password);
            cmd.ExecuteNonQuery(); // used for INSERT, UPDATE & DELETE statements - returns # of affected rows 
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
            cmd.CommandText = "INSERT INTO ClinicalCoordinator (FirstName, Lastname, Email, Password) VALUES (@FirstName, @Lastname, @Email, @Password)";
            cmd.Parameters.AddWithValue("FirstName", firstName);
            cmd.Parameters.AddWithValue("lastName", LastName);
            cmd.Parameters.AddWithValue("Email", email);
            cmd.Parameters.AddWithValue("Password", password);
            cmd.ExecuteNonQuery(); // used for INSERT, UPDATE & DELETE statements - returns # of affected rows 
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
    public ObservableCollection<Student> FindStudent(string lastName, string firstName)
    {
        try
        {
            students.Clear();
            var conn = new NpgsqlConnection(GetConnectionString());
            conn.Open();
            if (firstName.CompareTo("") == 0)
            {
                using var cmd = new NpgsqlCommand("SELECT firstname, lastname, email FROM Student WHERE firstname LIKE @search% OR lastname LIKE @search%", conn);
                cmd.Parameters.AddWithValue("search", lastName);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    String dbfirstName = reader.GetString(0);
                    String dblastName = reader.GetString(1);
                    String email = reader.GetString(2);
                    Student student = new Student(dbfirstName, dblastName, email);
                    students.Add(student);
                }
            }
            else
            {
                using var cmd = new NpgsqlCommand("SELECT firstname, lastname, email FROM Student WHERE firstname LIKE @first% AND lastname LIKE @last%", conn);
                cmd.Parameters.AddWithValue("last", lastName);
                cmd.Parameters.AddWithValue("first", firstName);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    String dbfirstName = reader.GetString(0);
                    String dblastName = reader.GetString(1);
                    String email = reader.GetString(2);
                    Student student = new Student(dbfirstName, dblastName, email);
                    students.Add(student);
                }
            }
        }
        catch (Npgsql.PostgresException ex)
        {
            students.Clear();
            return students;
        }
        return students;
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

