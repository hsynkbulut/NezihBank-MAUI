using NezihBankAPI.Data;
using NezihBankAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NezihBankAPI.Services
{
    public class PersonalAccountServiceAPI
    {
        private readonly BankDbContext _context;

        public PersonalAccountServiceAPI(BankDbContext context)
        {
            _context = context;
        }

        //Tüm Hesapları Listele
        public List<PersonalAccountsAPI> GetAllAccounts()
        {
            return _context.Accounts.ToList();
        }

        //ID'sine göre belirli hesabı getir
        public PersonalAccountsAPI GetAccount(int id)
        {
            //return _context.Accounts.Where(x => x.Id == id).FirstOrDefault();
            //return _context.Accounts.FirstOrDefault(x => x.Id == id);
            return _context.Accounts.Find(id);
        }

        //Yeni hesap ekle
        public void AddAccount(PersonalAccountsAPI duyuru)
        {
            _context.Accounts.Add(duyuru);
            _context.SaveChanges();
        }

        //Hesap sil
        public void DeleteAccount(int id)
        {
            var accountDelete = _context.Accounts.Find(id);
            if (accountDelete != null)
            {
                _context.Accounts.Remove(accountDelete);
                _context.SaveChanges();
            }
        }

        //Hesap güncelle
        public void UpdateAccount(PersonalAccountsAPI updatedPersonalAccount)
        {
            var accountUpdate = _context.Accounts.Find(updatedPersonalAccount.AccountId);
            if (accountUpdate != null)
            {
                // Tüm bilgilerin girildiğini kontrol etmek için doğrulama işlemleri
                if (!string.IsNullOrEmpty(updatedPersonalAccount.IbanNumber) &&
                    !string.IsNullOrEmpty(updatedPersonalAccount.AccountNumber) &&
                    !string.IsNullOrEmpty(updatedPersonalAccount.PersonalCustomerId.ToString()) &&
                    !string.IsNullOrEmpty(updatedPersonalAccount.Balance.ToString()))
                {
                    accountUpdate.IbanNumber = updatedPersonalAccount.IbanNumber;
                    accountUpdate.AccountNumber = updatedPersonalAccount.AccountNumber;
                    accountUpdate.Balance = updatedPersonalAccount.Balance;
                    accountUpdate.PersonalCustomerId = updatedPersonalAccount.PersonalCustomerId;

                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("Eksik veya yanlış bilgi girdiniz. Lütfen tüm bilgileri doğru bir şekilde giriniz.");
                }
            }
            else
            {
                throw new Exception("Belirtilen müşteri hesabı bulunamadı.");
            }
        }

    }
}
