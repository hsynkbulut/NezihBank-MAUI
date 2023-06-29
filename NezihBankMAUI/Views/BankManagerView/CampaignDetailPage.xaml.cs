using NezihBankMAUI.Models;

namespace NezihBankMAUI.Views.BankManagerView;

public partial class CampaignDetailPage : ContentPage
{
	public CampaignDetailPage(AnnouncementMAUI duyuru)
	{
		InitializeComponent();

        FillPage(duyuru); 
    }

    private void FillPage(AnnouncementMAUI duyuru)
    {
        lblBaslik.Text = duyuru.Title;
        lblIcerik.Text = duyuru.Content;
        lblImages.Source = new UriImageSource { Uri = new Uri(duyuru.ImageUrl) };
    }

    private async void btnHome_Clicked(object sender, EventArgs e)
    {
        //await Navigation.PopModalAsync();
        await Shell.Current.GoToAsync("//managerhome");
    }

    private async void btnBilgi_Clicked(object sender, EventArgs e)
    {
        await Launcher.OpenAsync("https://www.linkedin.com/in/hsynkbulut/");
    }
}