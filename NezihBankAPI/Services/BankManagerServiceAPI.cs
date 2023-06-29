using NezihBankAPI.Data;
using NezihBankAPI.Models;

namespace NezihBankAPI.Services
{
    public class BankManagerServiceAPI
    {
        private readonly BankDbContext _context;

        public BankManagerServiceAPI(BankDbContext context)
        {
            _context = context;
        }

        //Tüm Banka Yetkililerini Listele
        public List<BankManagerAPI> GetAllBankManagers()
        {
            return _context.BankManager.ToList();
        }

        //ID'sine göre belirli yetkiliyi getir
        public BankManagerAPI GetBankManager(int id)
        {
            //return _context.BankManager.Where(x => x.Id == id).FirstOrDefault();
            //return _context.BankManager.FirstOrDefault(x => x.Id == id);
            return _context.BankManager.Find(id);
        }

        public void AddBankManager(BankManagerAPI bankManager)
        {
            if (bankManager != null)
            {
                // Tüm bilgilerin girildiğini kontrol etmek için doğrulama işlemleri
                if (!string.IsNullOrEmpty(bankManager.IdentityNumber) &&
                    !string.IsNullOrEmpty(bankManager.Name) &&
                    !string.IsNullOrEmpty(bankManager.Surname) &&
                    !string.IsNullOrEmpty(bankManager.Password))
                {
                    // Ekleme işlemleri burada gerçekleştirilir
                    _context.BankManager.Add(bankManager);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("Eksik veya yanlış bilgi girdiniz. Lütfen tüm bilgileri doğru bir şekilde giriniz.");
                }
            }
            else
            {
                throw new Exception("Banka müdürü boş olamaz.");
            }
        }

        public void DeleteBankManager(int bankManagerId)
        {
            var bankManager = _context.BankManager.Find(bankManagerId);
            if (bankManager != null)
            {
                _context.BankManager.Remove(bankManager);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Belirtilen banka müdürü bulunamadı.");
            }
        }

        public void UpdateBankManager(BankManagerAPI updatedBankManager)
        {
            var bankManager = _context.BankManager.Find(updatedBankManager.BankManagerId);
            if (bankManager != null)
            {
                // Tüm bilgilerin girildiğini kontrol etmek için doğrulama işlemleri
                if (!string.IsNullOrEmpty(updatedBankManager.IdentityNumber) &&
                    !string.IsNullOrEmpty(updatedBankManager.Name) &&
                    !string.IsNullOrEmpty(updatedBankManager.Surname) &&
                    !string.IsNullOrEmpty(updatedBankManager.Password))
                {
                    bankManager.IdentityNumber = updatedBankManager.IdentityNumber;
                    bankManager.Name = updatedBankManager.Name;
                    bankManager.Surname = updatedBankManager.Surname;
                    bankManager.Password = updatedBankManager.Password;

                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("Eksik veya yanlış bilgi girdiniz. Lütfen tüm bilgileri doğru bir şekilde giriniz.");
                }
            }
            else
            {
                throw new Exception("Belirtilen banka müdürü bulunamadı.");
            }
        }      

    }
}
