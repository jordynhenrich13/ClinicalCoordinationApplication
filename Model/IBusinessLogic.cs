﻿using System;
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
        CreateAccountError CreateAccount(string email, string password, string firstName, string lastName);
        EditAccountError EditAccount();
        AddWorkedHoursError AddWorkedHours();
        EditWorkedHoursError EditWorkedHours();
        DeleteWorkedHoursError DeleteWorkedHours();
        UpdateClinicalInfoError UpdateClinicalInfo();
        AddClinicalNoteError AddClinicalNote();
        EditClinicalNoteError EditClinicalNote();
        DeleteClinicalNoteError DeleteClinicalNote();
        FindPreviousClinicsError FindPreviousClinics();
        FindNewClinicError FindNewClinic();
    }
}

