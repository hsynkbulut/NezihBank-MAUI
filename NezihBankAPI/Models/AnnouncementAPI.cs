using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NezihBankAPI.Models
{
    public class AnnouncementAPI
    {
        [Key]
        public int Id { get; set; } //Guid Id
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ImageUrl { get; set; }

        // FOREIGN KEY
        [ForeignKey("BankManagerAPI")]
        public int BankManagerId { get; set; }

        // NAVIGATION
        public BankManagerAPI BankManagerAPI { get; set; }
    }
}
