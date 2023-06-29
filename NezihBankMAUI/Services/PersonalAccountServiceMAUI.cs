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
    internal interface IPersonalAccountServiceMAUI
    {
        Task<List<PersonalAccountsMAUI>> GetCustomerAccounts();
        Task AddAccount(PersonalAccountsMAUI personalAccounts);
        Task DeleteAccount(int personalAccountsId);

        Task UpdateAccount(PersonalAccountsMAUI personalAccounts);
    }

    public class PersonalAccountServiceMAUI : IPersonalAccountServiceMAUI
    {
        HttpClient httpClient;

        JsonSerializerOptions jsonSerializerOptions;

        public PersonalAccountServiceMAUI()
        {
#if (DEBUG && ANDROID)
            HttpsClientHandlerService handler = new HttpsClientHandlerService();
            httpClient = new HttpClient(handler.GetPlatformMessageHandler());
#else
            httpClient = new HttpClient();
#endif
        }

        public async Task AddAccount(PersonalAccountsMAUI personalAccounts)
        {
            var json = JsonSerializer.Serialize(personalAccounts);
            JsonContent jsonContent = JsonContent.Create(personalAccounts);
            var response = await httpClient.PostAsync(UrlHelper.CustomerAccountsUrl, jsonContent);

            if (response.IsSuccessStatusCode)
            {

            }
        }

        public async Task<List<PersonalAccountsMAUI>> GetCustomerAccounts()
        {
            var result = await httpClient.GetFromJsonAsync<List<PersonalAccountsMAUI>>(UrlHelper.CustomerAccountsUrl);
            return result;
        }

        public async Task UpdateAccount(PersonalAccountsMAUI personalAccounts)
        {
            var json = JsonSerializer.Serialize(personalAccounts);
            JsonContent jsonContent = JsonContent.Create(personalAccounts);

            var url = $"{UrlHelper.CustomerAccountsUrl}/{personalAccounts.AccountId}";
            var response = await httpClient.PutAsync(url, jsonContent);

            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                
            }
        }

        public async Task DeleteAccount(int personalAccountsId)
        {
            var url = UrlHelper.CustomerAccountsUrl + $"/{personalAccountsId}";
            await httpClient.DeleteAsync(url);
        }
    }
}
