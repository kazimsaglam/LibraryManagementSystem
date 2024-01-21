using System;

namespace Library_Management_System
{
    public class Kitap
    {
        private static int sonKitapID = 0;

        public int kitapID;
        public string kitapAdi;
        public string yazarAdi;
        public string tur;
        public int kopyaSayisi;
        public int oduncAlinanKopyaSayisi;
        public DateTime? iadeTarihi;

        public Kitap()
        {
            sonKitapID++;
            kitapID = sonKitapID;
        }

    }
}
