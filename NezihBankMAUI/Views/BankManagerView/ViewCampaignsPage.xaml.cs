using NezihBankMAUI.Models;
using NezihBankMAUI.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NezihBankMAUI.Views.BankManagerView;

public partial class ViewCampaignsPage : ContentPage
{
    public ICommand KampanyaSilCommand { get; set; }
    public ICommand KampanyaGuncelleCommand { get; set; }

    ObservableCollection<AnnouncementMAUI> duyurular;

    IAnnouncementServiceMAUI duyuruService;

    public ViewCampaignsPage()
	{
		InitializeComponent();

        duyuruService = new AnnouncementServiceMAUI();
        duyurular = new ObservableCollection<AnnouncementMAUI>();
        lstKampanya.ItemsSource = duyurular;

        KampanyaSilCommand = new Command<AnnouncementMAUI>(async (AnnouncementMAUI seciliDuyuru) => {
            bool cevap = await DisplayAlert("Hoop! Bir Dakika", "Bu duyuruyu silmek istedi�inizden emin misiniz?", "Evet", "Hay�r");
            if (cevap)
            {
                await duyuruService.DeleteAnnouncement(seciliDuyuru.Id);
                await GetAnnouncements();
            }
        });

        KampanyaGuncelleCommand = new Command<AnnouncementMAUI>(async (AnnouncementMAUI seciliDuyuru) => {
            //await Navigation.PushModalAsync(new Views.BankManagerView.AddCampaignsPage());
            await Shell.Current.GoToAsync("//campaignAdd");
        });

        this.BindingContext = this;
    }

    private async void btnListele_Clicked(object sender, EventArgs e)
    {
        await GetAnnouncements();
    }

    private async Task GetAnnouncements()
    {
        var sonuc = await duyuruService.GetAnnouncements();
        duyurular.Clear();

        foreach (var item in sonuc)
        {
            duyurular.Add(item);
        }
    }

    private async void lstKampanya_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var seciliDuyuru = (AnnouncementMAUI)lstKampanya.SelectedItem;

        if (seciliDuyuru != null)
        {
            await Navigation.PushModalAsync(new Views.BankManagerView.CampaignDetailPage(seciliDuyuru));
        }
    }

    private async void lstKampanya_RemainingItemsThresholdReached(object sender, EventArgs e)
    {
        await GetAnnouncements();
    }

    private async void btnKampanyaSil_Clicked(object sender, EventArgs e)
    {
        string kampanyaIdString = KampanyaSilEntry.Text;

        if (string.IsNullOrEmpty(kampanyaIdString))
        {
            await DisplayAlert("Uyar�", "L�tfen kampanya ID'sini girin.", "Tamam");
            return;
        }

        if (!int.TryParse(kampanyaIdString, out int kampanyaId))
        {
            await DisplayAlert("Uyar�", "Ge�ersiz kampanya ID'si.", "Tamam");
            KampanyaSilEntry.Text = string.Empty;
            return;
        }

        var announcements = duyurular.FirstOrDefault(c => c.Id == kampanyaId);

        if (announcements != null)
        {
            bool cevap = await DisplayAlert("Hoop! Bir Dakika", "Bu kampanyay� silmek istedi�inizden emin misiniz?", "Evet", "Hay�r");

            if (cevap)
            {
                await duyuruService.DeleteAnnouncement(announcements.Id);
                await DisplayAlert("Bilgi", "Silme i�lemi ba�ar�l�", "Tamam");
                await GetAnnouncements();
            }
        }
        else
        {
            await DisplayAlert("Uyar�", "Bu ID'ye sahip bir kampanya bulunamad�.", "Tamam");
        }

        // T�m giri� alanlar�n� temizleyelim
        KampanyaSilEntry.Text = string.Empty;
    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        //await Navigation.PopModalAsync();
        await Shell.Current.GoToAsync("//managerhome");
    }
}