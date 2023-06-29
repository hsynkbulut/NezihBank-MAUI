using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NezihBankAPI.Models
{
    public class PersonalAccountsAPI
    {
        [Key]
        public int AccountId { get; set; }

        public string IbanNumber { get; set; }
        public string AccountNumber { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Balance { get; set; }

        // FOREIGN KEYS
        [ForeignKey("PersonalCustomerAPI")]
        public int PersonalCustomerId { get; set; } //Guid PersonalCustomerId

        // NAVIGATION
        public PersonalCustomersAPI PersonalCustomerAPI { get; set; }

        //[NotMapped]
        public ICollection<PersonalTransactionsAPI> TransactionsAPI { get; set; }
    }
}
