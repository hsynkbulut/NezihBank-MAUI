using NezihBankMAUI.Models;
using NezihBankMAUI.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NezihBankMAUI.Views.BankManagerView;

public partial class CustomerAddPage : ContentPage
{
    private readonly ICityService cityService;
    private readonly IDistrictService districtService;

    public ICommand KaydiZatenVarCommand { get; private set; }

    ObservableCollection<PersonalCustomersMAUI> personalCustomers;
    IPersonalCustomerServiceMAUI personalCustomerService;

    public CustomerAddPage()
	{
		InitializeComponent();

        cityService = new CityService();
        districtService = new DistrictService();

        pickerIlDoldur();

        KaydiZatenVarCommand = new Command(OnKaydiVarClicked);

        personalCustomerService = new PersonalCustomerServiceMAUI();
        personalCustomers = new ObservableCollection<PersonalCustomersMAUI>();

        this.BindingContext = this;
    }

    private void pickerIlDoldur()
    {
        pickerCity.ItemsSource = cityService.GetCities();
    }

    private void pickerCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        var seciliEleman = pickerCity.SelectedItem as City;
        if (seciliEleman is not null)
        {
            var districts = districtService.GetDistrictsbyIlId(seciliEleman.Id);
            pickerDistrict.ItemsSource = districts;
        }

    }

    private async void OnKaydiVarClicked()
    {
        //Navigation.PopModalAsync();
        await Shell.Current.GoToAsync("//managerhome");
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrEmpty(txtIdentityNumber.Text) || !long.TryParse(txtIdentityNumber.Text, out long identityNumber) || txtIdentityNumber.Text.Length != 11)
        {
            DisplayAlert("UYARI", "M��teri kimlik numaras� girilmedi veya 11 rakamdan olu�muyor!", "Tamam");
            return false;
        }

        if (string.IsNullOrEmpty(txtName.Text) || ContainsDigitsOrSymbols(txtName.Text))
        {
            DisplayAlert("UYARI", "M��teri ad� girilmedi veya ge�ersiz karakterler i�eriyor!", "Tamam");
            return false;
        }

        if (string.IsNullOrEmpty(txtSurname.Text) || ContainsDigitsOrSymbols(txtSurname.Text))
        {
            DisplayAlert("UYARI", "M��teri soyad� girilmedi veya ge�ersiz karakterler i�eriyor!", "Tamam");
            return false;
        }

        if (string.IsNullOrEmpty(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
        {
            DisplayAlert("UYARI", "M��teri mail adresi girilmedi veya ge�ersiz bir mail adresi!", "Tamam");
            return false;
        }

        if (string.IsNullOrEmpty(txtPhoneNumber.Text) || !IsValidPhoneNumber(txtPhoneNumber.Text))
        {
            DisplayAlert("UYARI", "M��teri telefon numaras� girilmedi veya numara ge�ersiz!", "Tamam");
            return false;
        }

        if (string.IsNullOrEmpty(txtPassword.Text) || !int.TryParse(txtPassword.Text, out int password) || txtPassword.Text.Length != 6)
        {
            DisplayAlert("UYARI", "M��teri �ifresi girilmedi veya ge�ersiz bir �ifre i�eriyor!", "Tamam");
            return false;
        }

        if (string.IsNullOrEmpty(txtAddress.Text))
        {
            DisplayAlert("UYARI", "M��teri ev adresi girilmedi!", "Tamam");
            return false;
        }

        if (pickerCity.SelectedItem == null)
        {
            DisplayAlert("UYARI", "M��terinin �ehri se�ilmedi!", "Tamam");
            return false;
        }

        if (pickerDistrict.SelectedItem == null)
        {
            DisplayAlert("UYARI", "M��terinin il�esi se�ilmedi!", "Tamam");
            return false;
        }

        return true;
    }

    private bool IsValidEmail(string email)
    {
        return email.Contains("@");
    }

    private bool IsValidPhoneNumber(string phoneNumber)
    {
        return phoneNumber.Length >= 10;
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

    private async void personalCustomerRegister_Clicked(object sender, EventArgs e)
    {
        if (ValidateInput())
        {
            var newCustomer = new PersonalCustomersMAUI()
            {
                Id = 0, //Id = Guid.NewGuid(),
                IdentityNumber = txtIdentityNumber.Text,
                Name = txtName.Text,
                Surname = txtSurname.Text,
                Password = txtPassword.Text,
                Email = txtEmail.Text,
                PhoneNumber = txtPhoneNumber.Text,
                BirthDate = birthDatePicker.Date,
                Address = txtAddress.Text,
                City = pickerCity.SelectedItem.ToString(),
                District = pickerDistrict.SelectedItem.ToString()
            };

            await personalCustomerService.AddCustomer(newCustomer);
            //await GetAnnouncements();
        }
        /*
        else
        {
            await DisplayAlert("UYARI", "L�tfen t�m bilgileri eksiksiz ve do�ru bir �ekilde girin.", "Tamam");
        }
        */

        // T�m giri� alanlar�n� temizleyelim
        txtIdentityNumber.Text = string.Empty;
        txtName.Text = string.Empty;
        txtSurname.Text = string.Empty;
        txtPassword.Text = string.Empty;
        txtEmail.Text = string.Empty;
        txtPhoneNumber.Text = string.Empty;
        txtAddress.Text = string.Empty;
        pickerCity.SelectedItem = null;
        pickerDistrict.SelectedItem = null;
    }

    private async void personalCustomerUpdate_Clicked(object sender, EventArgs e)
    {
        int customerId;
        if (!int.TryParse(txtCustomerId.Text, out customerId))
        {
            await DisplayAlert("HATA", "Ge�ersiz M��teri ID/Numaras�!", "Tamam");
            return;
        }

        if (ValidateInput())
        {
            // Veritaban�ndan t�m m��terileri alal�m
            List<PersonalCustomersMAUI> customers = await personalCustomerService.GetPersonalCustomers();

            // M��teri ID'sine g�re g�ncellenecek m��teriyi bulal�m
            PersonalCustomersMAUI customerFind = customers.FirstOrDefault(a => a.Id == customerId);

            if (customerFind != null && customerFind.Id == customerId)
            {
                // Kullan�c�n�n girdi�i bilgileri al�p g�ncelleme yapmak i�in updateCustomer nesnesini olu�tural�m
                var updateCustomer = new PersonalCustomersMAUI()
                {
                    Id = customerId,
                    IdentityNumber = txtIdentityNumber.Text,
                    Name = txtName.Text,
                    Surname = txtSurname.Text,
                    Password = txtPassword.Text,
                    Email = txtEmail.Text,
                    PhoneNumber = txtPhoneNumber.Text,
                    BirthDate = birthDatePicker.Date,
                    Address = txtAddress.Text,
                    City = pickerCity.SelectedItem.ToString(),
                    District = pickerDistrict.SelectedItem.ToString()
                };

                // Veritaban�ndan ilgili m��terinin bilgisini g�ncelleyelim
                await personalCustomerService.UpdateCustomer(updateCustomer);

                await DisplayAlert("B�LG�", "M��teri g�ncelleme i�lemi ba�ar�l�", "Tamam");
            }
            else
            {
                await DisplayAlert("UYARI", "B�yle bir M��teri numaras�/ID'sine sahip bir m��teri yok!", "Tamam");
            }

        }
        /*
        else
        {
            await DisplayAlert("UYARI", "L�tfen t�m bilgileri do�ru bir �ekilde girin.", "Tamam");
        }
        */

        // T�m giri� alanlar�n� temizleyelim
        txtCustomerId.Text = string.Empty;
        txtIdentityNumber.Text = string.Empty;
        txtName.Text = string.Empty;
        txtSurname.Text = string.Empty;
        txtPassword.Text = string.Empty;
        txtEmail.Text = string.Empty;
        txtPhoneNumber.Text = string.Empty;
        txtAddress.Text = string.Empty;
        pickerCity.SelectedItem = null;
        pickerDistrict.SelectedItem = null;
    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        //await Navigation.PopModalAsync();
        //await Shell.Current.GoToAsync("..");
        //await Shell.Current.Navigation.PopModalAsync();
        await Shell.Current.GoToAsync("//managerhome");
    }
}