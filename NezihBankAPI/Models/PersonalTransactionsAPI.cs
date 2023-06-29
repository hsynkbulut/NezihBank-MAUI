using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NezihBankAPI.Models
{
    public class PersonalTransactionsAPI
    {
        [Key]
        public int TransactionId { get; set; }

        // FOREIGN KEYS
        [ForeignKey("AccountAPI")]
        public int AccountId { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        public string TransactionCategory { get; set; }

        // NAVIGATION
        public PersonalAccountsAPI AccountAPI { get; set; }
    }
}
