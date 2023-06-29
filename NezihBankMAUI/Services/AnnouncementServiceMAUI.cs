using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NezihBankMAUI.Helper;
using NezihBankMAUI.Models;

namespace NezihBankMAUI.Services
{
    internal interface IAnnouncementServiceMAUI
    {
        Task<List<AnnouncementMAUI>> GetAnnouncements();
        Task AddAnnouncement(AnnouncementMAUI announcement);
        Task UpdateAnnouncement(AnnouncementMAUI announcement);
        Task DeleteAnnouncement(int announcementId);
    }

    public class AnnouncementServiceMAUI : IAnnouncementServiceMAUI
    {
        HttpClient httpClient;

        JsonSerializerOptions jsonSerializerOptions;

        public AnnouncementServiceMAUI()
        {
#if (DEBUG && ANDROID)
            HttpsClientHandlerService handler = new HttpsClientHandlerService();
            httpClient = new HttpClient(handler.GetPlatformMessageHandler());
#else
            httpClient = new HttpClient();
#endif

            /* jsonSerializerOptions: C# İsimlendirme Kurallarına uyum
            #region HTTP isteğinde bulunma UZUN YOL için bunun kullanımı gerekli
            //C# İsimlendirme Kurallarını Bozan Bir Sorun Var. Bunun Çözümü: JsonSerializerOptions
            jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            #endregion
            */
        }

        public async Task AddAnnouncement(AnnouncementMAUI announcement)
        {
            var json = JsonSerializer.Serialize(announcement);
            JsonContent jsonContent = JsonContent.Create(announcement);
            var response = await httpClient.PostAsync(UrlHelper.AnnouncementUrl, jsonContent);

            if (response.IsSuccessStatusCode)
            {

            }
        }

        public async Task<List<AnnouncementMAUI>> GetAnnouncements()
        {
            /*HTTP isteğinde bulunma (Uzun yol)
            #region HTTP isteğinde bulunma (Uzun yol)
            //HTTP isteğinde bulunmak
            var response = await httpClient.GetAsync(UrlHelper.DuyuruUrl);

            //if (response.IsSuccessStatusCode): başarıyla bu datayı alabildik mi?
            if (response.IsSuccessStatusCode)
            {
                string jsonContent = await response.Content.ReadAsStringAsync();

                //JSON'ı .NET nesneleri olarak okuma (seri durumdan çıkarma): JsonSerializer.Deserialize
                var sonuc = JsonSerializer.Deserialize<List<Duyuru>>(jsonContent, jsonSerializerOptions);
                return sonuc;
            }
            return new List<Duyuru>();
            #endregion
            */

            var result = await httpClient.GetFromJsonAsync<List<AnnouncementMAUI>>(UrlHelper.AnnouncementUrl);
            return result;  
        }

        public async Task DeleteAnnouncement(int announcementId)
        {
            var url = UrlHelper.AnnouncementUrl + $"/{announcementId}";
            await httpClient.DeleteAsync(url);
        }

        public async Task UpdateAnnouncement(AnnouncementMAUI announcement)
        {
            var json = JsonSerializer.Serialize(announcement);
            JsonContent jsonContent = JsonContent.Create(announcement);

            var url = $"{UrlHelper.AnnouncementUrl}/{announcement.Id}";
            var response = await httpClient.PutAsync(url, jsonContent);

            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                
            }
        }
    }
}
