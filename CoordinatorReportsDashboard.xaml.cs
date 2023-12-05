using System.Collections.ObjectModel;
using System.Threading;
using ClinicalCoordinationApplication.Model;
using ClinicalCoordinationApplication.Model.Reports;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using Font = Microsoft.Maui.Font;

namespace ClinicalCoordinationApplication;
public partial class CoordinatorReportsDashboard : ContentPage
{
    private readonly BusinessLogic bl;
    public ReportsView CoordinatorReports;

    public CoordinatorReportsDashboard()
    {
        InitializeComponent();
        bl = new(); // Assign business logic
        BindingContext = bl; // Set binding context

        // Initialize the collection view with reports assigned to the logged in coordinator
        CoordinatorReports = new(bl.GetCoordinatorReports(Preferences.Get("user_email", "Unknown")));

        // Set the reports as the binding context
        BindingContext = CoordinatorReports;
    }

    public async void UploadCompletedReportButtonClicked(object sender, EventArgs e)
    {
        // Get submission date immediately
        DateTime submissionDate = DateTime.UtcNow;

        // Get email of user who submitted the report
        //string submittedBy = bl.LoggedInUserName; // TODO, create email getter in DB
        string submittedBy = Preferences.Get("user_email", "Unknown"); // Retrieves user's email address from app preferences

        // Get report name of submission from xaml
        
        string reportName = "";
       

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
            bl.AddReportSubmission(result.FileName, stream, submissionDate, submittedBy, reportName);
        } else
        {
            // Show toast that informs user that an action was cancelled
            _ = bl.ShowCancellationToast("Submission");
            return;
        }
    }
}