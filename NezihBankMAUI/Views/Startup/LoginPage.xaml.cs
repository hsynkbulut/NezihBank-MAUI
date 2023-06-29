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
        // Kullanýcý giriþinde gerekli kimlik bilgilerini alalým
        string identityNumber = txtIdentityNumber.Text;
        string password = txtPassword.Text;

        if (string.IsNullOrEmpty(identityNumber) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Hata", "TC Kimlik Numarasý veya Þifre girilmedi.", "Tamam");
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
            await DisplayAlert("Hata", "Geçersiz TC Kimlik Numarasý veya Þifre", "Tamam");
        }

        // Giriþ alanlarýný temizle
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
        await DisplayAlert("BÝLGÝ", "Henüz bu özellik kullanýmda deðil!", "Tamam");
    }
}