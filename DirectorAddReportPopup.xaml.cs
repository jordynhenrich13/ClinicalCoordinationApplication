using CommunityToolkit.Maui.Views;
using ClinicalCoordinationApplication.Model;
namespace ClinicalCoordinationApplication;

public partial class DirectorAddReportPopup : Popup
{
    readonly BusinessLogic bl;
    private string selectedFileName;
    private Stream reportStream;

    public DateTime SelectedDate { get; set; }

    public DirectorAddReportPopup()
	{
        InitializeComponent();

        // Initialize business logic
        bl = new();

        // Bind the date
        DueDate.SetBinding(DatePicker.DateProperty, nameof(SelectedDate));
    }

    public async void UploadFileButtonClicked(object sender, EventArgs e)
    {
        // Report upload will only accept the following file types
        var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            {DevicePlatform.Android, new[]
                {
                    "application/pdf",
                    //"image/png",
                    //"image/jpeg",
                    //"image/jpg"
                }
            }
        });

        // Store user selected file as result
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Select an image to upload",
            FileTypes = customFileType
        });

        // Setup stream to read file contents
        if (result == null)
        {
            return;
        }
        // Assign file name as variable
        selectedFileName = result.FileName;

        // Update the upload button text with the selected file name
        uploadButton.Text = $"Upload File: {selectedFileName}";

        // Open stream for file
        Stream stream = await result.OpenReadAsync();

        // Save the uploaded report
        reportStream = stream;
    }

    public void OnSubmitButtonClicked(object sender, EventArgs e)
    {
        // Get report name, due date and who to send reports to from ui
        string reportName = ReportName.Text;
        DateTime dueDate = SelectedDate;
        string[] sendTo = SendTo.Text.Split(',');

        // Get logged in user's email from preferences
        //string uploadedBy = bl.LoggedInUserName;
        string uploadedBy = Preferences.Get("user_email", "Unknown");

        // Set upload date time to now
        DateTime uploadDate = DateTime.UtcNow;

        // Check if sending to existing coordinators
        //if (validateSendTo(sendTo) == false)
        //{
        //    Console.WriteLine('Coordinator listed in send to was not found');
        //    return;
        //}
        
        // Add report to observable collection
        bl.AddReport(reportName,
                     selectedFileName,
                     reportStream, 
                     uploadedBy,
                     uploadDate,
                     dueDate,
                     sendTo);
    }

    public bool validateSendTo(string[] recipients)
    {
        foreach(string recipient in recipients) {
            bool result = bl.FindCoordinatorByEmail(recipient);
            if (result == false) return false;
        }
        return true;
    }

    public void OnDeleteReportButtonClicked(object sender, EventArgs e)
    {
        // TODO: Implement this
        throw new NotImplementedException();
    }

    public async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        // Reset fields
        ReportName.Text = "";
        dueDate.Text = "";
        SendTo.Text = "";
        reportStream = null;

        // Close popup
        await CloseAsync();
    }
}
