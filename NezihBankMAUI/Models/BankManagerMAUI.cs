using System.ComponentModel.DataAnnotations;

namespace NezihBankMAUI.Models
{
    public class BankManagerMAUI
    {
        public int BankManagerId { get; set; }

        public string IdentityNumber { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Password { get; set; }

        // NAVIGATION
        public ICollection<AnnouncementMAUI> Announcements { get; set; }
    }
}
