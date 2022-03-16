using System.ComponentModel;

namespace SahibindenGetData
{
    public class IlanDetay
    {
        public string Isim { get; set; }
        public string IlanLinki { get; set; }
        public decimal Fiyat { get; set; }
        public fiyatBirimi FiyatBirimi { get; set; }



    }
    public enum fiyatBirimi
    {
        Dolar,
        Euro,
        TL
        

    }
}
