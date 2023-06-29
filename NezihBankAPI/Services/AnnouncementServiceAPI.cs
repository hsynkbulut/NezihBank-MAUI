using NezihBankAPI.Data;
using NezihBankAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NezihBankAPI.Services
{
    public class AnnouncementServiceAPI
    {
        private readonly BankDbContext _context;

        public AnnouncementServiceAPI(BankDbContext context)
        {
            _context = context;
        }

        //Tüm Duyuruları Getir
        public List<AnnouncementAPI> GetAllAnnouncements()
        {
            return _context.Announcements.ToList();
        }

        //Bugünün Duyurularını Getir
        public List<AnnouncementAPI> GetTodaysAnnouncements()
        {
            return _context.Announcements.Where(x => x.CreatedDate >= DateTime.Today).ToList();
        }

        //ID'sine göre belirli duyuruyu getir
        public AnnouncementAPI GetAnnouncement(int id) 
        {
            //return _context.Announcements.Where(x => x.Id == id).FirstOrDefault();
            //return _context.Announcements.FirstOrDefault(x => x.Id == id);
            return _context.Announcements.Find(id);
        }

        //Yeni duyuru ekle
        public void AddAnnouncement(AnnouncementAPI duyuru)
        {
            _context.Announcements.Add(duyuru);
            _context.SaveChanges();
        }

        //Duyuru sil
        public void DeleteAnnouncement(int id) //DuyuruSil(Guid id)
        {
            var announcementDelete = _context.Announcements.Find(id);
            if (announcementDelete != null)
            {
                _context.Announcements.Remove(announcementDelete);
                _context.SaveChanges();
            }
        }

        //Kampanya güncelle
        public void UpdateAnnouncement(AnnouncementAPI updatedAnnouncement)
        {
            var updateAnnouncement = _context.Announcements.Find(updatedAnnouncement.Id);
            if (updateAnnouncement != null)
            {
                // Tüm bilgilerin girildiğini kontrol etmek için doğrulama işlemleri
                if (!string.IsNullOrEmpty(updatedAnnouncement.Title) &&
                    !string.IsNullOrEmpty(updatedAnnouncement.Content) &&
                    !string.IsNullOrEmpty(updatedAnnouncement.CreatedDate.ToString()) &&
                    !string.IsNullOrEmpty(updatedAnnouncement.ImageUrl))
                {
                    updateAnnouncement.Title = updatedAnnouncement.Title;
                    updateAnnouncement.Content = updatedAnnouncement.Content;
                    updateAnnouncement.ImageUrl = updatedAnnouncement.ImageUrl;
                    updateAnnouncement.CreatedDate = updatedAnnouncement.CreatedDate;
                    updateAnnouncement.BankManagerId = updatedAnnouncement.BankManagerId;

                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("Eksik veya yanlış bilgi girdiniz. Lütfen tüm bilgileri doğru bir şekilde giriniz.");
                }
            }
            else
            {
                throw new Exception("Belirtilen kampanya bulunamadı.");
            }
        }

    }
}
