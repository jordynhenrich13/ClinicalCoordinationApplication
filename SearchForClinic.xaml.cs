using System.Collections.ObjectModel;
namespace ClinicalCoordinationApplication;

/// <summary>
/// Olivia Ozbaki
/// </summary>
public partial class SearchForClinic : ContentPage
{
    readonly ObservableCollection<Clinic> QueriedClinics = new();

    public SearchForClinic()
    {
        InitializeComponent();
        QueriedClinics.Add(new Clinic("Aurora", 10));
        QueriedClinics.Add(new Clinic("Thedacare", 20));
        QueriedClinics.Add(new Clinic("SSM", 30));
        ClinicsFoundList.ItemsSource = QueriedClinics;
    }

}