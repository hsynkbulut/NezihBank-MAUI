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
            bool cevap = await DisplayAlert("Hoop! Bir Dakika", "Bu m��teri hesab�n� silmek istedi�inizden emin misiniz?", "Evet", "Hay�r");
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
            DisplayAlert("UYARI", "M��teri numaras� girilmedi!", "Tamam");
            return false;
        }

        if (string.IsNullOrEmpty(txtIban.Text) || txtIban.Text.Length != 24)
        {
            DisplayAlert("UYARI", "IBAN numaras� 24 haneli olmal�d�r!", "Tamam");
            return false;
        }

        if (string.IsNullOrEmpty(txtHesapNo.Text) || txtHesapNo.Text.Length != 7)
        {
            DisplayAlert("UYARI", "Hesap numaras� 7 haneli olmal�d�r!", "Tamam");
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
                AccountId = 0, // Otomatik artan oldu�u i�in s�f�r olarak atanabilir veya atlanabilir
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
                await DisplayAlert("B�LG�", "Yeni m��teri hesab� ba�ar�yla eklendi", "Tamam");
                await GetCustomerAccounts();
            }
            else
            {
                await DisplayAlert("UYARI", "M��teri numaras� yanl��!", "Tamam");
            }
        }

        // T�m giri� alanlar�n� temizleyelim
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
                await DisplayAlert("UYARI", "Hesap ID'si bo� b�rak�lamaz!", "Tamam");
                return;
            }

            if (ValidateInput())
            {
                // Veritaban�ndan t�m hesaplar� alal�m
                List<PersonalAccountsMAUI> accounts = await accountService.GetCustomerAccounts();

                // Hesap ID'sine g�re g�ncellenecek hesab� bulal�m
                PersonalAccountsMAUI account = accounts.FirstOrDefault(a => a.AccountId == accountId);

                if (account != null && account.PersonalCustomerId == customerId)
                {
                    // Kullan�c�n�n girdi�i bilgileri al�p g�ncelleme yapmak i�in personalAccounts nesnesini olu�tural�m
                    PersonalAccountsMAUI personalAccounts = new PersonalAccountsMAUI
                    {
                        AccountId = accountId, //KAPANAB�L�R
                        PersonalCustomerId = account.PersonalCustomerId,
                        IbanNumber = "TR" + txtIban.Text,
                        AccountNumber = txtHesapNo.Text,
                        Balance = Convert.ToDecimal(txtBakiye.Text)
                    };

                    // Veritaban�nda ilgili hesab� g�ncelleyelim
                    await accountService.UpdateAccount(personalAccounts);

                    await DisplayAlert("B�LG�", "Hesap g�ncelleme i�lemi ba�ar�l�", "Tamam");
                    await GetCustomerAccounts();
                }
                else
                {
                    await DisplayAlert("UYARI", "Hesap ID'si veya m��teri numaras�/ID'si yanl��!", "Tamam");
                }
            }
            else
            {
                await DisplayAlert("UYARI", "L�tfen t�m bilgileri eksiksiz ve do�ru bir �ekilde girin.", "Tamam");
            }
        }
        catch (ArgumentNullException)
        {
            await DisplayAlert("UYARI", "Hesap ID'si bo� b�rak�lamaz!", "Tamam");
        }
        catch (Exception)
        {
            await DisplayAlert("UYARI", "Hesap ID'si rakamlardan olu�mal�!", "Tamam");
        }

        // T�m giri� alanlar�n� temizleyelim
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