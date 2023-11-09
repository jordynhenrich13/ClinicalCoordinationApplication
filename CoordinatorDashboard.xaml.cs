namespace ClinicalCoordinationApplication;

public partial class CoordinatorDashboard : ContentPage
{
	public CoordinatorDashboard()
	{
		InitializeComponent();
	}

    private void CohortPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Get the selected index
        int selectedIndex = cohortPicker.SelectedIndex;

        if (selectedIndex >= 0 && selectedIndex < cohortOptions.Length)
        {
            // Get the selected cohort from the array
            var selectedCohort = cohortOptions[selectedIndex];
        }
    }

    private string[] cohortOptions = { "October 2023", "Other Cohort Options..." };

    private void SideMenuButton_Clicked(object sender, EventArgs e)
    {
        // Handle the side menu button click
        // You can add your code to open the side menu or perform any other action here
    }

    private void OnViewProfileButtonClicked(object sender, EventArgs e)
    {
        // Navigate to the UserProfilePage
        Navigation.PushAsync(new StudentProfile());
    }
    // This goes to the profile of the student's profile that the coordinator is looking at
    // KNOW THAT THIS IS NOT THE PROFILE OF THAT ACTUAL USER!!

    private void ProfileIcon_Clicked(object sender, EventArgs e)
    {
        // Navigate to the StudentProfile page
        Navigation.PushAsync(new UserProfile());
    }
    // This goes to the profile of whatever user is signed in, could be a coordinator or student or director
    // The User Profile page should have a table in the database that holds the information specified on the UML

    private void UpdateStatusButton(object sender, EventArgs e)
    {
        // Navigate to the assign clinical information
        Navigation.PushAsync(new AssignClinicalInformation());
    }
    private void NavigateToAssignClinicalInformationPage(object sender, EventArgs e)
    {
        // Use the navigation logic to navigate to the AssignClinicalInformation page
        Navigation.PushAsync(new AssignClinicalInformation());
    }

    // Navigate to clinical 1 when the button text '1' is pressed
    private void Clinical1(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Clinical1());
    }
    private void Clinical2(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Clinical2());
    }
    private void Clinical3(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Clinical3());
    }
    private void Clinical4(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Clinical4());
    }
    private void Clinical5(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Clinical5());
    }
    private void Clinical6(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Clinical6());
    }


}
