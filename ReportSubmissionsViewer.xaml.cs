using System.Collections.ObjectModel;
using ClinicalCoordinationApplication.Model;
using ClinicalCoordinationApplication.Model.Reports;

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

    /// <summary>
    /// Downloads a copy of the report to the user's device
    /// </summary>
    /// <param name="sender">Object sending the request</param>
    /// <param name="e">Event arguments for the request</param>
    public void DownloadReportButtonTapped(object sender, EventArgs e)
    {
        // TODO
    }
}
