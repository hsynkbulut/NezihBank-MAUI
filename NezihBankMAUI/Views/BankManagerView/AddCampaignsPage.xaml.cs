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
            bool cevap = await DisplayAlert("Hoop! Bir Dakika", "Bu duyuruyu silmek istediðinizden emin misiniz?", "Evet", "Hayýr");
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
            DisplayAlert("BÝLGÝ", "Lütfen duyuru baþlýðýný giriniz!", "Tamam");
            return false;
        }

        if (string.IsNullOrEmpty(txtContent.Text))
        {
            DisplayAlert("BÝLGÝ", "Lütfen duyuru içeriðini giriniz!", "Tamam");
            return false;
        }

        if (string.IsNullOrEmpty(txtImageUrl.Text))
        {
            DisplayAlert("BÝLGÝ", "Lütfen duyuru görsel adresini giriniz!", "Tamam");
            return false;
        }
        
        return true;
    }

    private async void btnKaydet_Clicked(object sender, EventArgs e)
    {
        if (ValidateInput())
        {
            // Yetkili kimlik numarasýný tercihlerden alalým
            string bankManagerIdentityNumber = Preferences.Get("YetkiliKimlikNumarasi", "");

            // Veritabanýndan yetkilileri alalým
            List<BankManagerMAUI> bankManager = await bankManagerService.GetBankManagers();

            // Kimlik numarasýna göre ilgili banka yetkilisini bulalým
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

            await DisplayAlert("BÝLGÝ", "Yeni kampanya duyurusu baþarýyla eklendi", "Tamam");

            // Tüm giriþ alanlarýný temizleyelim
            txtTitle.Text = string.Empty;
            txtContent.Text = string.Empty;
            txtImageUrl.Text = string.Empty;
        }
        /*
        else
        {
            await DisplayAlert("UYARI", "Lütfen tüm bilgileri eksiksiz ve doðru bir þekilde girin.", "Tamam");
        }
        */
    }

    private async void btnGuncelle_Clicked(object sender, EventArgs e)
    {
        int announcementId;
        if (!int.TryParse(txtId.Text, out announcementId))
        {
            await DisplayAlert("HATA", "Geçersiz Duyuru Numarasý!", "Tamam");
            return;
        }

        if (ValidateInput())
        {
            // Yetkili kimlik numarasýný tercihlerden alalým
            string bankManagerIdentityNumber = Preferences.Get("YetkiliKimlikNumarasi", "");

            // Veritabanýndan yetkilileri alalým
            List<BankManagerMAUI> bankManagers = await bankManagerService.GetBankManagers();

            // Kimlik numarasýna göre ilgili banka yetkilisini bulalým
            BankManagerMAUI bankManagerFind = bankManagers.FirstOrDefault(a => a.IdentityNumber == bankManagerIdentityNumber);

            if (bankManagerFind == null)
            {
                await DisplayAlert("HATA", "Yetkili bulunamadý!", "Tamam");
                return;
            }

            // Veritabanýndan duyurularý alalým
            List<AnnouncementMAUI> announcements = await duyuruService.GetAnnouncements();

            // Duyuru ID'sine göre güncellenecek duyuruyu bulalým
            AnnouncementMAUI announcementFind = announcements.FirstOrDefault(a => a.Id == announcementId);

            if (announcementFind != null && announcementFind.Id == announcementId)
            {
                // Kullanýcýnýn girdiði bilgileri alýp güncelleme yapmak için updateAnnouncement nesnesini oluþturalým
                var updateAnnouncement = new AnnouncementMAUI()
                {
                    Id = announcementId,
                    Title = txtTitle.Text,
                    Content = txtContent.Text,
                    CreatedDate = DateTime.Now,
                    ImageUrl = txtImageUrl.Text,
                    BankManagerId = bankManagerFind.BankManagerId
                };

                // Güncelleme iþlemi için Guncelle fonksiyonunu çaðýrýr
                await duyuruService.UpdateAnnouncement(updateAnnouncement);
                await GetAnnouncements();

                await DisplayAlert("BÝLGÝ", "Duyuru güncelleme iþlemi baþarýlý", "Tamam");

                // Tüm giriþ alanlarýný temizleyelim
                txtId.Text = string.Empty;
                txtTitle.Text = string.Empty;
                txtContent.Text = string.Empty;
                txtImageUrl.Text = string.Empty;
            }
            else
            {
                await DisplayAlert("UYARI", "Böyle bir Duyuru numarasý/ID'sine sahip bir duyuru yok!", "Tamam");
            }
        }
        /*
        else
        {
            await DisplayAlert("UYARI", "Lütfen tüm bilgileri doðru bir þekilde girin.", "Tamam");
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