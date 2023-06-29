using NezihBankMAUI.Models;
using NezihBankMAUI.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NezihBankMAUI.Views.BankManagerView;

public partial class CustomerAccountAddPage : ContentPage
{
    public ICommand HesapSilCommand { get; set; }

    ObservableCollection<PersonalAccountsMAUI> hesaplar;

    IPersonalAccountServiceMAUI accountService;

    ObservableCollection<PersonalCustomersMAUI> customers;

    IPersonalCustomerServiceMAUI personalCustomerService;

    public CustomerAccountAddPage()
	{
		InitializeComponent();

        accountService = new PersonalAccountServiceMAUI();
        hesaplar = new ObservableCollection<PersonalAccountsMAUI>();
        lstHesaplar.ItemsSource = hesaplar;

        personalCustomerService = new PersonalCustomerServiceMAUI();
        customers = new ObservableCollection<PersonalCustomersMAUI>();

        HesapSilCommand = new Command<PersonalAccountsMAUI>(async (PersonalAccountsMAUI seciliHesap) => {
            bool cevap = await DisplayAlert("Hoop! Bir Dakika", "Bu müþteri hesabýný silmek istediðinizden emin misiniz?", "Evet", "Hayýr");
            if (cevap)
            {
                await accountService.DeleteAccount(seciliHesap.AccountId);
                await GetCustomerAccounts();
            }
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
        hesaplar.Clear();

        foreach (var item in sonuc)
        {
            hesaplar.Add(item);
        }
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrEmpty(txtCustomerId.Text))
        {
            DisplayAlert("UYARI", "Müþteri numarasý girilmedi!", "Tamam");
            return false;
        }

        if (string.IsNullOrEmpty(txtIban.Text) || txtIban.Text.Length != 24)
        {
            DisplayAlert("UYARI", "IBAN numarasý 24 haneli olmalýdýr!", "Tamam");
            return false;
        }

        if (string.IsNullOrEmpty(txtHesapNo.Text) || txtHesapNo.Text.Length != 7)
        {
            DisplayAlert("UYARI", "Hesap numarasý 7 haneli olmalýdýr!", "Tamam");
            return false;
        }

        if (string.IsNullOrEmpty(txtBakiye.Text))
        {
            DisplayAlert("UYARI", "Bakiye girilmedi!", "Tamam");
            return false;
        }

        return true;
    }

    private async void btnKaydet_Clicked(object sender, EventArgs e)
    {
        if (ValidateInput())
        {
            int customerId = int.Parse(txtCustomerId.Text);

            var duyuru = new PersonalAccountsMAUI()
            {
                AccountId = 0, // Otomatik artan olduðu için sýfýr olarak atanabilir veya atlanabilir
                PersonalCustomerId = customerId,
                IbanNumber = "TR" + txtIban.Text,
                AccountNumber = txtHesapNo.Text,
                Balance = Convert.ToDecimal(txtBakiye.Text),
            };

            var sonuc = await personalCustomerService.GetPersonalCustomers();
            var manager = sonuc.FirstOrDefault(item => item.Id == customerId);

            if (manager != null)
            {
                await accountService.AddAccount(duyuru);
                await DisplayAlert("BÝLGÝ", "Yeni müþteri hesabý baþarýyla eklendi", "Tamam");
                await GetCustomerAccounts();
            }
            else
            {
                await DisplayAlert("UYARI", "Müþteri numarasý yanlýþ!", "Tamam");
            }
        }

        // Tüm giriþ alanlarýný temizleyelim
        txtCustomerId.Text = string.Empty;
        txtIban.Text = string.Empty;
        txtHesapNo.Text = string.Empty;
        txtBakiye.Text = string.Empty;
    }

    private async void btnGuncelle_Clicked(object sender, EventArgs e)
    {
        try
        {
            int customerId = int.Parse(txtCustomerId.Text);
            int accountId = int.Parse(txtAccountId.Text);

            if (string.IsNullOrWhiteSpace(txtAccountId.Text))
            {
                await DisplayAlert("UYARI", "Hesap ID'si boþ býrakýlamaz!", "Tamam");
                return;
            }

            if (ValidateInput())
            {
                // Veritabanýndan tüm hesaplarý alalým
                List<PersonalAccountsMAUI> accounts = await accountService.GetCustomerAccounts();

                // Hesap ID'sine göre güncellenecek hesabý bulalým
                PersonalAccountsMAUI account = accounts.FirstOrDefault(a => a.AccountId == accountId);

                if (account != null && account.PersonalCustomerId == customerId)
                {
                    // Kullanýcýnýn girdiði bilgileri alýp güncelleme yapmak için personalAccounts nesnesini oluþturalým
                    PersonalAccountsMAUI personalAccounts = new PersonalAccountsMAUI
                    {
                        AccountId = accountId, //KAPANABÝLÝR
                        PersonalCustomerId = account.PersonalCustomerId,
                        IbanNumber = "TR" + txtIban.Text,
                        AccountNumber = txtHesapNo.Text,
                        Balance = Convert.ToDecimal(txtBakiye.Text)
                    };

                    // Veritabanýnda ilgili hesabý güncelleyelim
                    await accountService.UpdateAccount(personalAccounts);

                    await DisplayAlert("BÝLGÝ", "Hesap güncelleme iþlemi baþarýlý", "Tamam");
                    await GetCustomerAccounts();
                }
                else
                {
                    await DisplayAlert("UYARI", "Hesap ID'si veya müþteri numarasý/ID'si yanlýþ!", "Tamam");
                }
            }
            else
            {
                await DisplayAlert("UYARI", "Lütfen tüm bilgileri eksiksiz ve doðru bir þekilde girin.", "Tamam");
            }
        }
        catch (ArgumentNullException)
        {
            await DisplayAlert("UYARI", "Hesap ID'si boþ býrakýlamaz!", "Tamam");
        }
        catch (Exception)
        {
            await DisplayAlert("UYARI", "Hesap ID'si rakamlardan oluþmalý!", "Tamam");
        }

        // Tüm giriþ alanlarýný temizleyelim
        txtAccountId.Text = string.Empty;
        txtCustomerId.Text = string.Empty;
        txtIban.Text = string.Empty;
        txtHesapNo.Text = string.Empty;
        txtBakiye.Text = string.Empty;
    }


    private async void lstHesaplar_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var seciliDuyuru = (AnnouncementMAUI)lstHesaplar.SelectedItem;

        if (seciliDuyuru != null)
        {
            await Navigation.PushModalAsync(new Views.BankManagerView.CampaignDetailPage(seciliDuyuru));
        }
    }

    private async void lstHesaplar_RemainingItemsThresholdReached(object sender, EventArgs e)
    {
        await GetCustomerAccounts();
    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        //await Navigation.PopModalAsync();
        await Shell.Current.GoToAsync("//managerhome");
    }
}