using NezihBankMAUI.Models;

namespace NezihBankMAUI.Views.CustomerView;

public partial class ViewCampaignDetails : ContentPage
{
	public ViewCampaignDetails(AnnouncementMAUI duyuru)
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

    private async void btnGeri_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void btnBilgi_Clicked(object sender, EventArgs e)
    {
        await Launcher.OpenAsync("https://www.linkedin.com/in/hsynkbulut/");
    }
}