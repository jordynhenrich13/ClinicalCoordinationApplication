using ClinicalCoordinationApplication.Model;
using System.Collections.ObjectModel;

namespace ClinicalCoordinationApplication;

public partial class CoordinatorDashboard : ContentPage
{
    private IDatabase Database = new Database();
    public ObservableCollection<ClinicalButton> ClinicalButtons { get; set; } = new ObservableCollection<ClinicalButton>();
    public ObservableCollection<Student> Students { get; set; } = new ObservableCollection<Student>();

    public CoordinatorDashboard()
    {
        InitializeComponent();
        BindingContext = this;

        // Populate clinical buttons with corresponding clinical pages
        ClinicalButtons.Add(new ClinicalButton("Clinical 1", typeof(Clinical1)));
        ClinicalButtons.Add(new ClinicalButton("Clinical 2", typeof(Clinical2)));
        ClinicalButtons.Add(new ClinicalButton("Clinical 3", typeof(Clinical3)));
        ClinicalButtons.Add(new ClinicalButton("Clinical 4", typeof(Clinical4)));
        ClinicalButtons.Add(new ClinicalButton("Clinical 5", typeof(Clinical5)));
        ClinicalButtons.Add(new ClinicalButton("Clinical 6", typeof(Clinical6)));

        UpdateStudentList();
    }

    private void UpdateStudentList()
    {
        // Fetch the list of students from the database
        Students.Clear();
        ObservableCollection<Student> studentsFromDatabase = Database.SelectAllStudents();

        // Add fetched students to the observable collection
        foreach (var student in studentsFromDatabase)
        {
            Students.Add(student);
        }
    }

    private void ClinicalButtonClicked(object sender, EventArgs e)
    {
        try
        {
            if (sender is Button button && button.Text != null)
            {
                string clinicalPageNumber = button.Text;

                switch (clinicalPageNumber)
                {
                    case "1":
                        Navigation.PushAsync(new Clinical1());
                        break;
                    case "2":
                        Navigation.PushAsync(new Clinical2());
                        break;
                    case "3":
                        Navigation.PushAsync(new Clinical3());
                        break;
                    case "4":
                        Navigation.PushAsync(new Clinical4());
                        break;
                    case "5":
                        Navigation.PushAsync(new Clinical5());
                        break;
                    case "6":
                        Navigation.PushAsync(new Clinical6());
                        break;
                    default:
                        Console.WriteLine($"Unknown clinical page: {clinicalPageNumber}");
                        break;
                }

                // Log the current navigation stack for debugging
                var stack = Navigation?.NavigationStack;
                if (stack != null)
                {
                    Console.WriteLine($"Current Navigation Stack: {string.Join(", ", stack.Select(page => page.GetType().Name))}");
                }
                else
                {
                    Console.WriteLine("Navigation stack is null.");
                }
            }
            else
            {
                Console.WriteLine("Invalid sender or button text is null.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Navigation error: {ex.Message}");
        }
    }



    private void OnStudentSearched(object sender, EventArgs e)
    {
        SearchBar searchBar = (SearchBar)sender;
        if (searchBar.Text.CompareTo("") != 0)
        {
            MauiProgram.BusinessLogic.FindStudent(searchBar.Text);
        }
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
