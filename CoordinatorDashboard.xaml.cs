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

    private void ProfileIcon_Clicked(object sender, EventArgs e)
    {
        // Handle the profile icon button click
        // You can add your code to navigate to the user's profile or perform any other action here
    }
}
