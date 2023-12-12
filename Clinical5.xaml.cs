namespace ClinicalCoordinationApplication;

public partial class Clinical5 : ContentPage
{
    public Database database;
    public int clinicalPageNumber;
    private PreceptorViewModel preceptorViewModel;
    public Clinical5()
    {
        InitializeComponent();

        // Create an instance of PreceptorViewModel and set it as the BindingContext
        preceptorViewModel = new PreceptorViewModel();
        BindingContext = preceptorViewModel;

        database = new Database();


        // Load preceptor information when the page appears
        this.Appearing += (sender, e) => LoadPreceptorInformation();
    }
    private void SideMenuButton_Clicked(object sender, EventArgs e)
    {
        // Handle the side menu button click
        // You can add your code to open the side menu or perform any other action here
    }

    private void ProfileIcon_Clicked(object sender, EventArgs e)
    {
        // Handle the profile icon button click
        // You can add your code to navigate to the user's profile or perform any other action here
    }

    private void OnBackButtonClicked(object sender, EventArgs e) // Dashboard button
    {
        // Check if there are pages to navigate back to
        if (Navigation.NavigationStack.Count > 1)
        {
            // Use PopAsync to navigate back to the previous page
            Navigation.PopAsync();
        }
    }

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

    private void LoadPreceptorInformation()
    {
        clinicalPageNumber = 5;
        // Load preceptor information based on the currently signed-in student's email
        var loadedPreceptor = database.LoadPreceptorInformation(database.CurrentlySignedInStudentEmail, clinicalPageNumber);

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

    private async void SavePreceptorInformation(object sender, EventArgs e)
    {
        if (PreceptorInfo.IsVisible)
        {
            // Assuming you have set the BindingContext of the page to a PreceptorViewModel instance
            // var preceptorViewModel = (PreceptorViewModel)BindingContext;

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
