using NezihBankMAUI.Models;
using NezihBankMAUI.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Linq;

namespace NezihBankMAUI.Views.BankManagerView;

public partial class AddCampaignsPage : ContentPage
{
    public ICommand DuyuruSilCommand { get; set; }
    public ICommand DuyuruAyrintiCommand { get; set; }

    ObservableCollection<AnnouncementMAUI> duyurular;

    IAnnouncementServiceMAUI duyuruService;

    ObservableCollection<BankManagerMAUI> bankManagers;

    IBankManagerServiceMAUI bankManagerService;

    public AddCampaignsPage()
	{
		InitializeComponent();

        bankManagerService = new BankManagerServiceMAUI();
        bankManagers = new ObservableCollection<BankManagerMAUI>();

        duyuruService = new AnnouncementServiceMAUI();
        duyurular = new ObservableCollection<AnnouncementMAUI>();
        lstDuyuru.ItemsSource = duyurular;

        DuyuruSilCommand = new Command<AnnouncementMAUI>(async (AnnouncementMAUI seciliDuyuru) => {
            bool cevap = await DisplayAlert("Hoop! Bir Dakika", "Bu duyuruyu silmek istedi�inizden emin misiniz?", "Evet", "Hay�r");
            if (cevap)
            {
                await duyuruService.DeleteAnnouncement(seciliDuyuru.Id);
                await GetAnnouncements();
            }
        });

        DuyuruAyrintiCommand = new Command<AnnouncementMAUI>(async (AnnouncementMAUI seciliDuyuru) => {
            await Navigation.PushModalAsync(new Views.BankManagerView.CampaignDetailPage(seciliDuyuru));
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

    private bool ValidateInput()
    {
        if (string.IsNullOrEmpty(txtTitle.Text))
        {
            DisplayAlert("B�LG�", "L�tfen duyuru ba�l���n� giriniz!", "Tamam");
            return false;
        }

        if (string.IsNullOrEmpty(txtContent.Text))
        {
            DisplayAlert("B�LG�", "L�tfen duyuru i�eri�ini giriniz!", "Tamam");
            return false;
        }

        if (string.IsNullOrEmpty(txtImageUrl.Text))
        {
            DisplayAlert("B�LG�", "L�tfen duyuru g�rsel adresini giriniz!", "Tamam");
            return false;
        }
        
        return true;
    }

    private async void btnKaydet_Clicked(object sender, EventArgs e)
    {
        if (ValidateInput())
        {
            // Yetkili kimlik numaras�n� tercihlerden alal�m
            string bankManagerIdentityNumber = Preferences.Get("YetkiliKimlikNumarasi", "");

            // Veritaban�ndan yetkilileri alal�m
            List<BankManagerMAUI> bankManager = await bankManagerService.GetBankManagers();

            // Kimlik numaras�na g�re ilgili banka yetkilisini bulal�m
            BankManagerMAUI bankManagerFind = bankManager.FirstOrDefault(a => a.IdentityNumber == bankManagerIdentityNumber);

            var duyuru = new AnnouncementMAUI()
            {
                Id = 0, //Id = Guid.NewGuid(),
                Title = txtTitle.Text,
                Content = txtContent.Text,
                CreatedDate = DateTime.Now,
                ImageUrl = txtImageUrl.Text,
                BankManagerId = bankManagerFind.BankManagerId
            };
            await duyuruService.AddAnnouncement(duyuru);
            await GetAnnouncements();

            await DisplayAlert("B�LG�", "Yeni kampanya duyurusu ba�ar�yla eklendi", "Tamam");

            // T�m giri� alanlar�n� temizleyelim
            txtTitle.Text = string.Empty;
            txtContent.Text = string.Empty;
            txtImageUrl.Text = string.Empty;
        }
        /*
        else
        {
            await DisplayAlert("UYARI", "L�tfen t�m bilgileri eksiksiz ve do�ru bir �ekilde girin.", "Tamam");
        }
        */
    }

    private async void btnGuncelle_Clicked(object sender, EventArgs e)
    {
        int announcementId;
        if (!int.TryParse(txtId.Text, out announcementId))
        {
            await DisplayAlert("HATA", "Ge�ersiz Duyuru Numaras�!", "Tamam");
            return;
        }

        if (ValidateInput())
        {
            // Yetkili kimlik numaras�n� tercihlerden alal�m
            string bankManagerIdentityNumber = Preferences.Get("YetkiliKimlikNumarasi", "");

            // Veritaban�ndan yetkilileri alal�m
            List<BankManagerMAUI> bankManagers = await bankManagerService.GetBankManagers();

            // Kimlik numaras�na g�re ilgili banka yetkilisini bulal�m
            BankManagerMAUI bankManagerFind = bankManagers.FirstOrDefault(a => a.IdentityNumber == bankManagerIdentityNumber);

            if (bankManagerFind == null)
            {
                await DisplayAlert("HATA", "Yetkili bulunamad�!", "Tamam");
                return;
            }

            // Veritaban�ndan duyurular� alal�m
            List<AnnouncementMAUI> announcements = await duyuruService.GetAnnouncements();

            // Duyuru ID'sine g�re g�ncellenecek duyuruyu bulal�m
            AnnouncementMAUI announcementFind = announcements.FirstOrDefault(a => a.Id == announcementId);

            if (announcementFind != null && announcementFind.Id == announcementId)
            {
                // Kullan�c�n�n girdi�i bilgileri al�p g�ncelleme yapmak i�in updateAnnouncement nesnesini olu�tural�m
                var updateAnnouncement = new AnnouncementMAUI()
                {
                    Id = announcementId,
                    Title = txtTitle.Text,
                    Content = txtContent.Text,
                    CreatedDate = DateTime.Now,
                    ImageUrl = txtImageUrl.Text,
                    BankManagerId = bankManagerFind.BankManagerId
                };

                // G�ncelleme i�lemi i�in Guncelle fonksiyonunu �a��r�r
                await duyuruService.UpdateAnnouncement(updateAnnouncement);
                await GetAnnouncements();

                await DisplayAlert("B�LG�", "Duyuru g�ncelleme i�lemi ba�ar�l�", "Tamam");

                // T�m giri� alanlar�n� temizleyelim
                txtId.Text = string.Empty;
                txtTitle.Text = string.Empty;
                txtContent.Text = string.Empty;
                txtImageUrl.Text = string.Empty;
            }
            else
            {
                await DisplayAlert("UYARI", "B�yle bir Duyuru numaras�/ID'sine sahip bir duyuru yok!", "Tamam");
            }
        }
        /*
        else
        {
            await DisplayAlert("UYARI", "L�tfen t�m bilgileri do�ru bir �ekilde girin.", "Tamam");
        }
        */
    }


    private async void lstDuyuru_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var seciliDuyuru = (AnnouncementMAUI)lstDuyuru.SelectedItem;

        if (seciliDuyuru != null)
        {
            await Navigation.PushModalAsync(new Views.BankManagerView.CampaignDetailPage(seciliDuyuru));
        }
    }

    private async void lstDuyuru_RemainingItemsThresholdReached(object sender, EventArgs e)
    {
        await GetAnnouncements();
    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        //await Navigation.PopModalAsync();
        await Shell.Current.GoToAsync("//managerhome");
    }

}