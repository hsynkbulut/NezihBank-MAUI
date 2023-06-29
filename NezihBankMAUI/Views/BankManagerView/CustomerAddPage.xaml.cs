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
            DisplayAlert("UYARI", "Müþteri kimlik numarasý girilmedi veya 11 rakamdan oluþmuyor!", "Tamam");
            return false;
        }

        if (string.IsNullOrEmpty(txtName.Text) || ContainsDigitsOrSymbols(txtName.Text))
        {
            DisplayAlert("UYARI", "Müþteri adý girilmedi veya geçersiz karakterler içeriyor!", "Tamam");
            return false;
        }

        if (string.IsNullOrEmpty(txtSurname.Text) || ContainsDigitsOrSymbols(txtSurname.Text))
        {
            DisplayAlert("UYARI", "Müþteri soyadý girilmedi veya geçersiz karakterler içeriyor!", "Tamam");
            return false;
        }

        if (string.IsNullOrEmpty(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
        {
            DisplayAlert("UYARI", "Müþteri mail adresi girilmedi veya geçersiz bir mail adresi!", "Tamam");
            return false;
        }

        if (string.IsNullOrEmpty(txtPhoneNumber.Text) || !IsValidPhoneNumber(txtPhoneNumber.Text))
        {
            DisplayAlert("UYARI", "Müþteri telefon numarasý girilmedi veya numara geçersiz!", "Tamam");
            return false;
        }

        if (string.IsNullOrEmpty(txtPassword.Text) || !int.TryParse(txtPassword.Text, out int password) || txtPassword.Text.Length != 6)
        {
            DisplayAlert("UYARI", "Müþteri þifresi girilmedi veya geçersiz bir þifre içeriyor!", "Tamam");
            return false;
        }

        if (string.IsNullOrEmpty(txtAddress.Text))
        {
            DisplayAlert("UYARI", "Müþteri ev adresi girilmedi!", "Tamam");
            return false;
        }

        if (pickerCity.SelectedItem == null)
        {
            DisplayAlert("UYARI", "Müþterinin þehri seçilmedi!", "Tamam");
            return false;
        }

        if (pickerDistrict.SelectedItem == null)
        {
            DisplayAlert("UYARI", "Müþterinin ilçesi seçilmedi!", "Tamam");
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
            await DisplayAlert("UYARI", "Lütfen tüm bilgileri eksiksiz ve doðru bir þekilde girin.", "Tamam");
        }
        */

        // Tüm giriþ alanlarýný temizleyelim
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
            await DisplayAlert("HATA", "Geçersiz Müþteri ID/Numarasý!", "Tamam");
            return;
        }

        if (ValidateInput())
        {
            // Veritabanýndan tüm müþterileri alalým
            List<PersonalCustomersMAUI> customers = await personalCustomerService.GetPersonalCustomers();

            // Müþteri ID'sine göre güncellenecek müþteriyi bulalým
            PersonalCustomersMAUI customerFind = customers.FirstOrDefault(a => a.Id == customerId);

            if (customerFind != null && customerFind.Id == customerId)
            {
                // Kullanýcýnýn girdiði bilgileri alýp güncelleme yapmak için updateCustomer nesnesini oluþturalým
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

                // Veritabanýndan ilgili müþterinin bilgisini güncelleyelim
                await personalCustomerService.UpdateCustomer(updateCustomer);

                await DisplayAlert("BÝLGÝ", "Müþteri güncelleme iþlemi baþarýlý", "Tamam");
            }
            else
            {
                await DisplayAlert("UYARI", "Böyle bir Müþteri numarasý/ID'sine sahip bir müþteri yok!", "Tamam");
            }

        }
        /*
        else
        {
            await DisplayAlert("UYARI", "Lütfen tüm bilgileri doðru bir þekilde girin.", "Tamam");
        }
        */

        // Tüm giriþ alanlarýný temizleyelim
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