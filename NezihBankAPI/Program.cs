using NezihBankAPI.Data;
using NezihBankAPI.Models;
using NezihBankAPI.Services;

namespace NezihBankAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            #region Banka Yetkilisi
            //Banka yetkililerini Listelemek: MapGet
            app.MapGet("/bankamuduru", () =>
            {
                BankDbContext context = new BankDbContext();
                var bankManagerService = new BankManagerServiceAPI(context);
                return bankManagerService.GetAllBankManagers();
            });

            //ID'sine göre belirli yetkiliyi getir
            app.MapGet("/bankamuduru/{id}", (int id) =>
            {
                BankDbContext context = new BankDbContext();
                var bankManagerService = new BankManagerServiceAPI(context);
                return bankManagerService.GetBankManager(id);
            });

            //Banka yetkilisi Eklemek: MapPost
            app.MapPost("/bankamuduru", (BankManagerAPI bankManager) =>
            {
                BankDbContext context = new BankDbContext();
                var bankManagerService = new BankManagerServiceAPI(context);
                bankManagerService.AddBankManager(bankManager);
                return "Banka müdürü eklendi.";
            });

            //Banka yetkilisi Silmek: MapDelete
            app.MapDelete("/bankamuduru/{id}", (int id) =>
            {
                BankDbContext context = new BankDbContext();
                var bankManagerService = new BankManagerServiceAPI(context);
                bankManagerService.DeleteBankManager(id);
                return "Banka müdürü silindi.";
            });

            //Banka yetkili bilgisini Güncellemek: MapPut
            app.MapPut("/bankamuduru/{id}", (BankManagerAPI updatedBankManager) =>
            {
                BankDbContext context = new BankDbContext();
                var bankManagerService = new BankManagerServiceAPI(context);
                bankManagerService.UpdateBankManager(updatedBankManager);
                return "Banka müdürü bilgileri güncellendi.";
            });
            #endregion

            #region Duyurular / Kampanya
            //Duyurularý Listelemek: MapGet
            app.MapGet("/duyurular", () =>
            {
                BankDbContext context = new BankDbContext();
                var announcementService = new AnnouncementServiceAPI(context);
                return announcementService.GetAllAnnouncements();
            });

            // Bugünün Duyurularýný Listelemek: MapGet
            app.MapGet("/duyurular/yeni", () =>
            {
                BankDbContext context = new BankDbContext();
                var announcementService = new AnnouncementServiceAPI(context);
                return announcementService.GetTodaysAnnouncements();
            });

            // Belirli bir Duyuruyu Listelemek: MapGet
            app.MapGet("/duyurular/{id}", (int id) =>
            {
                BankDbContext context = new BankDbContext();
                var announcementService = new AnnouncementServiceAPI(context);
                return announcementService.GetAnnouncement(id);
            });

            //Duyuru Eklemek: MapPost
            app.MapPost("/duyurular", (AnnouncementAPI duyuru) =>
            {
                BankDbContext context = new BankDbContext();
                var announcementService = new AnnouncementServiceAPI(context);
                announcementService.AddAnnouncement(duyuru);
            });

            //Duyuru güncellemek: MapPut
            app.MapPut("/duyurular/{id}", (AnnouncementAPI updatedAnnouncement) =>
            {
                BankDbContext context = new BankDbContext();
                var announcementService = new AnnouncementServiceAPI(context);
                announcementService.UpdateAnnouncement(updatedAnnouncement);
                return "Kampanya güncellendi.";
            });

            //Duyuru Silmek: MapDelete
            app.MapDelete("/duyurular/{id}", (int id) =>
            {
                BankDbContext context = new BankDbContext();
                var announcementService = new AnnouncementServiceAPI(context);
                announcementService.DeleteAnnouncement(id);
            });
            #endregion

            #region Bireysel Müþteri
            //Bireysel Müþterileri Listelemek: MapGet
            app.MapGet("/bireyselmusteri", () =>
            {
                BankDbContext context = new BankDbContext();
                var personalCustomerService = new PersonalCustomerServiceAPI(context);
                return personalCustomerService.GetAllCustomers();
            });

            //ID'sine göre belirli müþteriyi getir
            app.MapGet("/bireyselmusteri/{id}", (int id) =>
            {
                BankDbContext context = new BankDbContext();
                var personalCustomerService = new PersonalCustomerServiceAPI(context);
                return personalCustomerService.GetCustomer(id);
            });

            //Müþteri Eklemek: MapPost
            app.MapPost("/bireyselmusteri", (PersonalCustomersAPI personalCustomers) =>
            {
                BankDbContext context = new BankDbContext();
                var personalCustomerService = new PersonalCustomerServiceAPI(context);
                personalCustomerService.AddPersonalCustomer(personalCustomers);
                return "Banka müdürü eklendi.";
            });

            //Müþteri Silmek: MapDelete
            app.MapDelete("/bireyselmusteri/{id}", (int id) =>
            {
                BankDbContext context = new BankDbContext();
                var personalCustomerService = new PersonalCustomerServiceAPI(context);
                personalCustomerService.DeletePersonalCustomer(id);
                return "Banka müdürü silindi.";
            });

            //Müþteri bilgisi güncellemek: MapPut
            app.MapPut("/bireyselmusteri/{id}", (PersonalCustomersAPI updatedCustomers) =>
            {
                BankDbContext context = new BankDbContext();
                var personalCustomerService = new PersonalCustomerServiceAPI(context);
                personalCustomerService.UpdatePersonalCustomer(updatedCustomers);
                return "Müþteri bilgileri güncellendi.";
            });
            #endregion

            #region Hesaplar
            //Bireysel Müþteri hesaplarýný Listelemek: MapGet
            app.MapGet("/musteribankahesaplari", () =>
            {
                BankDbContext context = new BankDbContext();
                var personalAccountService = new PersonalAccountServiceAPI(context);
                return personalAccountService.GetAllAccounts();
            });

            //ID'sine göre belirli müþteri hesabýný getir
            app.MapGet("/musteribankahesaplari/{id}", (int id) =>
            {
                BankDbContext context = new BankDbContext();
                var personalAccountService = new PersonalAccountServiceAPI(context);
                return personalAccountService.GetAccount(id);
            });

            //Müþteri hesabý Eklemek: MapPost
            app.MapPost("/musteribankahesaplari", (PersonalAccountsAPI personalAccounts) =>
            {
                BankDbContext context = new BankDbContext();
                var personalAccountService = new PersonalAccountServiceAPI(context);
                personalAccountService.AddAccount(personalAccounts);
                return "Müþteri banka hesabý eklendi.";
            });

            //Müþteri hesabý Güncellemek: MapPut
            app.MapPut("/musteribankahesaplari/{id}", (PersonalAccountsAPI personalAccounts) =>
            {
                BankDbContext context = new BankDbContext();
                var personalAccountService = new PersonalAccountServiceAPI(context);
                personalAccountService.UpdateAccount(personalAccounts);
                return "Müþteri banka hesabý güncellendi.";
            });

            //Müþteri hesabý Silmek: MapDelete
            app.MapDelete("/musteribankahesaplari/{id}", (int id) =>
            {
                BankDbContext context = new BankDbContext();
                var personalAccountService = new PersonalAccountServiceAPI(context);
                personalAccountService.DeleteAccount(id);
                return "Müþteri banka hesabý silindi.";
            });
            #endregion

            #region Bireysel Ýþlemler
            // Tüm bireysel iþlemleri getir
            app.MapGet("/bireyselislemler", () =>
            {
                BankDbContext context = new BankDbContext();
                var personalTransactionService = new PersonalTransactionServiceAPI(context);
                return personalTransactionService.GetAllTransactions();
            });
            
            // ID'sine göre belirli iþlemi getir
            app.MapGet("/bireyselislemler/{id}", (int id) =>
            {
                BankDbContext context = new BankDbContext();
                var personalTransactionService = new PersonalTransactionServiceAPI(context);
                return personalTransactionService.GetTransactionById(id);
            });

            // Hesap ID'sine Göre Bireysel Ýþlemleri Getir
            app.MapGet("/bireyselislemler/hesap/{accountId}", (int accountId) =>
            {
                BankDbContext context = new BankDbContext();
                var personalTransactionService = new PersonalTransactionServiceAPI(context);
                return personalTransactionService.GetTransactionsByAccountId(accountId);
            });

            // Bireysel Ýþlem Ekle: MapPost
            app.MapPost("/bireyselislemler", (PersonalTransactionsAPI transaction) =>
            {
                BankDbContext context = new BankDbContext();
                var personalTransactionService = new PersonalTransactionServiceAPI(context);
                personalTransactionService.AddTransaction(transaction);
                return "Bireysel iþlem eklendi.";
            });

            // Bireysel Ýþlem Güncelle: MapPut
            app.MapPut("/bireyselislemler/{id}", (PersonalTransactionsAPI updatedTransaction) =>
            {
                BankDbContext context = new BankDbContext();
                var personalTransactionService = new PersonalTransactionServiceAPI(context);
                personalTransactionService.UpdateTransaction(updatedTransaction);
                return "Bireysel iþlem güncellendi.";
            });

            // Bireysel Ýþlem Sil: MapDelete
            app.MapDelete("/bireyselislemler/{id}", (int id) =>
            {
                BankDbContext context = new BankDbContext();
                var personalTransactionService = new PersonalTransactionServiceAPI(context);
                personalTransactionService.DeleteTransaction(id);
                return "Bireysel iþlem silindi.";
            });
            #endregion

            app.Run();
        }
    }
}