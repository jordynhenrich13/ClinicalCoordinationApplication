﻿using System;
namespace ClinicalCoordinationApplication.Model
{
    public enum SignInError
    {
        InvalidEmailOrPassword,
        NoError
    }
    public enum CreateAccountError
    {
        InvalidFirstName,
        InvalidLastName,
        EmailAlreadyUsed,
        InvalidEmail,
        InvalidPassword,
        DBError,
        NoError
    }
    public enum EditAccountError
    {
        InvalidFirstName,
        InvalidLastName,
        EmailAlreadyUsed,
        InvalidEmail,
        DBError,
        NoError
    }
    public enum AddWorkedHoursError
    {
        InvalidNumber,
        NoError
    }
    public enum EditWorkedHoursError
    {
        InvalidNumber,
        NoError
    }
    public enum DeleteWorkedHoursError
    {

        NoError
    }
    public enum UpdateClinicalInfoError
    {

        NoError
    }
    public enum AddClinicalNoteError
    {

        NoError
    }
    public enum EditClinicalNoteError
    {

        NoError
    }
    public enum DeleteClinicalNoteError
    {

        NoError
    }
    public enum FindPreviousClinicsError
    {

        NoError
    }

    public enum FindNewClinicError
    {

        NoError
    }

    public enum FindStudentError
    {
        SearchTooLong,
        InvalidChar,
        NoError
    }
    public enum AddCoordinatorError
    {
        InvalidEmail,
        DBError,
        NoError
    }

    public enum AddReportError
    {
        CreateReportItemError,
        DuplicateReportNameError,
        CollectionAddError,
        DBAddError,
        NoError
    }

    public enum DeleteReportError
    {
        DBDeleteError,
        NoError
    }

    public enum AddReportSubmissionError
    {
        CreateReportSubmissionError,
        CollectionAddError,
        DBAddError,
        NoError
    }
}

