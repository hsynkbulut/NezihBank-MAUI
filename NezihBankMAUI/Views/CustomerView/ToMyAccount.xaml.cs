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

        // M��teri kimlik numaras�n� tercihlerden alal�m
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
        // pickerGonderenHesap se�imi de�i�ti�inde pickerAliciHesap'�n ItemsSource'unu g�ncelleyelim
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
            await DisplayAlert("Hata", "G�nderen ve al�c� hesaplar� se�iniz.", "Tamam");
            return;
        }

        if (string.IsNullOrEmpty(txtTutar.Text))
        {
            await DisplayAlert("Hata", "Transfer miktar�n� giriniz.", "Tamam");
            return;
        }

        if (!isTutarValid || transferMiktari <= 0)
        {
            await DisplayAlert("Hata", "Ge�ersiz transfer miktar�.", "Tamam");
            return;
        } 

        DateTime selectedDate = transactionDatePicker.Date;

        if (selectedDate == null || selectedDate > DateTime.Now || selectedDate.Date < DateTime.Now.Date)
        {
            await DisplayAlert("Hata", "Ge�ersiz i�lem tarihi.", "Tamam");
            return;
        }

        // G�nderici hesaptan transfer miktar�n� d��elim
        gonderenHesap.Balance -= transferMiktari;

        // Al�c� hesaba transfer miktar�n� ekleyelim
        aliciHesap.Balance += transferMiktari;

        // G�nderici hesab� g�ncelleyelim
        await accountService.UpdateAccount(gonderenHesap);

        // Al�c� hesab� g�ncelleyelim
        await accountService.UpdateAccount(aliciHesap);

        // PersonalTransactions tablosuna yap�lan i�lemi ekleyelim
        PersonalTransactionsMAUI transaction = new PersonalTransactionsMAUI
        {
            AccountId = gonderenHesap.AccountId,
            TransactionDate = selectedDate,
            Amount = transferMiktari,
            TransactionCategory = "Kendi Hesab�na Para Transferi"
        };

        await personalTransactionsService.AddTransaction(transaction);

        await DisplayAlert("Ba�ar�l�", "Para transferi i�lemi tamamland�.", "Tamam");

        // ��lem tamamland�ktan sonra alanlar� s�f�rlayal�m
        txtTutar.Text = string.Empty;
        pickerGonderenHesap.SelectedItem = null;
        pickerAliciHesap.SelectedItem = null;
    }

}