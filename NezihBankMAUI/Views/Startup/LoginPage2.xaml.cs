using System.Windows.Input;

namespace NezihBankMAUI.Views.Startup;

public partial class LoginPage2 : ContentPage
{
    public LoginPage2()
	{
		InitializeComponent();
    }

    private async void CorporateLoginBtnClicked(object sender, EventArgs e)
    {
        await DisplayAlert("BÝLGÝ", "Yakýnda eklenecek!", "Tamam");
    }

    private async void rememberCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        rememberCheckBox.IsChecked = false;
        await DisplayAlert("BÝLGÝ", "Henüz bu özellik kullanýmda deðil!", "Tamam");
    }
}