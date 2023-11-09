using System.Collections.ObjectModel;
using ClinicalCoordinationApplication.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClinicalCoordinationApplication;

public partial class FlyoutMenuPage : ContentPage
{
    private readonly BusinessLogic businessLogic;
    private readonly ObservableCollection<FlyoutPageItem> flyoutPageItems = new();
    public ObservableCollection<FlyoutPageItem> FlyoutPageItems { get { return flyoutPageItems; } }

    public FlyoutMenuPage()
    {
        InitializeComponent();
        businessLogic = new BusinessLogic();

        //BindingContext = this;

        // Determines which pages to show in navigation
        if (businessLogic.loggedInUserRole == null || businessLogic.loggedInUserRole == "")
        {
            Console.WriteLine("User is not signed in.");
            flyoutPageItems.Add(new FlyoutPageItem { Title = "Student Dashboard", IconSource = "" });
            flyoutPageItems.Add(new FlyoutPageItem { Title = "Sign In", IconSource = "" });
        }

        if (businessLogic.loggedInUserRole == "Student")
        {
            Console.WriteLine("User is signed in as a Student.");
            flyoutPageItems.Add(new FlyoutPageItem { Title = "Student Dashboard", IconSource = "" });
            flyoutPageItems.Add(new FlyoutPageItem { Title = "User Profile", IconSource = "" });
        }

        if (businessLogic.loggedInUserRole == "Coordinator")
        {
            Console.WriteLine("User is signed in as a Coordinator.");
            flyoutPageItems.Add(new FlyoutPageItem { Title = "Coordinator Dashboard", IconSource = "" });
            flyoutPageItems.Add(new FlyoutPageItem { Title = "User Profile", IconSource = "" });
            flyoutPageItems.Add(new FlyoutPageItem { Title = "Coordinator Reports", IconSource = "" });
        }

        if (businessLogic.loggedInUserRole == "Coordinator" && businessLogic.loggedInUserName == "Erika")
        {
            Console.WriteLine("User is signed in as a Coordinator.");
            flyoutPageItems.Add(new FlyoutPageItem { Title = "Coordinator Dashboard", IconSource = "" });
            flyoutPageItems.Add(new FlyoutPageItem { Title = "User Profile", IconSource = "" });
            flyoutPageItems.Add(new FlyoutPageItem { Title = "Director Reports", IconSource = "" });
        }
    }

    public class FlyoutPageItem
    {
        public string Title { get; set; }
        public string IconSource { get; set; }
        //public Type TargetType { get; set; }

    }
}