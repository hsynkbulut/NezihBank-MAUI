using NezihBankMAUI.Models;
using NezihBankMAUI.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NezihBankMAUI.Views.Startup;

public partial class BankManagerRegister : ContentPage
{
    public ICommand KaydiZatenVarCommand { get; private set; }

    ObservableCollection<BankManagerMAUI> bankManagers;
    IBankManagerServiceMAUI bankManagerService;

    public BankManagerRegister()
	{
		InitializeComponent();

        KaydiZatenVarCommand = new Command(OnKaydiVarClicked);

        bankManagerService = new BankManagerServiceMAUI();
        bankManagers = new ObservableCollection<BankManagerMAUI>();

        this.BindingContext = this;
    }

    private async void OnKaydiVarClicked()
    {
        await Shell.Current.GoToAsync("//bankManagerLogin");
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrEmpty(txtIdentityNumber.Text) || !long.TryParse(txtIdentityNumber.Text, out long identityNumber) || txtIdentityNumber.Text.Length != 11)
        {
            return false;
        }

        if (string.IsNullOrEmpty(txtName.Text) || ContainsDigitsOrSymbols(txtName.Text))
        {
            return false;
        }

        if (string.IsNullOrEmpty(txtSurname.Text) || ContainsDigitsOrSymbols(txtSurname.Text))
        {
            return false;
        }

        if (string.IsNullOrEmpty(txtPassword.Text) || !int.TryParse(txtPassword.Text, out int password) || txtPassword.Text.Length != 6)
        {
            return false;
        }

        return true;
    }

    private bool ContainsDigitsOrSymbols(string text)
    {
        foreach (char c in text)
        {
            if (char.IsDigit(c) || char.IsSymbol(c))
            {
                return true;
            }
        }
        return false;
    }

    private async void bankManagerRegister_Clicked(object sender, EventArgs e)
    {
        if (ValidateInput())
        {
            var duyuru = new BankManagerMAUI()
            {
                //BankManagerId = int.Parse(txtUserCode.Text),
                BankManagerId = 0,
                IdentityNumber = txtIdentityNumber.Text,
                Name = txtName.Text,
                Surname = txtSurname.Text,
                Password = txtPassword.Text
            };

            await bankManagerService.AddBankManager(duyuru);
            await Shell.Current.GoToAsync("//bankManagerLogin");
            //await GetAnnouncements();

            // Tüm giriþ alanlarýný temizleyelim
            txtIdentityNumber.Text = string.Empty;
            txtName.Text = string.Empty;
            txtSurname.Text = string.Empty;
            txtPassword.Text = string.Empty;
        }
        else
        {
            await DisplayAlert("UYARI", "Lütfen tüm bilgileri doðru bir þekilde girin.", "Tamam");
        }
    }

}