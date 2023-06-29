using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NezihBankMAUI.Helper
{
    internal class UrlHelper
    {
        public static string BaseAddress = 
            DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:7196" : "https://localhost:7196";

        public static string AnnouncementUrl = $"{BaseAddress}/duyurular";
        public static string BankManagerUrl = $"{BaseAddress}/bankamuduru";
        public static string PersonalCustomerUrl = $"{BaseAddress}/bireyselmusteri";
        public static string CustomerAccountsUrl = $"{BaseAddress}/musteribankahesaplari";
        public static string PersonalTransactionsUrl = $"{BaseAddress}/bireyselislemler";
    }
}
