using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library_Management_System
{
    public class Kutuphane
    {
        private List<Kitap> kitapListesi = new List<Kitap>();
        private string dosyaAdi = "KitapListesi.txt";

        public void KitapEkle(Kitap yeniKitap)
        {
            kitapListesi.Add(yeniKitap);
            KitaplariDosyayaKaydet();
            Console.WriteLine("\nKitap başarıyla eklendi.\n");
            KonsoluTemizle(1);
        }

        public void TumKitaplariGoruntule()
        {
            foreach (var kitap in kitapListesi)
            {
                Console.WriteLine($"ID: {kitap.kitapID}");
                Console.WriteLine($"Kitap Adı: {kitap.kitapAdi}");
                Console.WriteLine($"Yazar: {kitap.yazarAdi}");
                Console.WriteLine($"Tür: {kitap.tur}");
                Console.WriteLine($"Kopya Sayısı: {kitap.kopyaSayisi}");
                Console.WriteLine($"Ödünç Alınan Kopya Sayısı: {kitap.oduncAlinanKopyaSayisi}");
                Console.WriteLine("--------------------------------------------\n");
            }

            KitaplariDosyayaKaydet();
        }

        public void KitapAra(string anahtar)
        {
            var bulunanKitaplar = kitapListesi.Where(k => k.kitapAdi.IndexOf(anahtar, StringComparison.OrdinalIgnoreCase) >= 0 || k.yazarAdi.IndexOf(anahtar, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            if (bulunanKitaplar.Count == 0)
            {
                Console.WriteLine("\nAradığınız Kitap bulunamadı.\n");
            }
            else
            {
                Console.WriteLine("Arama Sonuçları:\n");
                foreach (var kitap in bulunanKitaplar)
                {
                    Console.WriteLine($"ID: {kitap.kitapID}, Kitap Adı: {kitap.kitapAdi}, Yazar: {kitap.yazarAdi}, Tür: {kitap.tur}, Kopya Sayısı: {kitap.kopyaSayisi}, Ödünç Alınan Kopya Sayısı: {kitap.oduncAlinanKopyaSayisi}\n");
                }
            }
        }

        public void KitapOduncAl(string kAdi)
        {
            var kitap = kitapListesi.FirstOrDefault(k => k.kitapAdi.Equals(kAdi, StringComparison.OrdinalIgnoreCase));

            if (kitap != null)
            {
                if (kitap.kopyaSayisi > 0)
                {
                    kitap.kopyaSayisi--;
                    kitap.oduncAlinanKopyaSayisi++;
                    kitap.iadeTarihi = DateTime.Now.AddDays(2);
                    Console.WriteLine($"\n{kitap.kitapAdi} kitabı ödünç alındı.\n");
                    KitaplariDosyayaKaydet();
                }
                else
                {
                    Console.WriteLine("\nBelirtilen kitap bulunamadı veya ödünç alınamaz.\n");
                }
            }
            else
            {
                Console.WriteLine("\nKitap bulunamadı.\n");
            }

            KitaplariDosyayaKaydet();
        }

        public void KitapIadeEt(string kAdi)
        {
            var kitap = kitapListesi.FirstOrDefault(k => k.kitapAdi.Equals(kAdi, StringComparison.OrdinalIgnoreCase));

            if (kitap != null)
            {
                if (kitap.oduncAlinanKopyaSayisi > 0)
                {
                    kitap.oduncAlinanKopyaSayisi--;
                    kitap.kopyaSayisi++;
                    kitap.iadeTarihi = null;
                    Console.WriteLine($"\n{kitap.kitapAdi} kitabı iade edildi.\n");
                    KitaplariDosyayaKaydet();
                }
                else
                {
                    Console.WriteLine("\nBu kitap zaten ödünç alınmamış.\n");
                }
            }
            else
            {
                Console.WriteLine("\nKitap bulunamadı.\n");
            }

            KitaplariDosyayaKaydet();
        }

        public void OduncAlinanKitaplariGoruntule()
        {
            var oduncKitaplar = kitapListesi.Where(k => k.oduncAlinanKopyaSayisi > 0).ToList();

            if (oduncKitaplar.Count == 0)
            {
                Console.WriteLine("Hiçbir kitap ödünç alınmamış.\n");
            }
            else
            {
                Console.WriteLine("Ödünç Alınmış Kitaplar:\n");
                foreach (var kitap in oduncKitaplar)
                {
                    Console.WriteLine($"ID: {kitap.kitapID}, Kitap Adı: {kitap.kitapAdi}, Yazar: {kitap.yazarAdi}, Tür: {kitap.tur}, Kopya Sayısı: {kitap.kopyaSayisi}, Ödünç Alınan Kopya Sayısı: {kitap.oduncAlinanKopyaSayisi}, İade Tarihi: {kitap.iadeTarihi}");
                    Console.WriteLine("---------------------------------------------------------------------------------------------------------\n");
                }
            }
        }

        public void SuresiGecmisKitaplariGoruntule()
        {
            var gecmisKitaplar = kitapListesi.Where(k => k.oduncAlinanKopyaSayisi > 0).ToList();

            if (gecmisKitaplar.Count > 0)
            {
                Console.WriteLine("Teslim Tarihi Geçmiş Kitaplar:\n");
                foreach (var kitap in gecmisKitaplar)
                {
                    if (kitap.iadeTarihi.HasValue && (kitap.iadeTarihi.Value - DateTime.Now).TotalMilliseconds < 0)
                    {
                        Console.WriteLine($"ID: {kitap.kitapID}, Kitap Adı: {kitap.kitapAdi}, Yazar: {kitap.yazarAdi}, Tür: {kitap.tur}, Kopya Sayısı: {kitap.kopyaSayisi}, Ödünç Alınan Kopya Sayısı: {kitap.oduncAlinanKopyaSayisi}, İade Tarihi: {kitap.iadeTarihi}");
                        Console.WriteLine("---------------------------------------------------------------------------------------------------------\n");
                    }
                }
            }
            else
            {
                Console.WriteLine("\nSüresi geçmiş kitap bulunamadı.\n");
            }
        }

        public void KonsoluTemizle(int saniye)
        {
            Thread.Sleep(saniye * 1000);
            Console.Clear();
        }

        private void KitaplariDosyayaKaydet()
        {
            using (StreamWriter writer = new StreamWriter(dosyaAdi))
            {
                foreach (var kitap in kitapListesi)
                {
                    string iadeTarihiString = kitap.iadeTarihi.HasValue ? kitap.iadeTarihi.Value.ToString("yyyy-MM-dd HH:mm:ss") : "null";
                    writer.WriteLine($"{kitap.kitapID},{kitap.kitapAdi},{kitap.yazarAdi},{kitap.tur},{kitap.kopyaSayisi},{kitap.oduncAlinanKopyaSayisi},{iadeTarihiString}");
                }
            }
        }

        public void KitaplariDosyadanOku()
        {
            if (File.Exists(dosyaAdi))
            {
                kitapListesi.Clear();

                using (StreamReader reader = new StreamReader(dosyaAdi))
                {
                    while (!reader.EndOfStream)
                    {
                        string[] kitapBilgileri = reader.ReadLine().Split(',');

                        Kitap kitap = new Kitap
                        {
                            kitapID = int.Parse(kitapBilgileri[0]),
                            kitapAdi = kitapBilgileri[1],
                            yazarAdi = kitapBilgileri[2],
                            tur = kitapBilgileri[3],
                            kopyaSayisi = int.Parse(kitapBilgileri[4]),
                            oduncAlinanKopyaSayisi = int.Parse(kitapBilgileri[5]),
                            iadeTarihi = ParseIadeTarihi(kitapBilgileri[6])
                        };
                        kitapListesi.Add(kitap);
                    }
                }

                KitaplariDosyayaKaydet();
            }
        }

        static DateTime? ParseIadeTarihi(string tarihStr)
        {
            if (tarihStr != "null")
            {
                try
                {
                    return DateTime.ParseExact(tarihStr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                }
                catch (FormatException)
                {
                    // Hata durumunda null döndür
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
