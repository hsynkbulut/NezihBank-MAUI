using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace NezihBankMAUI.Controls;

public partial class NavbarContent : ContentView
{
    public ICommand HomePageCommand { get; private set; }
    public ICommand GoToLoginPageCommand { get; private set; }
    public NavbarContent()
	{
		InitializeComponent();

        HomePageCommand = new Command(OnHomePageClicked);
        GoToLoginPageCommand = new Command(GoToLoginPage);
        BindingContext = this;
    }

    private async void OnHomePageClicked()
    {
        await Shell.Current.GoToAsync("//homepage");
    }

    private async void GoToLoginPage()
    {
        await Shell.Current.GoToAsync("//personalLogin");
    }

}