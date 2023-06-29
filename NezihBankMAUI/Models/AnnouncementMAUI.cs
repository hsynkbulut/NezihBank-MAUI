using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NezihBankMAUI.Models
{
    public class AnnouncementMAUI
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ImageUrl { get; set; }

        public int BankManagerId { get; set; }

        // NAVIGATION
        public BankManagerMAUI BankManager { get; set; }
    }
}
