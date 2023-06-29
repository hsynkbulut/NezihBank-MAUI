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
    internal interface IPersonalTransactionServiceMAUI
    {
        Task<List<PersonalTransactionsMAUI>> GetAccountTransactions(int accountId);
        Task AddTransaction(PersonalTransactionsMAUI transaction);
        Task UpdateTransaction(PersonalTransactionsMAUI transaction);
        Task DeleteTransaction(int transactionId);
    }

    public class PersonalTransactionServiceMAUI : IPersonalTransactionServiceMAUI
    {
        HttpClient httpClient;

        JsonSerializerOptions jsonSerializerOptions;

        public PersonalTransactionServiceMAUI()
        {
#if (DEBUG && ANDROID)
            HttpsClientHandlerService handler = new HttpsClientHandlerService();
            httpClient = new HttpClient(handler.GetPlatformMessageHandler());
#else
            httpClient = new HttpClient();
#endif
        }

        public async Task AddTransaction(PersonalTransactionsMAUI transaction)
        {
            var json = JsonSerializer.Serialize(transaction);
            JsonContent jsonContent = JsonContent.Create(transaction);
            var response = await httpClient.PostAsync(UrlHelper.PersonalTransactionsUrl, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                
            }
        }

        public async Task<List<PersonalTransactionsMAUI>> GetAccountTransactions(int accountId)
        {
            var url = $"{UrlHelper.PersonalTransactionsUrl}/hesap/{accountId}";
            var result = await httpClient.GetFromJsonAsync<List<PersonalTransactionsMAUI>>(url);
            return result;
        }

        public async Task UpdateTransaction(PersonalTransactionsMAUI transaction)
        {
            var json = JsonSerializer.Serialize(transaction);
            JsonContent jsonContent = JsonContent.Create(transaction);

            var url = $"{UrlHelper.PersonalTransactionsUrl}/{transaction.TransactionId}";
            var response = await httpClient.PutAsync(url, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                
            }
            else
            {
                
            }
        }

        public async Task DeleteTransaction(int transactionId)
        {
            var url = $"{UrlHelper.PersonalTransactionsUrl}/{transactionId}";
            await httpClient.DeleteAsync(url);
        }
    }
}
