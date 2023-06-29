using System.Windows.Input;

namespace NezihBankMAUI.Controls;

public partial class NavbarBankManager : ContentView
{
    public ICommand HomePageCommand { get; private set; }
    public ICommand GoToLoginPageCommand { get; private set; }

    public NavbarBankManager()
	{
		InitializeComponent();

        HomePageCommand = new Command(OnHomePageClicked);
        GoToLoginPageCommand = new Command(GoToLoginPage);
        BindingContext = this;
    }

    private async void OnHomePageClicked()
    {
        //await Navigation.PushModalAsync(new Views.BankManagerView.BankManagerHomepage());
        await Shell.Current.GoToAsync("//managerhome");
    }

    private async void GoToLoginPage()
    {
        //await Navigation.PushModalAsync(new View.Startup.BankManagerLogin());
        await Shell.Current.GoToAsync("///bankManagerLogin");
    }
}
