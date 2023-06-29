using Microsoft.Maui.Controls;
using NezihBankMAUI.Models;
using NezihBankMAUI.Services;
using System.Collections.ObjectModel;

namespace NezihBankMAUI.Views.CustomerView;

public partial class ToMyAccount : ContentPage
{
    ObservableCollection<PersonalAccountsMAUI> personalAccounts;
    IPersonalAccountServiceMAUI accountService;

    private ObservableCollection<PersonalCustomersMAUI> customers;
    IPersonalCustomerServiceMAUI personalCustomerService;

    private ObservableCollection<PersonalTransactionsMAUI> transactions;
    IPersonalTransactionServiceMAUI personalTransactionsService;

    public ToMyAccount()
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

        // Müþteri kimlik numarasýný tercihlerden alalým
        string musteriKimlikNumarasi = Preferences.Get("MusteriKimlikNumarasi", "");

        var sonuc = await accountService.GetCustomerAccounts();
        var sonuc2 = await personalCustomerService.GetPersonalCustomers();

        var customer = sonuc2.FirstOrDefault(c => c.IdentityNumber == musteriKimlikNumarasi);

        var customerAccounts = sonuc.Where(c => c.PersonalCustomerId == customer.Id).ToList();

        personalAccounts.Clear();
        foreach (var account in customerAccounts)
        {
            personalAccounts.Add(account);
        }

        pickerGonderenHesap.ItemsSource = personalAccounts;
        pickerAliciHesap.ItemsSource = personalAccounts.Except(new[] { pickerGonderenHesap.SelectedItem }).ToList();
    }


    private void pickerGonderenHesap_SelectedIndexChanged(object sender, EventArgs e)
    {
        // pickerGonderenHesap seçimi deðiþtiðinde pickerAliciHesap'ýn ItemsSource'unu güncelleyelim
        if (pickerGonderenHesap.SelectedItem != null)
        {
            var selectedAccount = (PersonalAccountsMAUI)pickerGonderenHesap.SelectedItem;
            var selectedAccountId = selectedAccount.AccountId;

            var otherAccounts = personalAccounts.Where(a => a.AccountId != selectedAccountId).ToList();
            pickerAliciHesap.ItemsSource = otherAccounts;
        }

    }

    private async void btnGeri_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void btnSend_Clicked(object sender, EventArgs e)
    {
        var gonderenHesap = (PersonalAccountsMAUI)pickerGonderenHesap.SelectedItem;
        var aliciHesap = (PersonalAccountsMAUI)pickerAliciHesap.SelectedItem;

        decimal transferMiktari;
        bool isTutarValid = decimal.TryParse(txtTutar.Text, out transferMiktari);

        if (gonderenHesap == null || aliciHesap == null)
        {
            await DisplayAlert("Hata", "Gönderen ve alýcý hesaplarý seçiniz.", "Tamam");
            return;
        }

        if (string.IsNullOrEmpty(txtTutar.Text))
        {
            await DisplayAlert("Hata", "Transfer miktarýný giriniz.", "Tamam");
            return;
        }

        if (!isTutarValid || transferMiktari <= 0)
        {
            await DisplayAlert("Hata", "Geçersiz transfer miktarý.", "Tamam");
            return;
        } 

        DateTime selectedDate = transactionDatePicker.Date;

        if (selectedDate == null || selectedDate > DateTime.Now || selectedDate.Date < DateTime.Now.Date)
        {
            await DisplayAlert("Hata", "Geçersiz iþlem tarihi.", "Tamam");
            return;
        }

        // Gönderici hesaptan transfer miktarýný düþelim
        gonderenHesap.Balance -= transferMiktari;

        // Alýcý hesaba transfer miktarýný ekleyelim
        aliciHesap.Balance += transferMiktari;

        // Gönderici hesabý güncelleyelim
        await accountService.UpdateAccount(gonderenHesap);

        // Alýcý hesabý güncelleyelim
        await accountService.UpdateAccount(aliciHesap);

        // PersonalTransactions tablosuna yapýlan iþlemi ekleyelim
        PersonalTransactionsMAUI transaction = new PersonalTransactionsMAUI
        {
            AccountId = gonderenHesap.AccountId,
            TransactionDate = selectedDate,
            Amount = transferMiktari,
            TransactionCategory = "Kendi Hesabýna Para Transferi"
        };

        await personalTransactionsService.AddTransaction(transaction);

        await DisplayAlert("Baþarýlý", "Para transferi iþlemi tamamlandý.", "Tamam");

        // Ýþlem tamamlandýktan sonra alanlarý sýfýrlayalým
        txtTutar.Text = string.Empty;
        pickerGonderenHesap.SelectedItem = null;
        pickerAliciHesap.SelectedItem = null;
    }

}