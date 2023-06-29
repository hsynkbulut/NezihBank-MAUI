using Microsoft.Maui.Controls;
using NezihBankMAUI.Models;
using NezihBankMAUI.Services;
using System.Collections.ObjectModel;

namespace NezihBankMAUI.Views.BankManagerView;

public partial class BankManagerHomepage : ContentPage
{
    private ObservableCollection<BankManagerMAUI> bankManagers;
    IBankManagerServiceMAUI bankManagerService;

    public BankManagerHomepage()
	{
		InitializeComponent();

        bankManagerService = new BankManagerServiceMAUI();
        bankManagers = new ObservableCollection<BankManagerMAUI>();

        this.BindingContext = this;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        // Yetkili kimlik numarasýný tercihlerden alalým
        string bankManagerIdentityNumber = Preferences.Get("YetkiliKimlikNumarasi", "");

        var result = await bankManagerService.GetBankManagers();
        var managerFind = result.FirstOrDefault(c => c.IdentityNumber == bankManagerIdentityNumber);

        managerName.Text = managerFind.Name;
        managerSurname.Text = managerFind.Surname;
    }

    private async void AddCustomerOnFrame_Tapped(object sender, TappedEventArgs e)
    {
        //Navigation.PushModalAsync(new Views.BankManagerView.CustomerAddPage());
        await Shell.Current.GoToAsync("//customerAdd");    
    }
    
    private async void ViewCustomersOnFrame_Tapped(object sender, TappedEventArgs e)
    {
        //await Navigation.PushModalAsync(new Views.BankManagerView.ViewCustomersPage());
        await Shell.Current.GoToAsync("//customerView");
    }

    private async void AddCustomerAccountOnFrame_Tapped(object sender, TappedEventArgs e)
    {
        //await Navigation.PushModalAsync(new Views.BankManagerView.CustomerAccountAddPage());
        await Shell.Current.GoToAsync("//customerAccountAdd");
    }

    private async void ViewCustomerAccountOnFrame_Tapped(object sender, TappedEventArgs e)
    {
        //await Navigation.PushModalAsync(new Views.BankManagerView.ViewCustomerAccountPage());
        await Shell.Current.GoToAsync("//customerAccountView");
    }

    private async void AddAnnouncementOnFrame_Tapped(object sender, TappedEventArgs e)
    {
        //await Navigation.PushModalAsync(new Views.BankManagerView.AddCampaignsPage());
        await Shell.Current.GoToAsync("//campaignAdd");
    }

    private async void ViewAnnouncementOnFrame_Tapped(object sender, TappedEventArgs e)
    {
        //await Navigation.PushModalAsync(new Views.BankManagerView.ViewCampaignsPage());
        await Shell.Current.GoToAsync("//campaignView");
    }

    private async void ChangePasswordOnFrame_Tapped(object sender, EventArgs e)
    {
        string bankManagerIdentityNumber = Preferences.Get("YetkiliKimlikNumarasi", "");

        var managers = await bankManagerService.GetBankManagers();
        var managerFind = managers.FirstOrDefault(c => c.IdentityNumber == bankManagerIdentityNumber);

        if (managerFind != null)
        {
            string oldPassword = await DisplayPromptAsync("Þifre Deðiþtir", "Mevcut Þifrenizi Girin", accept: "Tamam", cancel: "Geri", maxLength: 6, keyboard: Keyboard.Numeric, placeholder: "Mevcut þifreniz");
            if (oldPassword == null) // Kullanýcý geri tuþuna bastýysa oldPassword null olacak
            {
                return; // Geri tuþuna basýldýðý için fonksiyondan çýkýlacak
            }
            if (oldPassword == managerFind.Password)
            {
                string newPassword = await DisplayPromptAsync("Þifre Deðiþtir", "Yeni Þifrenizi Girin", accept: "Tamam", cancel: "Geri", maxLength: 6, keyboard: Keyboard.Numeric, placeholder: "Yeni þifreniz");
                if (!string.IsNullOrEmpty(newPassword))
                {
                    managerFind.Password = newPassword;
                    // Veritabanýnda müþteri þifresini güncelleyelim
                    await bankManagerService.UpdateBankManager(managerFind);
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