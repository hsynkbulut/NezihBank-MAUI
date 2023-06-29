using NezihBankMAUI.Models;
using NezihBankMAUI.Services;
using System.Collections.ObjectModel;
using System.Security.Principal;
using System.Windows.Input;

namespace NezihBankMAUI.Views.BankManagerView;

public partial class ViewCustomerAccountPage : ContentPage
{
    public ICommand HesapSilCommand { get; set; }
    public ICommand HesapGuncelleCommand { get; set; }

    ObservableCollection<PersonalAccountsMAUI> accounts;

    IPersonalAccountServiceMAUI accountService;

    public ViewCustomerAccountPage()
	{
		InitializeComponent();

        accountService = new PersonalAccountServiceMAUI();
        accounts = new ObservableCollection<PersonalAccountsMAUI>();
        lstAccount.ItemsSource = accounts;

        HesapSilCommand = new Command<PersonalAccountsMAUI>(async (PersonalAccountsMAUI seciliHesap) => {
            bool cevap = await DisplayAlert("Hoop! Bir Dakika", "Bu hesabý silmek istediðinizden emin misiniz?", "Evet", "Hayýr");
            if (cevap)
            {
                await accountService.DeleteAccount(seciliHesap.AccountId);
                await GetCustomerAccounts();
            }
        });

        HesapGuncelleCommand = new Command<PersonalAccountsMAUI>(async (PersonalAccountsMAUI seciliHesap) => {
            //await Navigation.PushModalAsync(new Views.BankManagerView.CustomerAccountAddPage());
            await Shell.Current.GoToAsync("//customerAccountAdd");
        });

        this.BindingContext = this;
    }

    private async void btnListele_Clicked(object sender, EventArgs e)
    {
        await GetCustomerAccounts();
    }

    private async Task GetCustomerAccounts()
    {
        var sonuc = await accountService.GetCustomerAccounts();
        accounts.Clear();

        foreach (var item in sonuc)
        {
            accounts.Add(item);
        }
    }

    private async void lstAccount_RemainingItemsThresholdReached(object sender, EventArgs e)
    {
        await GetCustomerAccounts();
    }

    private async void btnHesapSil_Clicked(object sender, EventArgs e)
    {
        string hesapIdString = HesapSilEntry.Text;

        if (string.IsNullOrEmpty(hesapIdString))
        {
            await DisplayAlert("Uyarý", "Hesap ID'si girilmedi.", "Tamam");
            return;
        }

        if (!int.TryParse(hesapIdString, out int hesapId))
        {
            await DisplayAlert("Uyarý", "Geçersiz Hesap ID'si.", "Tamam");
            HesapSilEntry.Text = string.Empty;
            return;
        }

        var hesaplar = accounts.FirstOrDefault(c => c.AccountId == hesapId);

        if (hesaplar != null)
        {
            bool cevap = await DisplayAlert("Hoop! Bir Dakika", "Bu hesabý silmek istediðinizden emin misiniz?", "Evet", "Hayýr");

            if (cevap)
            {
                await accountService.DeleteAccount(hesaplar.AccountId);
                await DisplayAlert("Bilgi", "Silme iþlemi baþarýlý", "Tamam");
                await GetCustomerAccounts();
            }
        }
        else
        {
            await DisplayAlert("Uyarý", "Bu ID'ye sahip bir müþteri hesabý bulunamadý.", "Tamam");
        }

        // Tüm giriþ alanlarýný temizleyelim
        HesapSilEntry.Text = string.Empty;
    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        //await Navigation.PopModalAsync();
        await Shell.Current.GoToAsync("//managerhome");
    }
}