using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

/*
CLEAN SRT - All In
Ana proje olan Clean SRT'nin klasör bazında çalışan versiyonudur. Yolu girilen klasördeki bütün SRT dosyalarını sadeleştirir.

Bazı cihazlar tarafından işlenmeden direkt ekrana yazdırılan <i>, <b>, {\an8}, <font color="red"> ve benzeri
etiketleri SRT dosyasından temizleyip sade bir dosyaya dönüştürür. Amaç, altyazıyla izlenen videolarda çıkan
ve can sıkan etiketlerden kurtulmak, daha rahat bir izleme deneyimi sunmaktır. Olabildiğince sade kodlanmıştır.
Windows-1254 tipi dosyaları okur ve yazar. Etiketleri temizleme dışında dosyada herhangi bir değişiklik yapmaz.
Zaman damgalarını, satır numaralarını ve altyazı metinlerini olduğu gibi bırakır.

Başlangıçta el ile klasör yolunu girmeniz gerekir. Dosyalarınızın yedeğini almayı unutmayın.

Farklı klasördeki dosyalar için Clean SRT ile tek tek yol girerek temizleme işlemi yapabilirsiniz.

Serkan SARP, 2025
*/

namespace Clean_SRT___All_In
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("CLEAN SRT - All In - Serkan Sarp - 2025\n");

            Console.Write("- Lütfen içinde .SRT dosyaları olan klasörün tam yolunu girin: ");
            string klasorYolu = Console.ReadLine();

            // Klasör kontrolü
            if (!Directory.Exists(klasorYolu))
            {
                Console.WriteLine("Klasör bulunamadı.");
                return;
            }

            // Klasördeki .SRT dosyalarını alıp string dizisine ekleyeceğim ama...
            string[] srtDosyalari = Directory.GetFiles(klasorYolu, "*.srt");

            // Önce klasörde hiç .SRT dosyası olmaması kontrolünü yapıyorum.
            if (srtDosyalari.Length == 0)
            {
                Console.WriteLine("Klasörde .SRT dosyası bulunamadı.");
                return;
            }

            // SRT dosyalarının sayısını veriyorum.
            Console.WriteLine($"\n- {srtDosyalari.Length} adet .SRT dosyası bulundu. Temizleniyor...\n");


            int toplamEtiket = 0;

            foreach (string dosyaYolu in srtDosyalari)
            {
                string srt = File.ReadAllText(dosyaYolu, Encoding.GetEncoding("windows-1254"));

                MatchCollection etiketler = Regex.Matches(srt, @"(<[^>]+>|\{[^}]+\})");
                int etiketSayisi = etiketler.Count;
                toplamEtiket += etiketSayisi;

                string temizlenmis = Regex.Replace(srt, @"(<[^>]+>|\{[^}]+\})", "");

                File.WriteAllText(dosyaYolu, temizlenmis, Encoding.GetEncoding("windows-1254"));

                Console.WriteLine($"- {Path.GetFileName(dosyaYolu)} dosyasından {etiketSayisi} etiket temizlendi.");
            }

            Console.WriteLine("\n- Toplam "+toplamEtiket+" etiket temizlenmiştir.");
            Console.WriteLine("- Lütfen programı sonlandırmak için bir tuşa basın.");

            Console.ReadKey();
        }
    }
}
