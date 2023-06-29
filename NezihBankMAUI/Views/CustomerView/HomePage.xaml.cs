using Microsoft.Maui.Controls;
using NezihBankMAUI.Models;
using NezihBankMAUI.Services;
using NezihBankMAUI.Views.Startup;
using System.Collections.ObjectModel;
using System.Web;
using System.Windows.Input;
using System.Xml.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Xaml;
using System.Runtime.CompilerServices;

namespace NezihBankMAUI.Views.CustomerView;

public partial class HomePage : ContentPage
{
    private ObservableCollection<PersonalAccountsMAUI> hesaplar;
    IPersonalAccountServiceMAUI accountService;

    private ObservableCollection<PersonalCustomersMAUI> customers;
    IPersonalCustomerServiceMAUI personalCustomerService;

    public HomePage()
    {
        InitializeComponent();

        accountService = new PersonalAccountServiceMAUI();
        hesaplar = new ObservableCollection<PersonalAccountsMAUI>();
        carouselView.ItemsSource = hesaplar;

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

        // CarouselView'e sadece giriþ yapan müþteriye ait hesaplarý ekleyelim
        carouselView.ItemsSource = customerAccounts;

        customerName.Text = customer.Name;
        customerSurname.Text = customer.Surname;
    }

    private void carouselView_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
    {
        //var seciliEleman = carouselView.CurrentItem as PersonalAccounts;
        var seciliEleman = (PersonalAccountsMAUI)carouselView.CurrentItem;

        if (seciliEleman is not null)
        {
            lblTest.Text = seciliEleman.AccountNumber;
        }

    }

    private async void btnMoneyTransfer_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//islem");
    }

    private async void btnAccountActivities_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("BÝLGÝ", "Henüz bu sayfa kullanýmda deðil!", "Tamam");
    }

}