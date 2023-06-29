using NezihBankMAUI.Models;
using NezihBankMAUI.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Linq;

namespace NezihBankMAUI.Views.Startup;

public partial class BankManagerLogin : ContentPage
{
    public ICommand KaydolCommand3 { get; private set; }

    ObservableCollection<BankManagerMAUI> bankManagers;

    IBankManagerServiceMAUI bankManagerService;

    public BankManagerLogin()
	{
		InitializeComponent();

        bankManagerService = new BankManagerServiceMAUI();
        bankManagers = new ObservableCollection<BankManagerMAUI>();

        KaydolCommand3 = new Command(OnKaydolClicked3);
        BindingContext = this;
    }

    private async Task GetBankManagers()
    {
        var sonuc = await bankManagerService.GetBankManagers();
        bankManagers.Clear();

        foreach (var item in sonuc)
        {
            bankManagers.Add(item);
        }
    }

    private async Task CheckBankManager()
    {
        var sonuc = await bankManagerService.GetBankManagers();
        string identityNumber = txtIdentityNumber.Text;
        string password = txtPassword.Text;

        Preferences.Set("YetkiliKimlikNumarasi", identityNumber);

        if (string.IsNullOrEmpty(identityNumber) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("HATA", "Lütfen TC Kimlik Numarasý ve þifreyi girin.", "Tamam");
        }
        else
        {
            var manager = sonuc.FirstOrDefault(item => item.IdentityNumber == identityNumber && item.Password == password);

            if (manager != null)
            {
                //await Navigation.PushModalAsync(new Views.BankManagerView.BankManagerHomepage());
                await Shell.Current.GoToAsync("//managerhome");
            }
            else
            {
                await DisplayAlert("HATA", "TC Kimlik No veya þifre hatalý.", "Tamam");
            }
        }

        // Tüm giriþ alanlarýný temizleyelim
        txtIdentityNumber.Text = string.Empty;
        txtPassword.Text = string.Empty;
    }


    private async void BankManagerLoginBtnClicked(object sender, EventArgs e)
    {
        await CheckBankManager();
    }

    private async void OnKaydolClicked3()
    {
        await Shell.Current.GoToAsync("//bankManagerRegister");
    }

    private async void rememberCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        rememberCheckBox.IsChecked = false;
        await DisplayAlert("BÝLGÝ", "Henüz bu özellik kullanýmda deðil!", "Tamam");
    }
}