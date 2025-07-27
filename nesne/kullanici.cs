using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nesne
{
    public class Kullanici
    {
        public int Id { get; set; }
        public string Isim { get; set; }
        public string Soyisim { get; set; }
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }

        public Kullanici() { }

        public Kullanici(int id, string isim, string soyisim, string kAdi, string sifre)
        {
            Id = id;
            Isim = isim;
            Soyisim = soyisim;
            KullaniciAdi = kAdi;
            Sifre = sifre;
        }
    }
}
