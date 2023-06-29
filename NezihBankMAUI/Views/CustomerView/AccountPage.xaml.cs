using Microsoft.Maui.Controls;
using NezihBankMAUI.Models;
using NezihBankMAUI.Services;
using System.Collections.ObjectModel;

namespace NezihBankMAUI.Views.CustomerView;

public partial class AccountPage : ContentPage
{
    private ObservableCollection<PersonalAccountsMAUI> hesaplar;
    IPersonalAccountServiceMAUI accountService;

    private ObservableCollection<PersonalCustomersMAUI> customers;
    IPersonalCustomerServiceMAUI personalCustomerService;

    public AccountPage()
	{
		InitializeComponent();

        accountService = new PersonalAccountServiceMAUI();
        hesaplar = new ObservableCollection<PersonalAccountsMAUI>();
        lstAccounts.ItemsSource = hesaplar;

        personalCustomerService = new PersonalCustomerServiceMAUI();
        customers = new ObservableCollection<PersonalCustomersMAUI>();

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

        lstAccounts.ItemsSource = customerAccounts;
    }

}