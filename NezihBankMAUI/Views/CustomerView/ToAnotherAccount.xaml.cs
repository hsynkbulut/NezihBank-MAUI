using NezihBankMAUI.Models;
using NezihBankMAUI.Services;
using System.Collections.ObjectModel;

namespace NezihBankMAUI.Views.CustomerView;

public partial class ToAnotherAccount : ContentPage
{
    ObservableCollection<PersonalAccountsMAUI> personalAccounts;
    IPersonalAccountServiceMAUI accountService;

    private ObservableCollection<PersonalCustomersMAUI> customers;
    IPersonalCustomerServiceMAUI personalCustomerService;

    private ObservableCollection<PersonalTransactionsMAUI> transactions;
    IPersonalTransactionServiceMAUI personalTransactionsService;

    public ToAnotherAccount()
	{
		InitializeComponent();

        accountService = new PersonalAccountServiceMAUI();
        personalAccounts = new ObservableCollection<PersonalAccountsMAUI>();

        personalCustomerService = new PersonalCustomerServiceMAUI();
        customers = new ObservableCollection<PersonalCustomersMAUI>();

        personalTransactionsService = new PersonalTransactionServiceMAUI();
        transactions = new ObservableCollection<PersonalTransactionsMAUI>();

        this.BindingContext = this;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        await LoadCustomerAccounts();
    }

    private async Task LoadCustomerAccounts()
    {
        // Müþteri kimlik numarasýný tercihlerden alalým
        string musteriKimlikNumarasi = Preferences.Get("MusteriKimlikNumarasi", "");

        var sonuc = await accountService.GetCustomerAccounts();
        var sonuc2 = await personalCustomerService.GetPersonalCustomers();

        var customer = sonuc2.FirstOrDefault(c => c.IdentityNumber == musteriKimlikNumarasi);

        // Gönderen müþteriye ait hesaplarý alalým
        var customerAccounts = sonuc.Where(c => c.PersonalCustomerId == customer.Id).ToList();

        personalAccounts.Clear();
        foreach (var account in customerAccounts)
        {
            personalAccounts.Add(account);
        }

        pickerGonderenHesap.ItemsSource = personalAccounts;
    }

    private async void btnSend_Clicked(object sender, EventArgs e)
    {
        string aliciHesapNumarasi = txtAliciHesap.Text;
        string tutarText = txtTutar.Text;

        if (string.IsNullOrEmpty(aliciHesapNumarasi) || string.IsNullOrEmpty(tutarText))
        {
            await DisplayAlert("Hata", "Alici hesap numarasý veya tutar girilmedi.", "Tamam");
            return;
        }

        if (!decimal.TryParse(tutarText, out decimal tutar) || tutar <= 0)
        {
            await DisplayAlert("Hata", "Geçersiz tutar.", "Tamam");
            return;
        }

        if (pickerGonderenHesap.SelectedItem == null)
        {
            await DisplayAlert("Hata", "Gönderen hesap seçilmedi.", "Tamam");
            return;
        }

        PersonalAccountsMAUI gonderenHesap = (PersonalAccountsMAUI)pickerGonderenHesap.SelectedItem;

        if (gonderenHesap.Balance < tutar)
        {
            await DisplayAlert("Hata", "Yetersiz bakiye.", "Tamam");
            return;
        }

        DateTime tarih = transactionDatePicker.Date;

        PersonalTransactionsMAUI yeniIslem = new PersonalTransactionsMAUI
        {
            AccountId = gonderenHesap.AccountId,
            TransactionDate = tarih,
            Amount = tutar,
            TransactionCategory = "Baþka Hesaba Para Transferi"
        };

        // Alýcý hesabý bulalým
        var allAccounts = await accountService.GetCustomerAccounts();
        PersonalAccountsMAUI aliciHesap = allAccounts.FirstOrDefault(a => string.Equals(a.AccountNumber, aliciHesapNumarasi, StringComparison.OrdinalIgnoreCase));

        if (aliciHesap != null)
        {
            // Alýcý hesabýn bakiyesini güncelle
            aliciHesap.Balance += tutar;

            try
            {
                // Ýþlemi kaydet
                await personalTransactionsService.AddTransaction(yeniIslem);

                // Gönderen hesabýn bakiyesini güncelle
                gonderenHesap.Balance -= tutar;

                // Gönderen ve alýcý hesaplarý güncelle
                await accountService.UpdateAccount(gonderenHesap);
                await accountService.UpdateAccount(aliciHesap);

                await DisplayAlert("Baþarýlý", "Para transferi gerçekleþtirildi.", "Tamam");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", "Ýþlem gerçekleþtirilirken bir hata oluþtu: " + ex.Message, "Tamam");
            }
        }
        else
        {
            await DisplayAlert("Hata", "Geçerli bir alýcý hesap numarasý girin.", "Tamam");
        }

        // Ýþlem tamamlandýktan sonra alanlarý sýfýrlayalým
        txtAliciHesap.Text = string.Empty;
        pickerGonderenHesap.SelectedItem = null;
        txtTutar.Text = string.Empty;
    }

    private async void btnGeri_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}