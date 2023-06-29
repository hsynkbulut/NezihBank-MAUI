using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NezihBankMAUI.Models
{
    public class PersonalTransactionsMAUI
    {
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string TransactionCategory { get; set; } 

        // NAVIGATION
        public PersonalAccountsMAUI AccountMAUI { get; set; } 
    }
}
