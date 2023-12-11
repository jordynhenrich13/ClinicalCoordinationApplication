﻿using System;
using System.Collections.ObjectModel;

namespace ClinicalCoordinationApplication.Model
{
    public interface IDatabase
    {
        void SignIn(string email);
        CreateAccountError CreateStudentAccount(string email, string password, string firstName, string LastName);
        CreateAccountError CreateCoordinatorAccount(string email, string password, string firstName, string LastName);
        Account GetAccount(string email);
        Clinical GetDashBoardClinicalInformation(string email);
        ObservableCollection<Student> SelectAllStudents();
        ObservableCollection<Student> FindStudent(string search);
        ObservableCollection<Student> Students { get; }
        public Account GetUserType();
        public void DeleteProfile();
        AddWorkedHoursError AddHoursWorked(String clinical, DateTime dateTime, TimeSpan clinicalHoursWorked, string notes, string studentEmail);

    }
}

