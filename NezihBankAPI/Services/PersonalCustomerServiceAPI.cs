using NezihBankAPI.Data;
using NezihBankAPI.Models;

namespace NezihBankAPI.Services
{
    public class PersonalCustomerServiceAPI
    {
        private readonly BankDbContext _context;

        public PersonalCustomerServiceAPI(BankDbContext context)
        {
            _context = context;
        }

        //Tüm Banka Müşterilerini Listele
        public List<PersonalCustomersAPI> GetAllCustomers()
        {
            return _context.PersonalCustomers.ToList();
        }

        //ID'sine göre belirli müşteriyi getir
        public PersonalCustomersAPI GetCustomer(int id) 
        {
            //return _context.PersonalCustomers.Where(x => x.Id == id).FirstOrDefault();
            //return _context.PersonalCustomers.FirstOrDefault(x => x.Id == id);
            return _context.PersonalCustomers.Find(id);
        }

        public void AddPersonalCustomer(PersonalCustomersAPI personalCustomers)
        {
            if (personalCustomers != null)
            {
                // Tüm bilgilerin girildiğini kontrol etmek için doğrulama işlemleri 
                if (!string.IsNullOrEmpty(personalCustomers.IdentityNumber) &&
                    !string.IsNullOrEmpty(personalCustomers.Name) &&
                    !string.IsNullOrEmpty(personalCustomers.Surname) &&
                    !string.IsNullOrEmpty(personalCustomers.Password) &&
                    !string.IsNullOrEmpty(personalCustomers.Email) &&
                    !string.IsNullOrEmpty(personalCustomers.PhoneNumber) &&
                    !string.IsNullOrEmpty(personalCustomers.BirthDate.ToString()) &&
                    !string.IsNullOrEmpty(personalCustomers.Address) &&
                    !string.IsNullOrEmpty(personalCustomers.City) &&
                    !string.IsNullOrEmpty(personalCustomers.District))
                {
                    // Ekleme işlemleri burada gerçekleştirilir
                    _context.PersonalCustomers.Add(personalCustomers);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("Eksik veya yanlış bilgi girdiniz. Lütfen tüm bilgileri doğru bir şekilde giriniz.");
                }
            }
            else
            {
                throw new Exception("Banka müdürü boş olamaz.");
            }
        }

        //Müşteri sil
        public void DeletePersonalCustomer(int personalCustomersId)
        {
            var customersDelete = _context.PersonalCustomers.Find(personalCustomersId);
            if (customersDelete != null)
            {
                _context.PersonalCustomers.Remove(customersDelete);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Belirtilen banka müdürü bulunamadı.");
            }
        }

        //Müşteri güncelle
        public void UpdatePersonalCustomer(PersonalCustomersAPI updatedPersonalCustomer)
        {
            var updateCustomer = _context.PersonalCustomers.Find(updatedPersonalCustomer.Id);
            if (updateCustomer != null)
            {
                // Tüm bilgilerin girildiğini kontrol etmek için doğrulama işlemleri
                if (!string.IsNullOrEmpty(updatedPersonalCustomer.IdentityNumber) &&
                    !string.IsNullOrEmpty(updatedPersonalCustomer.Name) &&
                    !string.IsNullOrEmpty(updatedPersonalCustomer.Surname) &&
                    !string.IsNullOrEmpty(updatedPersonalCustomer.Password) &&
                    !string.IsNullOrEmpty(updateCustomer.Email) &&
                    !string.IsNullOrEmpty(updateCustomer.PhoneNumber) &&
                    !string.IsNullOrEmpty(updateCustomer.BirthDate.ToString()) &&
                    !string.IsNullOrEmpty(updateCustomer.Address) &&
                    !string.IsNullOrEmpty(updateCustomer.City) &&
                    !string.IsNullOrEmpty(updateCustomer.District))
                {
                    updateCustomer.IdentityNumber = updatedPersonalCustomer.IdentityNumber;
                    updateCustomer.Name = updatedPersonalCustomer.Name;
                    updateCustomer.Surname = updatedPersonalCustomer.Surname;
                    updateCustomer.Password = updatedPersonalCustomer.Password;
                    updateCustomer.Email = updatedPersonalCustomer.Email;
                    updateCustomer.PhoneNumber = updatedPersonalCustomer.PhoneNumber;
                    updateCustomer.BirthDate = updatedPersonalCustomer.BirthDate;
                    updateCustomer.Address = updatedPersonalCustomer.Address;
                    updateCustomer.City = updatedPersonalCustomer.City;
                    updateCustomer.District = updatedPersonalCustomer.District;

                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("Eksik veya yanlış bilgi girdiniz. Lütfen tüm bilgileri doğru bir şekilde giriniz.");
                }
            }
            else
            {
                throw new Exception("Belirtilen müşteri bulunamadı.");
            }
        }

    }
}
