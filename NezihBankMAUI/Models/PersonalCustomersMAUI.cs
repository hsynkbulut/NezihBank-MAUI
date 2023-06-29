using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NezihBankMAUI.Models
{
    public class PersonalCustomersMAUI
    {
        public int Id { get; set; } 
        public string IdentityNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string District { get; set; }

        // NAVIGATION
        public ICollection<PersonalAccountsMAUI> PersonalAccounts { get; set; }
    }
}
