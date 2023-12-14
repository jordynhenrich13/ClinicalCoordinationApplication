namespace ClinicalCoordinationApplication;
using ClinicalCoordinationApplication.Model;

// Partial class for the Clinical2 page
public partial class Clinical2 : ContentPage
{
    // Database instance for handling data operations
    public Database database;

    // View model for preceptor information
    private PreceptorViewModel preceptorViewModel;

    // Clinical page number
    public int clinicalPageNumber;

    // Constructor for Clinical2 page
    public Clinical2(Student selectedStudent)
    {
        InitializeComponent();

        // Initialize the preceptor view model and set the selected student
        preceptorViewModel = new PreceptorViewModel();
        preceptorViewModel.SelectedStudent = selectedStudent;
        BindingContext = preceptorViewModel;

        // Initialize the database
        database = new Database();

        // Attach an event handler to execute when the page appears
        this.Appearing += (sender, e) => LoadPreceptorInformation(selectedStudent);
    }

    // Event handler for the side menu button click
    private void SideMenuButton_Clicked(object sender, EventArgs e)
    {
        // Handle the side menu button click
        // You can add your code to open the side menu or perform any other action here
    }

    // Event handler for the profile icon click
    private void ProfileIcon_Clicked(object sender, EventArgs e)
    {
        // Handle the profile icon button click
        // You can add your code to navigate to the user's profile or perform any other action here
    }

    // Event handler for the back button click (Dashboard button)
    private void OnBackButtonClicked(object sender, EventArgs e)
    {
        // Check if there are pages to navigate back to
        if (Navigation.NavigationStack.Count > 1)
        {
            // Use PopAsync to navigate back to the previous page
            Navigation.PopAsync();
        }
    }

    // Event handler for checkbox state change
    private void OnCheckBoxChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            // If the checkbox is checked, show the PreceptorInfo section
            PreceptorInfo.IsVisible = true;
        }
        else
        {
            // If the checkbox is not checked, hide the PreceptorInfo section
            PreceptorInfo.IsVisible = false;
        }
    }

    // Method to load preceptor information
    private void LoadPreceptorInformation(Student selectedStudent)
    {
        // Set the clinical page number
        clinicalPageNumber = 2;

        // Load preceptor information based on the currently signed-in student's email
        var loadedPreceptor = database.LoadPreceptorInformation(selectedStudent.Email, clinicalPageNumber);

        if (loadedPreceptor != null)
        {
            // Update the BindingContext with the loaded preceptor information
            preceptorViewModel.Title = loadedPreceptor.Title;
            preceptorViewModel.Name = loadedPreceptor.Name;
            preceptorViewModel.Facility = loadedPreceptor.Facility;
            preceptorViewModel.PreceptorEmail = loadedPreceptor.PreceptorEmail;
            preceptorViewModel.Phone = loadedPreceptor.Phone;
        }
    }

    // Event handler to save preceptor information
    private async void SavePreceptorInformation(object sender, EventArgs e)
    {
        if (PreceptorInfo.IsVisible)
        {
            // Validate if required fields are filled
            if (string.IsNullOrWhiteSpace(preceptorViewModel.Title) || string.IsNullOrWhiteSpace(preceptorViewModel.Name))
            {
                // Handle validation error, show a message, etc.
                return;
            }

            // Call the method in Database.cs to save preceptor information to the database
            // Replace this with the actual method in your Database class
            database.SavePreceptorToDatabase(preceptorViewModel);

            // Update the LastUpdateLabel with the current timestamp
            LastUpdateLabel.Text = $"Last Updated: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";

            // Display a success message
            await DisplayAlert("Success", "Saved Preceptor", "OK");
        }
    }
}

