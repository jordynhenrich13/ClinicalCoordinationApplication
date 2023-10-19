using CommunityToolkit.Maui.Views;

namespace ClinicalCoordinationApplication;

public partial class DirectorAddReportPopup : Popup
{
	public DirectorAddReportPopup()
	{
        InitializeComponent();
	}

    /// NEED TO MAKE THESE DIFF? 
    void OnSubmitButtonClicked(object sender, EventArgs e) => Close();
    void OnCancelButtonClicked(object sender, EventArgs e) => Close();
}
