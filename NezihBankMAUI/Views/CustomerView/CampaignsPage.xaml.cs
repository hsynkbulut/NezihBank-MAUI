using NezihBankMAUI.Models;
using NezihBankMAUI.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NezihBankMAUI.Views.CustomerView;

public partial class CampaignsPage : ContentPage
{
    public ICommand KampanyaAyrintiCommand { get; set; }

    ObservableCollection<AnnouncementMAUI> duyurular;

    IAnnouncementServiceMAUI duyuruService;

    public CampaignsPage()
	{
		InitializeComponent();

        duyuruService = new AnnouncementServiceMAUI();
        duyurular = new ObservableCollection<AnnouncementMAUI>();
        lstKampanya.ItemsSource = duyurular;

        KampanyaAyrintiCommand = new Command<AnnouncementMAUI>(async (AnnouncementMAUI seciliDuyuru) => {
            await Navigation.PushModalAsync(new Views.CustomerView.ViewCampaignDetails(seciliDuyuru));
        });

        GetAnnouncements();


        this.BindingContext = this;
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
            await Navigation.PushModalAsync(new Views.CustomerView.ViewCampaignDetails(seciliDuyuru));
        }
    }

    private async void lstKampanya_RemainingItemsThresholdReached(object sender, EventArgs e)
    {
        await GetAnnouncements();
    }

}