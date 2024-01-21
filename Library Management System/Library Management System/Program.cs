using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace Library_Management_System
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Kutuphane kutuphane = new Kutuphane();
            kutuphane.KitaplariDosyadanOku();

            while (true)
            {
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("| Kütüphane Yönetim Sistemine Hoşgeldiniz |");
                Console.WriteLine("-------------------------------------------");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("1. Yeni Kitap Ekle");
                Console.WriteLine("2. Tüm Kitapları Görüntüle");
                Console.WriteLine("3. Süresi Geçmiş Kitapları Görüntüle");
                Console.WriteLine("4. Kitap Ara");
                Console.WriteLine("5. Kitap Ödünç Al");
                Console.WriteLine("6. Kitap İade Et");
                Console.WriteLine("9. Çıkış");
                Console.ResetColor();

                Console.Write("\nYapmak istediğiniz işlemi seçin: ");
                string secim = Console.ReadLine();
                
                switch(secim)
                {
                    case "1":
                        kutuphane.KonsoluTemizle(0);
                        Kitap yeniKitap = new Kitap();
                        Console.Write("Kitap Adı: ");
                        yeniKitap.kitapAdi = Console.ReadLine();
                        Console.Write("Yazar Adı : ");
                        yeniKitap.yazarAdi = Console.ReadLine();
                        Console.Write("Kitabın Türü : ");
                        yeniKitap.tur = Console.ReadLine();
                        Console.Write("Kopya Sayısı : ");
                        while (!int.TryParse(Console.ReadLine(), out yeniKitap.kopyaSayisi) || yeniKitap.kopyaSayisi < 1)
                        {
                            Console.WriteLine("Geçersiz giriş. Kopya sayısı 1'den küçük veya harf olamaz.");
                            Console.Write("Kopya Sayısı: ");
                        }
                        kutuphane.KitapEkle(yeniKitap);
                        break;

                    case "2":
                        kutuphane.KonsoluTemizle(0);
                        kutuphane.TumKitaplariGoruntule();
                        break;

                    case "3":
                        kutuphane.KonsoluTemizle(0);
                        kutuphane.SuresiGecmisKitaplariGoruntule();
                        break;

                    case "4":
                        kutuphane.KonsoluTemizle(0);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("Aranacak Kelime (Başlık veya Yazar): ");
                        Console.ResetColor();
                        string anahtar = Console.ReadLine();
                        kutuphane.KitapAra(anahtar);
                        break;

                    case "5":
                        kutuphane.KonsoluTemizle(0);
                        kutuphane.TumKitaplariGoruntule();
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("Ödünç Alınacak Kitabın Adı: ");
                        Console.ResetColor();
                        string oduncKitap = Console.ReadLine();
                        kutuphane.KitapOduncAl(oduncKitap);
                        break;

                    case "6":
                        kutuphane.KonsoluTemizle(0);
                        kutuphane.OduncAlinanKitaplariGoruntule();
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("İade Edilecek Kitabın Adı: ");
                        Console.ResetColor();
                        string iadeKitap = Console.ReadLine();
                        kutuphane.KitapIadeEt(iadeKitap);
                        break;

                    case "9":
                        Environment.Exit(0);
                        break;

                    default:
                        kutuphane.KonsoluTemizle(0);
                        Console.WriteLine("Geçersiz seçenek. Lütfen tekrar deneyiniz!");
                        break;
                }

            }
        }
    }
}
