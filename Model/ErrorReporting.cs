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
        InvalidEmail,
        InvalidPassword,
        NoError
    }
    public enum EditAccountError
    {

        NoError
    }
    public enum AddWorkedHoursError
    {

        NoError
    }
    public enum EditWorkedHoursError
    {

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
}
