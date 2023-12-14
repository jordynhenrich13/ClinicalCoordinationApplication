using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using AndroidX.Lifecycle;
using AndroidX.Navigation;
using ClinicalCoordinationApplication.Model;
using ClinicalCoordinationApplication.Model.Reports;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui;
using static Android.Icu.Text.AlphabeticIndex;
using Font = Microsoft.Maui.Font;

namespace ClinicalCoordinationApplication;
public partial class CoordinatorReportsDashboard : ContentPage
{
    private readonly BusinessLogic bl;
    public ReportsView CoordinatorReports;
    private readonly IFileSaver fileSaver;

    public CoordinatorReportsDashboard(IFileSaver fileSaver)
    {
        InitializeComponent();
        bl = new(); // Assign business logic
        BindingContext = bl; // Set binding context

        // Initialize the collection view with reports assigned to the logged in coordinator
        CoordinatorReports = new(bl.GetCoordinatorReports(Preferences.Get("user_email", "Unknown")));

        // Set the reports as the binding context
        BindingContext = CoordinatorReports;

        this.fileSaver = fileSaver;
    }


    public async void UploadCompletedReportButtonClicked(object sender, EventArgs e)
    {
       
            // Find the reportName label by name
            if (sender is Button button && button.CommandParameter is string reportName)
            {
                // Access the Text property of the label
                string _reportName = reportName;

                // Use the reportName as needed
                Console.WriteLine("reportName = " + _reportName);

                // Get submission date immediately
                DateTime submissionDate = DateTime.UtcNow;

                // Get email of user who submitted the report
                string submittedBy = Preferences.Get("user_email", "Unknown"); // Retrieves user's email address from app preferences

                // Report upload will only accept the following file types
                var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        {DevicePlatform.Android, new[]
                            {
                                "application/pdf",
                                "image/png",
                                "image/jpeg",
                                "image/jpg"
                            }
                        }
                    });

                // Store user selected file as result
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select an image to upload",
                    FileTypes = customFileType
                });

                // Do not continue if user does not make a selection
                if (result == null)
                {
                    // Show toast that informs user that an action was cancelled
                    _ = bl.ShowCancellationToast("Submission");
                    return;
                }

                // Open stream for file
                Stream stream = await result.OpenReadAsync();

                // Confirm file upload
                bool continueSubmission = await DisplayAlert("Please confirm report", $"You have chosen to submit {result.FileName}. Is that correct?", "Yes", "No");


                // Submit report with details from upload context
                if (continueSubmission)
                {
                    // Add submission to view model / observable collection
                    ReportSubmission submission = new(result.FileName, stream, submittedBy, submissionDate, _reportName);
                    CoordinatorReports.Submissions.Add(submission);

                    // Add report submission to DB
                    bl.AddReportSubmission(submission);
                }
                else
                {
                    // Show toast that informs user that an action was cancelled
                    _ = bl.ShowCancellationToast("Submission");
                    return;
                }

            }
        }

    public void OpenSubmissionsButtonTapped(object sender, EventArgs e) {
        if (sender is Button button && button.CommandParameter is string reportName)
        {
            Navigation.PushAsync(new ReportSubmissionsViewer(reportName));
        }
    }

    /// <summary>
    /// Downloads a copy of the report to the user's device
    /// </summary>
    /// <param name="sender">Object sending the request</param>
    /// <param name="e">Event arguments for the request</param>
    public async void DownloadReportButtonTapped(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is string reportName)
        {
            CancellationToken cancellationToken = new();

            ReportItem report = CoordinatorReports.ReportItems.FirstOrDefault(item => item.ReportName == reportName);

            var fileSaverResult = await fileSaver.SaveAsync($"{report.ReportName}_{report.FileName}", report.ReportStream, cancellationToken);

            fileSaverResult.EnsureSuccess();
            await Toast.Make($"File is saved: {fileSaverResult.FilePath}").Show(cancellationToken);
        }
    }

    void DownloadSubmissionsButton_Clicked(System.Object sender, System.EventArgs e)
    {
    }

    void DownloadSubmissionsButton_Clicked_1(System.Object sender, System.EventArgs e)
    {
    }
}
