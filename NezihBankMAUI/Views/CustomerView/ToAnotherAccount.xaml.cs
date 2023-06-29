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
        // M��teri kimlik numaras�n� tercihlerden alal�m
        string musteriKimlikNumarasi = Preferences.Get("MusteriKimlikNumarasi", "");

        var sonuc = await accountService.GetCustomerAccounts();
        var sonuc2 = await personalCustomerService.GetPersonalCustomers();

        var customer = sonuc2.FirstOrDefault(c => c.IdentityNumber == musteriKimlikNumarasi);

        // G�nderen m��teriye ait hesaplar� alal�m
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
            await DisplayAlert("Hata", "Alici hesap numaras� veya tutar girilmedi.", "Tamam");
            return;
        }

        if (!decimal.TryParse(tutarText, out decimal tutar) || tutar <= 0)
        {
            await DisplayAlert("Hata", "Ge�ersiz tutar.", "Tamam");
            return;
        }

        if (pickerGonderenHesap.SelectedItem == null)
        {
            await DisplayAlert("Hata", "G�nderen hesap se�ilmedi.", "Tamam");
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
            TransactionCategory = "Ba�ka Hesaba Para Transferi"
        };

        // Al�c� hesab� bulal�m
        var allAccounts = await accountService.GetCustomerAccounts();
        PersonalAccountsMAUI aliciHesap = allAccounts.FirstOrDefault(a => string.Equals(a.AccountNumber, aliciHesapNumarasi, StringComparison.OrdinalIgnoreCase));

        if (aliciHesap != null)
        {
            // Al�c� hesab�n bakiyesini g�ncelle
            aliciHesap.Balance += tutar;

            try
            {
                // ��lemi kaydet
                await personalTransactionsService.AddTransaction(yeniIslem);

                // G�nderen hesab�n bakiyesini g�ncelle
                gonderenHesap.Balance -= tutar;

                // G�nderen ve al�c� hesaplar� g�ncelle
                await accountService.UpdateAccount(gonderenHesap);
                await accountService.UpdateAccount(aliciHesap);

                await DisplayAlert("Ba�ar�l�", "Para transferi ger�ekle�tirildi.", "Tamam");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", "��lem ger�ekle�tirilirken bir hata olu�tu: " + ex.Message, "Tamam");
            }
        }
        else
        {
            await DisplayAlert("Hata", "Ge�erli bir al�c� hesap numaras� girin.", "Tamam");
        }

        // ��lem tamamland�ktan sonra alanlar� s�f�rlayal�m
        txtAliciHesap.Text = string.Empty;
        pickerGonderenHesap.SelectedItem = null;
        txtTutar.Text = string.Empty;
    }

    private async void btnGeri_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}