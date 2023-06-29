using System.ComponentModel.DataAnnotations;

namespace NezihBankAPI.Models
{
    public class PersonalCustomersAPI
    {
        [Key]
        public int Id { get; set; } //Guid Id

        [MaxLength(11), Required]
        public string IdentityNumber { get; set; } //Müşteri TC

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        [MaxLength(11), Required]
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string District { get; set; }

        // NAVIGATION
        public ICollection<PersonalAccountsAPI> PersonalAccountsAPI { get; set; }
    }
}
