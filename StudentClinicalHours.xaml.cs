using ClinicalCoordinationApplication.Model;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace ClinicalCoordinationApplication
{
    public partial class StudentClinicalHours : ContentPage
    {
        BusinessLogic businessLogic = new BusinessLogic();
        Account obja = new Account(null, null, null);

        public ObservableCollection<Clinical> ClinicalList { get; set; }

        public StudentClinicalHours()
        {
            InitializeComponent();
            obja = businessLogic.GetUserAccount();

            // Retrieve student clinical hours
            ClinicalList = businessLogic.GetStudentClinicalHours(obja.Email);

            // Set the data context for binding
            BindingContext = this;
        }
    }
}
