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

            //ID'sine g�re belirli yetkiliyi getir
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
                return "Banka m�d�r� eklendi.";
            });

            //Banka yetkilisi Silmek: MapDelete
            app.MapDelete("/bankamuduru/{id}", (int id) =>
            {
                BankDbContext context = new BankDbContext();
                var bankManagerService = new BankManagerServiceAPI(context);
                bankManagerService.DeleteBankManager(id);
                return "Banka m�d�r� silindi.";
            });

            //Banka yetkili bilgisini G�ncellemek: MapPut
            app.MapPut("/bankamuduru/{id}", (BankManagerAPI updatedBankManager) =>
            {
                BankDbContext context = new BankDbContext();
                var bankManagerService = new BankManagerServiceAPI(context);
                bankManagerService.UpdateBankManager(updatedBankManager);
                return "Banka m�d�r� bilgileri g�ncellendi.";
            });
            #endregion

            #region Duyurular / Kampanya
            //Duyurular� Listelemek: MapGet
            app.MapGet("/duyurular", () =>
            {
                BankDbContext context = new BankDbContext();
                var announcementService = new AnnouncementServiceAPI(context);
                return announcementService.GetAllAnnouncements();
            });

            // Bug�n�n Duyurular�n� Listelemek: MapGet
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

            //Duyuru g�ncellemek: MapPut
            app.MapPut("/duyurular/{id}", (AnnouncementAPI updatedAnnouncement) =>
            {
                BankDbContext context = new BankDbContext();
                var announcementService = new AnnouncementServiceAPI(context);
                announcementService.UpdateAnnouncement(updatedAnnouncement);
                return "Kampanya g�ncellendi.";
            });

            //Duyuru Silmek: MapDelete
            app.MapDelete("/duyurular/{id}", (int id) =>
            {
                BankDbContext context = new BankDbContext();
                var announcementService = new AnnouncementServiceAPI(context);
                announcementService.DeleteAnnouncement(id);
            });
            #endregion

            #region Bireysel M��teri
            //Bireysel M��terileri Listelemek: MapGet
            app.MapGet("/bireyselmusteri", () =>
            {
                BankDbContext context = new BankDbContext();
                var personalCustomerService = new PersonalCustomerServiceAPI(context);
                return personalCustomerService.GetAllCustomers();
            });

            //ID'sine g�re belirli m��teriyi getir
            app.MapGet("/bireyselmusteri/{id}", (int id) =>
            {
                BankDbContext context = new BankDbContext();
                var personalCustomerService = new PersonalCustomerServiceAPI(context);
                return personalCustomerService.GetCustomer(id);
            });

            //M��teri Eklemek: MapPost
            app.MapPost("/bireyselmusteri", (PersonalCustomersAPI personalCustomers) =>
            {
                BankDbContext context = new BankDbContext();
                var personalCustomerService = new PersonalCustomerServiceAPI(context);
                personalCustomerService.AddPersonalCustomer(personalCustomers);
                return "Banka m�d�r� eklendi.";
            });

            //M��teri Silmek: MapDelete
            app.MapDelete("/bireyselmusteri/{id}", (int id) =>
            {
                BankDbContext context = new BankDbContext();
                var personalCustomerService = new PersonalCustomerServiceAPI(context);
                personalCustomerService.DeletePersonalCustomer(id);
                return "Banka m�d�r� silindi.";
            });

            //M��teri bilgisi g�ncellemek: MapPut
            app.MapPut("/bireyselmusteri/{id}", (PersonalCustomersAPI updatedCustomers) =>
            {
                BankDbContext context = new BankDbContext();
                var personalCustomerService = new PersonalCustomerServiceAPI(context);
                personalCustomerService.UpdatePersonalCustomer(updatedCustomers);
                return "M��teri bilgileri g�ncellendi.";
            });
            #endregion

            #region Hesaplar
            //Bireysel M��teri hesaplar�n� Listelemek: MapGet
            app.MapGet("/musteribankahesaplari", () =>
            {
                BankDbContext context = new BankDbContext();
                var personalAccountService = new PersonalAccountServiceAPI(context);
                return personalAccountService.GetAllAccounts();
            });

            //ID'sine g�re belirli m��teri hesab�n� getir
            app.MapGet("/musteribankahesaplari/{id}", (int id) =>
            {
                BankDbContext context = new BankDbContext();
                var personalAccountService = new PersonalAccountServiceAPI(context);
                return personalAccountService.GetAccount(id);
            });

            //M��teri hesab� Eklemek: MapPost
            app.MapPost("/musteribankahesaplari", (PersonalAccountsAPI personalAccounts) =>
            {
                BankDbContext context = new BankDbContext();
                var personalAccountService = new PersonalAccountServiceAPI(context);
                personalAccountService.AddAccount(personalAccounts);
                return "M��teri banka hesab� eklendi.";
            });

            //M��teri hesab� G�ncellemek: MapPut
            app.MapPut("/musteribankahesaplari/{id}", (PersonalAccountsAPI personalAccounts) =>
            {
                BankDbContext context = new BankDbContext();
                var personalAccountService = new PersonalAccountServiceAPI(context);
                personalAccountService.UpdateAccount(personalAccounts);
                return "M��teri banka hesab� g�ncellendi.";
            });

            //M��teri hesab� Silmek: MapDelete
            app.MapDelete("/musteribankahesaplari/{id}", (int id) =>
            {
                BankDbContext context = new BankDbContext();
                var personalAccountService = new PersonalAccountServiceAPI(context);
                personalAccountService.DeleteAccount(id);
                return "M��teri banka hesab� silindi.";
            });
            #endregion

            #region Bireysel ��lemler
            // T�m bireysel i�lemleri getir
            app.MapGet("/bireyselislemler", () =>
            {
                BankDbContext context = new BankDbContext();
                var personalTransactionService = new PersonalTransactionServiceAPI(context);
                return personalTransactionService.GetAllTransactions();
            });
            
            // ID'sine g�re belirli i�lemi getir
            app.MapGet("/bireyselislemler/{id}", (int id) =>
            {
                BankDbContext context = new BankDbContext();
                var personalTransactionService = new PersonalTransactionServiceAPI(context);
                return personalTransactionService.GetTransactionById(id);
            });

            // Hesap ID'sine G�re Bireysel ��lemleri Getir
            app.MapGet("/bireyselislemler/hesap/{accountId}", (int accountId) =>
            {
                BankDbContext context = new BankDbContext();
                var personalTransactionService = new PersonalTransactionServiceAPI(context);
                return personalTransactionService.GetTransactionsByAccountId(accountId);
            });

            // Bireysel ��lem Ekle: MapPost
            app.MapPost("/bireyselislemler", (PersonalTransactionsAPI transaction) =>
            {
                BankDbContext context = new BankDbContext();
                var personalTransactionService = new PersonalTransactionServiceAPI(context);
                personalTransactionService.AddTransaction(transaction);
                return "Bireysel i�lem eklendi.";
            });

            // Bireysel ��lem G�ncelle: MapPut
            app.MapPut("/bireyselislemler/{id}", (PersonalTransactionsAPI updatedTransaction) =>
            {
                BankDbContext context = new BankDbContext();
                var personalTransactionService = new PersonalTransactionServiceAPI(context);
                personalTransactionService.UpdateTransaction(updatedTransaction);
                return "Bireysel i�lem g�ncellendi.";
            });

            // Bireysel ��lem Sil: MapDelete
            app.MapDelete("/bireyselislemler/{id}", (int id) =>
            {
                BankDbContext context = new BankDbContext();
                var personalTransactionService = new PersonalTransactionServiceAPI(context);
                personalTransactionService.DeleteTransaction(id);
                return "Bireysel i�lem silindi.";
            });
            #endregion

            app.Run();
        }
    }
}