using Microsoft.Maui.Controls.Shapes;

namespace NezihBankMAUI.Views.CustomerView;

public partial class TransactionPage : ContentPage
{
	public TransactionPage()
    {
        InitializeComponent();
	}
    
    private void ToMyAccountOnFrame_Tapped(object sender, TappedEventArgs e)
    {
        Navigation.PushModalAsync(new Views.CustomerView.ToMyAccount());
    }

    private void ToAnotherAccountOnFrame_Tapped(object sender, TappedEventArgs e)
    {
        Navigation.PushModalAsync(new Views.CustomerView.ToAnotherAccount());
    }

    private async void PaymentsOnFrame_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new Views.CustomerView.PaymentsPage());
    }

    private async void DonationTransactionsOnFrame_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new Views.CustomerView.DonationPage());
    }
}