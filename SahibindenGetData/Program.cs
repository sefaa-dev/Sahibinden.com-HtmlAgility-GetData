using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SahibindenGetData
{
    internal class Program
    {
        static void Main(string[] args)
        { 
            List<IlanDetay> ilanlar = new List<IlanDetay>();  //İlanlarımı bir listede tutuyorum.
        
            HtmlWeb web = new HtmlWeb();                      
            string urlAnaSayfa = "https://www.sahibinden.com";   //Veri çekeceğim url urlAnaSayfa ya atıyorum
            HtmlDocument doc = web.Load(urlAnaSayfa);          
            HtmlDocument ilanDocument;  //İlan dokuman sayfasını içinde tutmak için
            // Ürün sayısını bulmak için urunNodes
            //var urunNodes = doc.DocumentNode.SelectNodes("//*[@id='container']/div[3]/div/div[3]/div[3]/ul/li/a");
            HtmlNode urunNode;

            string urunUrl;
            string hrefValue;
            string ilanFiyatText;
            int i = 0;

            while (i < 30) // urunNodes.Count -> Çok fazla istek atınca sahibinden atıyor
            {
                IlanDetay ilanDetay = new IlanDetay();
                i++;
                urunNode = doc.DocumentNode.SelectSingleNode("//*[@id='container']/div[3]/div/div[3]/div[3]/ul/li[" + i + "]/a/span");
                if (urunNode != null) //reklamları almamak için , çünkü reklamlarda span yok.
                {
                    
                    var hrefNode = doc.DocumentNode.SelectSingleNode("//*[@id='container']/div[3]/div/div[3]/div[3]/ul/li[" + i + "]/a[@href]");
                    hrefValue = hrefNode.GetAttributeValue("href", string.Empty);
                    urunUrl = urlAnaSayfa + hrefValue;

                    Thread.Sleep(500);
                    ilanDocument = web.Load(urunUrl);
                    var ilanNode = ilanDocument.DocumentNode.SelectSingleNode("//div[@class='classifiedInfo ']/h3"); //İlan fiyatının bulunduğu node

                    if (ilanNode != null)  
                    {
                        ilanFiyatText = ilanNode.InnerText.Substring(1).Trim();   //Baştaki /n silmek için ve sonraki boşlukları trimle budamak için
                        if (ilanFiyatText.Substring(0, 1) == "€")
                        {
                           
                            ilanDetay.FiyatBirimi = fiyatBirimi.Euro;
                            ilanFiyatText = ilanFiyatText.Substring(2);
                        }
                        else if (ilanFiyatText.Substring(0, 1) == "$")
                        {
                            ilanDetay.FiyatBirimi = fiyatBirimi.Dolar;
                            ilanFiyatText = ilanFiyatText.Substring(2);
                        }
                        else 
                        {
                            ilanDetay.FiyatBirimi = fiyatBirimi.TL;

                        }
                        int index = ilanFiyatText.IndexOf(' ');  
                        ilanFiyatText = ilanFiyatText.Substring(0, index);
                    }
                    else      //ücretsiz sahiplendirme vb. yazı olduğu durumlarda 
                    {
                        ilanFiyatText = "0";
                    }
                    ilanDetay.Isim = urunNode.InnerText;
                    ilanDetay.IlanLinki = urunUrl;
                    ilanDetay.Fiyat = Decimal.Parse(ilanFiyatText);
                    ilanlar.Add(ilanDetay);
                }

            }
            i = 0;
            foreach (var item in ilanlar)
            {
                i++;
                Console.WriteLine(i + " ) " + item.Isim + "\nİlan Fiyat: " + item.Fiyat + item.FiyatBirimi + "\n");
            }
        }
    }
}