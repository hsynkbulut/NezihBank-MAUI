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
    internal interface IBankManagerServiceMAUI
    {
        Task<List<BankManagerMAUI>> GetBankManagers();
        Task AddBankManager(BankManagerMAUI bankManager);
        Task DeleteBankManager(int bankManagerId);
        Task UpdateBankManager(BankManagerMAUI bankManager);
    }

    public class BankManagerServiceMAUI : IBankManagerServiceMAUI
    {
        HttpClient httpClient;

        JsonSerializerOptions jsonSerializerOptions;

        public BankManagerServiceMAUI()
        {
#if (DEBUG && ANDROID)
            HttpsClientHandlerService handler = new HttpsClientHandlerService();
            httpClient = new HttpClient(handler.GetPlatformMessageHandler());
#else
            httpClient = new HttpClient();
#endif
        }

        public async Task<List<BankManagerMAUI>> GetBankManagers()
        {
            var result = await httpClient.GetFromJsonAsync<List<BankManagerMAUI>>(UrlHelper.BankManagerUrl);
            return result;
        }

        public async Task AddBankManager(BankManagerMAUI bankManager)
        {
            var json = JsonSerializer.Serialize(bankManager);
            JsonContent jsonContent = JsonContent.Create(bankManager);
            var response = await httpClient.PostAsync(UrlHelper.BankManagerUrl, jsonContent);

            if (response.IsSuccessStatusCode)
            {

            }
        }

        public async Task UpdateBankManager(BankManagerMAUI bankManager)
        {
            var json = JsonSerializer.Serialize(bankManager);
            JsonContent jsonContent = JsonContent.Create(bankManager);

            var url = $"{UrlHelper.BankManagerUrl}/{bankManager.BankManagerId}";
            var response = await httpClient.PutAsync(url, jsonContent);

            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                
            }
        }

        public async Task DeleteBankManager(int bankManagerId)
        {
            var url = UrlHelper.BankManagerUrl + $"/{bankManagerId}";
            await httpClient.DeleteAsync(url);
        }

    }
}
