﻿using System;
using System.ComponentModel;

namespace ClinicalCoordinationApplication
{
    [Serializable()]
    public class Clinical : INotifyPropertyChanged
    {
        public string clinicalName;
        private int clinicalNumber;
        private string description;
        public string name;
        public double hoursworked;
        public int total;
        public string clinicalsite;
        private int requiredHours;
        private int durationInWeeks;
        private Clinic clinic;
        private bool hasPreceptor;
        private Preceptor preceptor;
        private string studentEmail;
        private string clinicalStatus;
        private string bindname;
        public string dateWorked;
        public string notes;

        public int ClinicalNumber
        {
            get { return clinicalNumber; }
            set
            {
                clinicalNumber = value;
                OnPropertyChanged(nameof(clinicalNumber));
            }
        }

        public string ClinicalName
        {
            get { return clinicalName; }
            set
            {
                clinicalName = value;
                OnPropertyChanged(nameof(clinicalName));
            }
        }

        public string DateWorked
        {
            get { return dateWorked; }
            set
            {
                dateWorked = value;
                OnPropertyChanged(nameof(dateWorked));
            }
        }

        public double HoursWorked
        {
            get { return hoursworked; }
            set
            {
                hoursworked = value;
                OnPropertyChanged(nameof(hoursworked));
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(nameof(description));
            }
        }

        public int RequiredHours
        {
            get { return requiredHours; }
            set
            {
                requiredHours = value;
                OnPropertyChanged(nameof(requiredHours));
            }
        }

        public int DurationInWeeks
        {
            get { return durationInWeeks; }
            set
            {
                durationInWeeks = value;
                OnPropertyChanged(nameof(durationInWeeks));
            }
        }

        public Clinic Clinic
        {
            get { return clinic; }
            set
            {
                clinic = value;
                OnPropertyChanged(nameof(clinic));
            }
        }

        public bool HasPreceptor
        {
            get { return hasPreceptor; }
            set
            {
                hasPreceptor = value;
                OnPropertyChanged(nameof(hasPreceptor));
            }
        }

        public Preceptor Preceptor
        {
            get { return preceptor; }
            set
            {
                preceptor = value;
                OnPropertyChanged(nameof(preceptor));
            }
        }

        public Clinical(string clinicalName, int clinicalNumber, string description, int requiredHours, int durationInWeeks, Clinic clinic, bool hasPreceptor, Preceptor preceptor)
        {
            this.clinicalName = clinicalName;
            this.clinicalNumber = clinicalNumber;
            this.description = description;
            this.requiredHours = requiredHours;
            this.durationInWeeks = durationInWeeks;
            this.clinic = clinic;
            this.hasPreceptor = hasPreceptor;
            this.preceptor = preceptor;
        }

        public Clinical(string studentEmail)
        {
            this.studentEmail = studentEmail;
            clinicalStatus = "Not Started";
            bindname = "Adult Health";

        }
        public Clinical(string studentEmail, string clinicalName, double loghours, string dateWorked, string notes)
        {
            this.studentEmail = studentEmail;
            this.clinicalName = clinicalName;
            this.hoursworked = loghours;
            this.dateWorked = dateWorked;
            this.notes = notes;
        }

        public Clinical(string clinicalName, string name, string clinicalsite, double loghours, int total)
        {
            this.clinicalName = clinicalName;
            this.name = name;
            this.clinicalsite = clinicalsite;
            this.hoursworked = loghours;
            this.total = total;
        }

        public Clinical(string studentEmail, string clinicalName, double loghours)
        {
            this.studentEmail = studentEmail;
            this.clinicalName = clinicalName;
            this.hoursworked = loghours;
        }

       




        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
