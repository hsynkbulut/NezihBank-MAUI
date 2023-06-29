using System.Windows.Input;

namespace NezihBankMAUI.Views.CustomerView;

public partial class AboutPage : ContentPage
{
    public ICommand LinkedinCommand { get; private set; }
    public ICommand GitHubCommand { get; private set; }
    public ICommand MediumCommand { get; private set; }

    public AboutPage()
	{
		InitializeComponent();
		hakkimda.Text = hakkimdaCS;

        LinkedinCommand = new Command(OnClickedLinkedIn);
        GitHubCommand = new Command(OnClickedGitHub);
        MediumCommand = new Command(OnClickedMedium);
        BindingContext = this;
    }

	string hakkimdaCS = """
        Merhaba! Ben Hüseyin, Isparta Uygulamalý Bilimler Üniversitesi Bilgisayar Mühendisliði 2.sýnýf 
        öðrencisiyim ve ayný zamanda bir mobil uygulama geliþtiricisi adayýyým. Dart ve C# dilleri 
        üzerine çalýþýyorum. Flutter ve .NET MAUI gibi teknolojileri öðrenerek mobil uygulama geliþtirme 
        konusunda kendimi geliþtirmeye devam ediyorum. Amacým, kullanýcý dostu ve yaratýcý mobil uygulamalar 
        geliþtirmek için teknolojiyi kullanarak insanlarýn hayatýný kolaylaþtýrmaktýr.

        Benim için yazýlým geliþtirme sadece bir iþ deðil, ayný zamanda bir tutku. Ýþime duyduðum tutku, 
        her proje üzerinde özenle çalýþmamý, yenilikçi çözümler bulmamý ve en iyi kullanýcý deneyimini 
        sunmamý saðlýyor. Öðrenci olarak sürekli öðrenmeye ve kendimi geliþtirmeye büyük bir önem veriyorum. 
        Teknolojideki son trendleri takip etmek ve en iyi uygulamalarý kullanmak için çaba sarf ediyorum.

        Benimle iþbirliði yapmak veya projeler hakkýnda daha fazla bilgi almak isteyenler için bir iletiþim kanalý sunuyorum. 
        Ýletiþim butonunu kullanarak bana ulaþabilir veya sosyal medya hesaplarýmdan benimle baðlantý kurabilirsiniz.

        Teþekkür ederim, ziyaretiniz için! Umarým uygulamamý beðenirsiniz.
        """;

    private async void OnClickedLinkedIn()
    {
		await Launcher.OpenAsync("https://www.linkedin.com/in/hsynkbulut/");
    }

    private async void OnClickedGitHub()
    {
        await Launcher.OpenAsync("https://github.com/hsynkbulut");
    }

    private async void OnClickedMedium()
    {
        await Launcher.OpenAsync("https://medium.com/@hsynkbulut");
    }

    private async void OnClickedContact(object sender, EventArgs e)
    {
        await Launcher.OpenAsync("mailto:hsyn.kbulut@gmail.com");
    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//homepage");
    }
}