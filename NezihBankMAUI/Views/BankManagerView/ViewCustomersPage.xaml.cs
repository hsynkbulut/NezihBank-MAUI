using NezihBankMAUI.Models;
using NezihBankMAUI.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NezihBankMAUI.Views.BankManagerView;

public partial class ViewCustomersPage : ContentPage
{
    public ICommand MusteriSilCommand { get; set; }
    public ICommand MusteriGuncelleCommand { get; set; }

    ObservableCollection<PersonalCustomersMAUI> musteriler;

    IPersonalCustomerServiceMAUI personalCustomerService;

    public ViewCustomersPage()
	{
		InitializeComponent();

        personalCustomerService = new PersonalCustomerServiceMAUI();
        musteriler = new ObservableCollection<PersonalCustomersMAUI>();
        lstMusteri.ItemsSource = musteriler;

        MusteriSilCommand = new Command<PersonalCustomersMAUI>(async (PersonalCustomersMAUI seciliMusteri) => {
            bool cevap = await DisplayAlert("Ups! Bir Dakika", "Bu müþteriyi silmek istediðinizden emin misiniz?", "Evet", "Hayýr");
            if (cevap)
            {
                await personalCustomerService.DeleteCustomer(seciliMusteri.Id);
                await GetPersonalCustomers();
            }
        });

        MusteriGuncelleCommand = new Command<PersonalCustomersMAUI>(async (PersonalCustomersMAUI seciliMusteri) => {
            //await Navigation.PushModalAsync(new Views.BankManagerView.CustomerAddPage());
            await Shell.Current.GoToAsync("//customerAdd");
        });

        this.BindingContext = this;
    }

    private async void btnListele_Clicked(object sender, EventArgs e)
    {
        await GetPersonalCustomers();
    }

    private async Task GetPersonalCustomers()
    {
        var sonuc = await personalCustomerService.GetPersonalCustomers();
        musteriler.Clear();

        foreach (var item in sonuc)
        {
            musteriler.Add(item);
        }
    }

    private async void lstMusteri_RemainingItemsThresholdReached(object sender, EventArgs e)
    {
        await GetPersonalCustomers();
    }

    private async void btnMusteriSil_Clicked(object sender, EventArgs e)
    {
        string musteriIdString = MusteriSilEntry.Text;

        if (string.IsNullOrEmpty(musteriIdString))
        {
            await DisplayAlert("Uyarý", "Müþteri ID'si girilmedi.", "Tamam");
            return;
        }

        int musteriId = int.Parse(musteriIdString);

        var personalCustomers = musteriler.FirstOrDefault(c => c.Id == musteriId);

        if (personalCustomers != null)
        {
            bool cevap = await DisplayAlert("Hoop! Bir Dakika", "Bu müþteriyi silmek istediðinizden emin misiniz?", "Evet", "Hayýr");

            if (cevap)
            {
                await personalCustomerService.DeleteCustomer(personalCustomers.Id);
                await DisplayAlert("Bilgi", "Silme iþlemi baþarýlý", "Tamam");
                await GetPersonalCustomers();
            }
        }
        else
        {
            await DisplayAlert("Uyarý", "Bu ID'ye sahip bir müþteri bulunamadý.", "Tamam");
        }

        // Tüm giriþ alanlarýný temizleyelim
        MusteriSilEntry.Text = string.Empty;
    }


    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        //await Navigation.PopModalAsync();
        await Shell.Current.GoToAsync("//managerhome");
    }
}