using Microsoft.Maui.Controls;
using NezihBankMAUI.Models;
using NezihBankMAUI.Services;
using System.Collections.ObjectModel;

namespace NezihBankMAUI.Views.CustomerView;

public partial class ProfilePage : ContentPage
{
    private ObservableCollection<PersonalAccountsMAUI> hesaplar;
    IPersonalAccountServiceMAUI accountService;

    private ObservableCollection<PersonalCustomersMAUI> customers;
    IPersonalCustomerServiceMAUI personalCustomerService;

    public ProfilePage()
	{
		InitializeComponent();

        accountService = new PersonalAccountServiceMAUI();
        hesaplar = new ObservableCollection<PersonalAccountsMAUI>();

        personalCustomerService = new PersonalCustomerServiceMAUI();
        customers = new ObservableCollection<PersonalCustomersMAUI>();

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

        if (customer != null)
        {
            var customerAccounts = sonuc.Where(c => c.PersonalCustomerId == customer.Id).ToList();

            customerName.Text = customer.Name;
            customerSurname.Text = customer.Surname;
        }
        
    }

    private async void ViewAddressOnFrame_Tapped(object sender, TappedEventArgs e)
    {
        // M��teri kimlik numaras�n� tercihlerden alal�m
        string customerIdentityNumber = Preferences.Get("MusteriKimlikNumarasi", "");

        // Veritaban�ndan ilgili m��teriyi bulal�m
        var customers = await personalCustomerService.GetPersonalCustomers();
        var customer = customers.FirstOrDefault(c => c.IdentityNumber == customerIdentityNumber);

        if (customer != null)
        {
            string address = customer.Address;
            await DisplayAlert("Ev Adresi", address, "Tamam");
        }
    }

    private async void ViewEmailOnFrame_Tapped(object sender, TappedEventArgs e)
    {
        string customerIdentityNumber = Preferences.Get("MusteriKimlikNumarasi", "");

        var customers = await personalCustomerService.GetPersonalCustomers();
        var customer = customers.FirstOrDefault(c => c.IdentityNumber == customerIdentityNumber);

        if (customer != null)
        {
            string email = customer.Email;
            await DisplayAlert("E-posta", email, "Tamam");
        }
    }

    private async void ViewPhoneNumberOnFrame_Tapped(object sender, TappedEventArgs e)
    {
        string customerIdentityNumber = Preferences.Get("MusteriKimlikNumarasi", "");

        var customers = await personalCustomerService.GetPersonalCustomers();
        var customer = customers.FirstOrDefault(c => c.IdentityNumber == customerIdentityNumber);

        if (customer != null)
        {
            string phoneNumber = customer.PhoneNumber;
            await DisplayAlert("Telefon Numaras�", phoneNumber, "Tamam");
        }
    }

    private async void ChangePasswordOnFrame_Tapped(object sender, EventArgs e)
    {
        string customerIdentityNumber = Preferences.Get("MusteriKimlikNumarasi", "");

        var customers = await personalCustomerService.GetPersonalCustomers();
        var customer = customers.FirstOrDefault(c => c.IdentityNumber == customerIdentityNumber);

        if (customer != null)
        {
            string oldPassword = await DisplayPromptAsync("�ifre De�i�tir", "Mevcut �ifrenizi Girin", accept: "Tamam", cancel: "Geri", maxLength: 6, keyboard: Keyboard.Numeric, placeholder: "Mevcut �ifreniz");
            if (oldPassword == null) // Kullan�c� geri tu�una bast�ysa oldPassword null olacak
            {
                return; // Geri tu�una bas�ld��� i�in fonksiyondan ��k�lacak
            }
            if (oldPassword == customer.Password)
            {
                string newPassword = await DisplayPromptAsync("�ifre De�i�tir", "Yeni �ifrenizi Girin", accept: "Tamam", cancel: "Geri", maxLength: 6, keyboard: Keyboard.Numeric, placeholder: "Yeni �ifreniz");
                if (!string.IsNullOrEmpty(newPassword))
                {
                    customer.Password = newPassword;

                    // Veritaban�nda m��teri �ifresini g�ncelleme i�lemini yapal�m
                    await personalCustomerService.UpdateCustomer(customer);
                    await DisplayAlert("Ba�ar�l�", "�ifre de�i�tirme i�lemi tamamland�.", "Tamam");
                }
            }
            else
            {
                await DisplayAlert("Hata", "Mevcut �ifrenizi yanl�� girdiniz.", "Tamam");
            }
        }
    }

}