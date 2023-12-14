using ClinicalCoordinationApplication.Model;
using System.Collections.ObjectModel;

namespace ClinicalCoordinationApplication
{
    public partial class CoordinatorDashboard : ContentPage
    {
        // Observable collections to hold clinical buttons and students
        public ObservableCollection<ClinicalButton> ClinicalButtons { get; set; } = new ObservableCollection<ClinicalButton>();
        public ObservableCollection<Student> Students { get; set; } = new ObservableCollection<Student>();

        public CoordinatorDashboard()
        {
            InitializeComponent();
            BindingContext = this;

            // Populate clinical buttons with corresponding clinical pages
            ClinicalButtons.Add(new ClinicalButton("Clinical1", typeof(Clinical1)));
            ClinicalButtons.Add(new ClinicalButton("Clinical2", typeof(Clinical2)));
            ClinicalButtons.Add(new ClinicalButton("Clinical3", typeof(Clinical3)));
            ClinicalButtons.Add(new ClinicalButton("Clinical4", typeof(Clinical4)));
            ClinicalButtons.Add(new ClinicalButton("Clinical5", typeof(Clinical5)));
            ClinicalButtons.Add(new ClinicalButton("Clinical6", typeof(Clinical6)));

            // Initial update of the student list
            UpdateStudentList();
        }

        // Method to update the list of students
        private void UpdateStudentList()
        {
            Students.Clear();
            ObservableCollection<Student> studentsFromDatabase = MauiProgram.BusinessLogic.Students;

            // Add fetched students to the observable collection
            foreach (var student in studentsFromDatabase)
            {
                Students.Add(student);
            }
        }

        // Event handler for clinical button clicks
        private async void ClinicalButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                Console.WriteLine($"Button clicked! CommandParameter: {button.CommandParameter}");

                if (int.TryParse(button.CommandParameter?.ToString(), out int clinicalPageNumber))
                {
                    Console.WriteLine($"Clinical page number: {clinicalPageNumber}");

                    // Get the selected student
                    var selectedStudent = (Student)button.BindingContext;

                    // Navigate to the corresponding clinical page based on the button clicked
                    switch (clinicalPageNumber)
                    {
                        case 1:
                            await Navigation.PushAsync(new Clinical1(selectedStudent));
                            break;
                        case 2:
                            await Navigation.PushAsync(new Clinical2(selectedStudent));
                            break;
                        case 3:
                            await Navigation.PushAsync(new Clinical3(selectedStudent));
                            break;
                        case 4:
                            await Navigation.PushAsync(new Clinical4(selectedStudent));
                            break;
                        case 5:
                            await Navigation.PushAsync(new Clinical5(selectedStudent));
                            break;
                        case 6:
                            await Navigation.PushAsync(new Clinical6(selectedStudent));
                            break;
                        default:
                            Console.WriteLine($"Unknown clinical page: {clinicalPageNumber}");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"Invalid CommandParameter: {button.CommandParameter}");
                }
            }
        }

        // Event handler for the search bar's text change
        private void OnStudentSearched(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;

            // Check if the search bar text is empty
            if (searchBar.Text.CompareTo("") == 0)
            {
                // Fetch all students and update the student list
                MauiProgram.BusinessLogic.GetAllStudents();
                UpdateStudentList();
            }
            else
            {
                // Search for students based on the search bar text and update the student list
                MauiProgram.BusinessLogic.FindStudent(searchBar.Text);
                UpdateStudentList();
            }
        }

        // Event handler for cohort picker's selection change
        private void CohortPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = cohortPicker.SelectedIndex;

            // Check if the selected index is within bounds
            if (selectedIndex >= 0 && selectedIndex < cohortOptions.Length)
            {
                // Get the selected cohort from the array
                var selectedCohort = cohortOptions[selectedIndex];
            }
        }

        // Array of cohort options
        private string[] cohortOptions = { "October 2023", "Other Cohort Options..." };

        // Event handler for side menu button click
        private void SideMenuButton_Clicked(object sender, EventArgs e)
        {
            // Handle the side menu button click
        }

        // Event handler for profile icon click
        private void ProfileIcon_Clicked(object sender, EventArgs e)
        {
            // Navigate to the user profile page
            Navigation.PushAsync(new UserProfile());
        }

        // Event handler for update status button click
        private void UpdateStatusButton(object sender, EventArgs e)
        {
            // Navigate to the assign clinical information page
            Navigation.PushAsync(new AssignClinicalInformation());
        }

        // Event handler to navigate to the assign clinical information page
        private void NavigateToAssignClinicalInformationPage(object sender, EventArgs e)
        {
            // Use the navigation logic to navigate to the AssignClinicalInformation page
            Navigation.PushAsync(new AssignClinicalInformation());
        }
    }
}
