using CommunityToolkit.Maui.Views;

namespace ClinicalCoordinationApplication;

public partial class DirectorReportsDashboard : ContentPage
{

	public DirectorReportsDashboard()
	{
		InitializeComponent();

	}

    public void DisplayPopup()
    {
        var popup = new DirectorAddReportPopup();
        this.ShowPopup(popup);
    }

    void AddReportClicked(object sender, EventArgs e)
    {
        DisplayPopup();
    }
}

