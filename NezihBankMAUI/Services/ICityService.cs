using NezihBankMAUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NezihBankMAUI.Services
{
    internal interface ICityService
    {
        List<City> GetCities();
    }

    class CityService : ICityService
    {
        public List<City> GetCities()
        {
            return new List<City>()
                {
                new City() { Id = 1, Ad = "Adana" },
                new City() { Id = 2, Ad = "Adıyaman" },
                new City() { Id = 3, Ad = "Afyon" },
                new City() { Id = 4, Ad = "Ağrı" },
                new City() { Id = 5, Ad = "Amasya" },
                new City() { Id = 6, Ad = "Ankara" },
                new City() { Id = 7, Ad = "Antalya" },
                new City() { Id = 8, Ad = "Artvin" },
                new City() { Id = 9, Ad = "Aydın" },
                new City() { Id = 10, Ad = "Balıkesir" },
                new City() { Id = 11, Ad = "Bilecik" },
                new City() { Id = 12, Ad = "Bingöl" },
                new City() { Id = 13, Ad = "Bitlis" },
                new City() { Id = 14, Ad = "Bolu" },
                new City() { Id = 15, Ad = "Burdur" },
                new City() { Id = 16, Ad = "Bursa" },
                new City() { Id = 17, Ad = "Çanakkale" },
                new City() { Id = 18, Ad = "Çankırı" },
                new City() { Id = 19, Ad = "Çorum" },
                new City() { Id = 20, Ad = "Denizli" },
                new City() { Id = 21, Ad = "Diyarbakır" },
                new City() { Id = 22, Ad = "Edirne" },
                new City() { Id = 23, Ad = "Elazığ" },
                new City() { Id = 24, Ad = "Erzincan" },
                new City() { Id = 25, Ad = "Erzurum" },
                new City() { Id = 26, Ad = "Eskişehir" },
                new City() { Id = 27, Ad = "Gaziantep" },
                new City() { Id = 28, Ad = "Giresun" },
                new City() { Id = 29, Ad = "Gümüşhane" },
                new City() { Id = 30, Ad = "Hakkari" },
                new City() { Id = 31, Ad = "Hatay" },
                new City() { Id = 32, Ad = "Isparta" },
                new City() { Id = 33, Ad = "Mersin" },
                new City() { Id = 34, Ad = "İstanbul" },
                new City() { Id = 35, Ad = "İzmir" },
                new City() { Id = 36, Ad = "Kars" },
                new City() { Id = 37, Ad = "Kastamonu" },
                new City() { Id = 38, Ad = "Kayseri" },
                new City() { Id = 39, Ad = "Kırklareli" },
                new City() { Id = 40, Ad = "Kırşehir" },
                new City() { Id = 41, Ad = "Kocaeli" },
                new City() { Id = 42, Ad = "Konya" },
                new City() { Id = 43, Ad = "Kütahya" },
                new City() { Id = 44, Ad = "Malatya" },
                new City() { Id = 45, Ad = "Manisa" },
                new City() { Id = 46, Ad = "Kahramanmaraş" },
                new City() { Id = 47, Ad = "Mardin" },
                new City() { Id = 48, Ad = "Muğla" },
                new City() { Id = 49, Ad = "Muş" },
                new City() { Id = 50, Ad = "Nevşehir" },
                new City() { Id = 51, Ad = "Niğde" },
                new City() { Id = 52, Ad = "Ordu" },
                new City() { Id = 53, Ad = "Rize" },
                new City() { Id = 54, Ad = "Sakarya" },
                new City() { Id = 55, Ad = "Samsun" },
                new City() { Id = 56, Ad = "Siirt" },
                new City() { Id = 57, Ad = "Sinop" },
                new City() { Id = 58, Ad = "Sivas" },
                new City() { Id = 59, Ad = "Tekirdağ" },
                new City() { Id = 60, Ad = "Tokat" },
                new City() { Id = 61, Ad = "Trabzon" },
                new City() { Id = 62, Ad = "Tunceli" },
                new City() { Id = 63, Ad = "Şanlıurfa" },
                new City() { Id = 64, Ad = "Uşak" },
                new City() { Id = 65, Ad = "Van" },
                new City() { Id = 66, Ad = "Yozgat" },
                new City() { Id = 67, Ad = "Zonguldak" },
                new City() { Id = 68, Ad = "Aksaray" },
                new City() { Id = 69, Ad = "Bayburt" },
                new City() { Id = 70, Ad = "Karaman" },
                new City() { Id = 71, Ad = "Kırıkkale" },
                new City() { Id = 72, Ad = "Batman" },
                new City() { Id = 73, Ad = "Şırnak" },
                new City() { Id = 74, Ad = "Bartın" },
                new City() { Id = 75, Ad = "Ardahan" },
                new City() { Id = 76, Ad = "Iğdır" },
                new City() { Id = 77, Ad = "Yalova" },
                new City() { Id = 78, Ad = "Karabük" },
                new City() { Id = 79, Ad = "Kilis" },
                new City() { Id = 80, Ad = "Osmaniye" },
                new City() { Id = 81, Ad = "Düzce" },
            };
        }
    }
}
