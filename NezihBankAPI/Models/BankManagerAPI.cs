using System.ComponentModel.DataAnnotations;

namespace NezihBankAPI.Models
{
    public class BankManagerAPI
    {
        [Key]
        public int BankManagerId { get; set; }

        [Required]
        public string IdentityNumber { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Password { get; set; }

        // NAVIGATION
        public ICollection<AnnouncementAPI> Announcements { get; set; }
    }
}
