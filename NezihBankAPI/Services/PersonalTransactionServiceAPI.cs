using NezihBankAPI.Data;
using NezihBankAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NezihBankAPI.Services
{
    public class PersonalTransactionServiceAPI
    {
        private readonly BankDbContext _context;

        public PersonalTransactionServiceAPI(BankDbContext dbContext)
        {
            _context = dbContext;
        }

        // Tüm işlemleri getir
        public IEnumerable<PersonalTransactionsAPI> GetAllTransactions()
        {
            return _context.Transactions.ToList();
        }

        // Belirli bir işlemi getir
        public PersonalTransactionsAPI GetTransactionById(int transactionId)
        {
            //return _context.Transactions.FirstOrDefault(t => t.TransactionId == transactionId);
            //return _context.Transactions.Where(x => x.TransactionId == transactionId).FirstOrDefault();
            return _context.Transactions.Find(transactionId);
        }

        // Belirli bir hesaba ait olan işlemleri getir
        public IEnumerable<PersonalTransactionsAPI> GetTransactionsByAccountId(int accountId)
        {
            return _context.Transactions.Where(t => t.AccountId == accountId).ToList();
        }

        // İşlem ekle
        public void AddTransaction(PersonalTransactionsAPI transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }

        // İşlem güncelle
        public void UpdateTransaction(PersonalTransactionsAPI updatedTransaction)
        {
            var transactionUpdate = _context.Transactions.Find(updatedTransaction.TransactionId);
            if (transactionUpdate != null)
            {
                if (!string.IsNullOrEmpty(updatedTransaction.TransactionDate.ToString()) &&
                    !string.IsNullOrEmpty(updatedTransaction.Amount.ToString()) &&
                    !string.IsNullOrEmpty(updatedTransaction.TransactionCategory))
                {
                    // Doğrulama işleminden sonra işlem bilgilerini güncelliyoruz
                    transactionUpdate.TransactionDate = updatedTransaction.TransactionDate;
                    transactionUpdate.Amount = updatedTransaction.Amount;
                    transactionUpdate.TransactionCategory = updatedTransaction.TransactionCategory;

                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("Eksik veya yanlış bilgi girdiniz. Lütfen tüm bilgileri doğru bir şekilde giriniz.");
                }
            }
            else
            {
                throw new Exception("Herhangi bir işlem bulunamadı.");
            }
        }

        // İşlem sil
        public void DeleteTransaction(int transactionId)
        {
            var transactionDelete = _context.Transactions.Find(transactionId);
            if (transactionDelete != null)
            {
                _context.Transactions.Remove(transactionDelete);
                _context.SaveChanges();
            }
        }
    }
}
