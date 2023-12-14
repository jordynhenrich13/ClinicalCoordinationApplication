
using System.Text;
using ClinicalCoordinationApplication.Model;
using ClinicalCoordinationApplication.Model.Reports;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui.Views;

namespace ClinicalCoordinationApplication;

/// <summary>
/// Allows the director to manage reports. Add, delete, and download functionality only. 
/// </summary>
public partial class DirectorReportsDashboard : ContentPage
{
    private readonly BusinessLogic bl;
    public ReportsView AllReports;
    private readonly IFileSaver fileSaver;

    public DirectorReportsDashboard(IFileSaver fileSaver)
	{
		InitializeComponent();
        bl = new(); // Initialize BL

        AllReports = new(bl.GetDirectorReports());

        BindingContext = AllReports;

        this.fileSaver = fileSaver;
    }

    /// <summary>
    /// Adds a report to the database.
    /// </summary>
    public void AddReportClicked(object sender, EventArgs e)
    {
        var popup = new DirectorAddReportPopup();
        this.ShowPopup(popup);
    }

    /// <summary>
    /// Downloads the submissions for the report item clicked.
    /// </summary>
    public void OpenSubmissionsButtonTapped(object sender, EventArgs e)
    {
        // Get name of the report
        if (sender is Button button && button.CommandParameter is string reportName)
        {
            Navigation.PushAsync(new ReportSubmissionsViewer(reportName));
        }
    }


    /// <summary>
    /// Delete a report
    /// </summary>
    public async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is string reportName)
        {
            bool result = await DisplayAlert($"Delete {reportName}", "This action is permanent, would you like to continue?", "YES", "NO");

            if (result)
            {
                bl.DeleteReport(reportName);
                AllReports.ReportItems = bl.GetDirectorReports();
            }
            return;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public async void DownloadSubmissionsButtonClicked(object sender, EventArgs e)
    {
        CancellationToken cancellationToken = new();

        if (sender is Button button && button.CommandParameter is string reportName)
        {
            //Get all submissions for the report
            var reportSubmissions = bl.GetReportSubmissions(reportName);

            if (reportSubmissions == null || reportSubmissions.Count < 1)
            {
                await Toast.Make($"No submissions found").Show(cancellationToken);
                return;
            }

            // Download each submission to the user's device
            foreach (var submission in reportSubmissions)
            {
                var fileName = $"{submission.ReportName}_{submission.FileName}.txt";
                var fileSaverResult = await fileSaver.SaveAsync(fileName, submission.ReportStream, cancellationToken);

                fileSaverResult.EnsureSuccess();
                await Toast.Make($"File is saved: {fileSaverResult.FilePath}").Show(cancellationToken);
            } 
        }
    }

}