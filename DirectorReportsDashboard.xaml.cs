
using ClinicalCoordinationApplication.Model;
using ClinicalCoordinationApplication.Model.Reports;
using CommunityToolkit.Maui.Views;

namespace ClinicalCoordinationApplication;

public partial class DirectorReportsDashboard : ContentPage
{
    private readonly BusinessLogic bl;

    public ReportsView AllReports;
    
    public DirectorReportsDashboard()
	{
		InitializeComponent();
        bl = new(); // Initialize BL

        AllReports = new(bl.GetDirectorReports());

        BindingContext = AllReports;
    }

    /// <summary>
    /// Adds a report to the database.
    /// </summary>
    /// <param name="sender">TODO: Fill this out</param>
    /// <param name="e">TODO: Fill this out</param>
    public void AddReportClicked(object sender, EventArgs e)
    {
        var popup = new DirectorAddReportPopup();
        this.ShowPopup(popup);
    }

    /// <summary>
    /// Downloads the submissions for the report item clicked.
    /// </summary>
    /// <param name="sender">TODO: Fill this out</param>
    /// <param name="e">TODO: Fill this out</param>
    public void DownloadSubmissionsButtonClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        ReportItem report = (ReportItem)button.Parent.BindingContext;
        string reportName = report.ReportName;

        var reportSubmissions = bl.GetReportSubmissions(reportName)!;

        if (reportSubmissions != null)
        {


            foreach (var submission in reportSubmissions)
            {
                string fileName = $"Report_{Guid.NewGuid()}.pdf";

                // Combine the file path with the device's documents directory
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);

                // Write the PDF stream to the file
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    submission.ReportStream.CopyTo(fileStream);
                }
            }

            // Display toast saying action was successful
            bl.ShowDownloadToast($"Submissions for {reportName}");
        } else
        {
            _ = bl.ShowNoSubmissionsToast();
        }
    }
        
}