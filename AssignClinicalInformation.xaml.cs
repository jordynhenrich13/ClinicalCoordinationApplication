namespace ClinicalCoordinationApplication;

public partial class AssignClinicalInformation : ContentPage
{
    public AssignClinicalInformation()
    {
        InitializeComponent();
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

    //private void OnNewNoteButtonClicked(object sender, EventArgs e)
    //{
    //    var newNote = new Frame
    //    {
    //        CornerRadius = 10,
    //        Padding = 5,
    //    };

    //    var editor = new Editor
    //    {
    //        Placeholder = "Enter your notes here...",
    //        HorizontalOptions = LayoutOptions.FillAndExpand,
    //        HeightRequest = 200,
    //    };

    //    newNote.Content = editor;

    //    var deleteButton = new Button
    //    {
    //        Text = "Delete",
    //        WidthRequest = 75,
    //    };

    //    var noteStackLayout = new StackLayout
    //    {
    //        Orientation = StackOrientation.Horizontal,
    //    };

    //    noteStackLayout.Children.Add(newNote);
    //    noteStackLayout.Children.Add(deleteButton);

    //    NotesContainer.Children.Add(noteStackLayout);
    //}



    //private void OnDeleteNoteButtonClicked(object sender, EventArgs e)
    //{
    //    // Get the button that was clicked
    //    var deleteButton = (Button)sender;

    //    // Find the parent StackLayout which contains the note and delete button
    //    var noteStackLayout = (StackLayout)deleteButton.Parent;

    //    if (noteStackLayout != null)
    //    {
    //        // Remove the parent StackLayout (the entire note with the delete button)
    //        NotesContainer.Children.Remove(noteStackLayout);
    //    }
    //}

    private void OnBackButtonClicked(object sender, EventArgs e) // Dashboard button
    {
        // Check if there are pages to navigate back to
        if (Navigation.NavigationStack.Count > 1)
        {
            // Use PopAsync to navigate back to the previous page
            Navigation.PopAsync();
        }
    }

    private void OnYesButtonClicked(object sender, EventArgs e)
    {
        PreceptorInfo.IsVisible = true;
    }

    private void OnNoButtonClicked(object sender, EventArgs e)
    {
        // Handle the "No" button click
        PreceptorInfo.IsVisible = false; // Hide the PreceptorInfo section
    }


}