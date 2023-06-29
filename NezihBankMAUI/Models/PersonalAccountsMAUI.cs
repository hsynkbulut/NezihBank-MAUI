using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NezihBankMAUI.Models
{
    public class PersonalAccountsMAUI
    {
        public int AccountId { get; set; }

        public string IbanNumber { get; set; }
        public string AccountNumber { get; set; }

        public decimal Balance { get; set; }

        public int PersonalCustomerId { get; set; } 

        // NAVIGATION
        public PersonalCustomersMAUI PersonalCustomers { get; set; }

        public ICollection<PersonalTransactionsMAUI> PersonalTransactions { get; set; }
    }
}
