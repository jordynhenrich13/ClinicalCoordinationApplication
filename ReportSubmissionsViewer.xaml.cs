using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using ClinicalCoordinationApplication.Model;
using ClinicalCoordinationApplication.Model.Reports;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;

namespace ClinicalCoordinationApplication;

/// <summary>
/// Displays all submissions for a selected report
/// </summary>
public partial class ReportSubmissionsViewer : ContentPage
{
    private readonly BusinessLogic bl;

    public ObservableCollection<ReportSubmission> Submissions;

    /// <summary>
    /// Constructs a view of all submissions for the selected report
    /// </summary>
    /// <param name="reportName">Report to retrieve submissions for</param>
    public ReportSubmissionsViewer(string reportName)
    {
        // Initialize Business Logic
        bl = new();

        // Retrieve submissions for the report selected
        Submissions = bl.GetReportSubmissions(reportName);

        InitializeComponent();

        Viewer.Title = reportName + " Submissions";

        BindingContext = Submissions;
    }
}
