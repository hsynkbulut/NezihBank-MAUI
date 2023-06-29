using Microsoft.Maui.HotReload;
using NezihBankMAUI.Models;
using NezihBankMAUI.Services;
using NezihBankMAUI.Views;
using NezihBankMAUI.Views.CustomerView;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NezihBankMAUI.Views.Startup;

public partial class LoginPage : ContentPage
{
    public ICommand KaydolCommand { get; private set; }

    ObservableCollection<PersonalCustomersMAUI> personalCustomers;

    IPersonalCustomerServiceMAUI personalCustomerService;

    public LoginPage()
	{
		InitializeComponent();

        personalCustomerService = new PersonalCustomerServiceMAUI();
        personalCustomers = new ObservableCollection<PersonalCustomersMAUI>();

        KaydolCommand = new Command(OnKaydolClicked);
        BindingContext = this;
    }

    private async void btnPersonalLogin_Clicked(object sender, EventArgs e)
    {
        // Kullan�c� giri�inde gerekli kimlik bilgilerini alal�m
        string identityNumber = txtIdentityNumber.Text;
        string password = txtPassword.Text;

        if (string.IsNullOrEmpty(identityNumber) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Hata", "TC Kimlik Numaras� veya �ifre girilmedi.", "Tamam");
            return;
        }

        Preferences.Set("MusteriKimlikNumarasi", identityNumber);

        var personalCustomers = await personalCustomerService.GetPersonalCustomers();
        var customer = personalCustomers.FirstOrDefault(c => c.IdentityNumber == identityNumber && c.Password == password);

        if (customer != null)
        {
            await Shell.Current.GoToAsync("//homepage");
        }
        else
        {
            await DisplayAlert("Hata", "Ge�ersiz TC Kimlik Numaras� veya �ifre", "Tamam");
        }

        // Giri� alanlar�n� temizle
        txtIdentityNumber.Text = string.Empty;
        txtPassword.Text = string.Empty;
    }

    private async void OnKaydolClicked()
    {
        //1.YOL
        await Shell.Current.GoToAsync("//personalRegister");

        //2.YOL
        //await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
    }

    private async void rememberCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        rememberCheckBox.IsChecked = false;
        await DisplayAlert("B�LG�", "Hen�z bu �zellik kullan�mda de�il!", "Tamam");
    }
}