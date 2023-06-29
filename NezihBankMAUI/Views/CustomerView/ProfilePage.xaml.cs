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

        // Müþteri kimlik numarasýný tercihlerden alalým
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
        // Müþteri kimlik numarasýný tercihlerden alalým
        string customerIdentityNumber = Preferences.Get("MusteriKimlikNumarasi", "");

        // Veritabanýndan ilgili müþteriyi bulalým
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
            await DisplayAlert("Telefon Numarasý", phoneNumber, "Tamam");
        }
    }

    private async void ChangePasswordOnFrame_Tapped(object sender, EventArgs e)
    {
        string customerIdentityNumber = Preferences.Get("MusteriKimlikNumarasi", "");

        var customers = await personalCustomerService.GetPersonalCustomers();
        var customer = customers.FirstOrDefault(c => c.IdentityNumber == customerIdentityNumber);

        if (customer != null)
        {
            string oldPassword = await DisplayPromptAsync("Þifre Deðiþtir", "Mevcut Þifrenizi Girin", accept: "Tamam", cancel: "Geri", maxLength: 6, keyboard: Keyboard.Numeric, placeholder: "Mevcut þifreniz");
            if (oldPassword == null) // Kullanýcý geri tuþuna bastýysa oldPassword null olacak
            {
                return; // Geri tuþuna basýldýðý için fonksiyondan çýkýlacak
            }
            if (oldPassword == customer.Password)
            {
                string newPassword = await DisplayPromptAsync("Þifre Deðiþtir", "Yeni Þifrenizi Girin", accept: "Tamam", cancel: "Geri", maxLength: 6, keyboard: Keyboard.Numeric, placeholder: "Yeni þifreniz");
                if (!string.IsNullOrEmpty(newPassword))
                {
                    customer.Password = newPassword;

                    // Veritabanýnda müþteri þifresini güncelleme iþlemini yapalým
                    await personalCustomerService.UpdateCustomer(customer);
                    await DisplayAlert("Baþarýlý", "Þifre deðiþtirme iþlemi tamamlandý.", "Tamam");
                }
            }
            else
            {
                await DisplayAlert("Hata", "Mevcut þifrenizi yanlýþ girdiniz.", "Tamam");
            }
        }
    }

}