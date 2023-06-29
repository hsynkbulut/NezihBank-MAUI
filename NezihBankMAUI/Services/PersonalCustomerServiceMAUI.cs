using NezihBankMAUI.Helper;
using NezihBankMAUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NezihBankMAUI.Services
{
    internal interface IPersonalCustomerServiceMAUI
    {
        Task<List<PersonalCustomersMAUI>> GetPersonalCustomers();
        Task AddCustomer(PersonalCustomersMAUI personalCustomers);
        Task DeleteCustomer(int personalCustomersId);

        Task UpdateCustomer(PersonalCustomersMAUI personalCustomers);
    }

    public class PersonalCustomerServiceMAUI : IPersonalCustomerServiceMAUI
    {
        HttpClient httpClient;

        JsonSerializerOptions jsonSerializerOptions;

        public PersonalCustomerServiceMAUI()
        {
#if (DEBUG && ANDROID)
            HttpsClientHandlerService handler = new HttpsClientHandlerService();
            httpClient = new HttpClient(handler.GetPlatformMessageHandler());
#else
            httpClient = new HttpClient();
#endif
        }

        public async Task AddCustomer(PersonalCustomersMAUI personalCustomers)
        {
            var json = JsonSerializer.Serialize(personalCustomers);
            JsonContent jsonContent = JsonContent.Create(personalCustomers);
            var response = await httpClient.PostAsync(UrlHelper.PersonalCustomerUrl, jsonContent);

            if (response.IsSuccessStatusCode)
            {

            }
        }

        public async Task<List<PersonalCustomersMAUI>> GetPersonalCustomers()
        {
            var result = await httpClient.GetFromJsonAsync<List<PersonalCustomersMAUI>>(UrlHelper.PersonalCustomerUrl);
            return result;
        }

        public async Task UpdateCustomer(PersonalCustomersMAUI personalCustomers)
        {
            var json = JsonSerializer.Serialize(personalCustomers);
            JsonContent jsonContent = JsonContent.Create(personalCustomers);

            var url = $"{UrlHelper.PersonalCustomerUrl}/{personalCustomers.Id}";
            var response = await httpClient.PutAsync(url, jsonContent);

            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                
            }
        }

        public async Task DeleteCustomer(int personalCustomersId)
        {
            var url = UrlHelper.PersonalCustomerUrl + $"/{personalCustomersId}";
            await httpClient.DeleteAsync(url);
        }

    }
}
