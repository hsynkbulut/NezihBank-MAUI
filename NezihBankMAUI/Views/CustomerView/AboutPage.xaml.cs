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
        Merhaba! Ben H�seyin, Isparta Uygulamal� Bilimler �niversitesi Bilgisayar M�hendisli�i 2.s�n�f 
        ��rencisiyim ve ayn� zamanda bir mobil uygulama geli�tiricisi aday�y�m. Dart ve C# dilleri 
        �zerine �al���yorum. Flutter ve .NET MAUI gibi teknolojileri ��renerek mobil uygulama geli�tirme 
        konusunda kendimi geli�tirmeye devam ediyorum. Amac�m, kullan�c� dostu ve yarat�c� mobil uygulamalar 
        geli�tirmek i�in teknolojiyi kullanarak insanlar�n hayat�n� kolayla�t�rmakt�r.

        Benim i�in yaz�l�m geli�tirme sadece bir i� de�il, ayn� zamanda bir tutku. ��ime duydu�um tutku, 
        her proje �zerinde �zenle �al��mam�, yenilik�i ��z�mler bulmam� ve en iyi kullan�c� deneyimini 
        sunmam� sa�l�yor. ��renci olarak s�rekli ��renmeye ve kendimi geli�tirmeye b�y�k bir �nem veriyorum. 
        Teknolojideki son trendleri takip etmek ve en iyi uygulamalar� kullanmak i�in �aba sarf ediyorum.

        Benimle i�birli�i yapmak veya projeler hakk�nda daha fazla bilgi almak isteyenler i�in bir ileti�im kanal� sunuyorum. 
        �leti�im butonunu kullanarak bana ula�abilir veya sosyal medya hesaplar�mdan benimle ba�lant� kurabilirsiniz.

        Te�ekk�r ederim, ziyaretiniz i�in! Umar�m uygulamam� be�enirsiniz.
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