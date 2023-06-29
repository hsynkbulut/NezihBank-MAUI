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
        await DisplayAlert("B�LG�", "Yak�nda eklenecek!", "Tamam");
    }

    private async void rememberCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        rememberCheckBox.IsChecked = false;
        await DisplayAlert("B�LG�", "Hen�z bu �zellik kullan�mda de�il!", "Tamam");
    }
}