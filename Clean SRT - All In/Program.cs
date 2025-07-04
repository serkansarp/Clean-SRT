using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Clean_SRT___All_In
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("CLEAN SRT - All In - Serkan Sarp - 2025\n");

            Console.Write("- Lütfen içinde .srt dosyaları olan klasörün tam yolunu girin: ");
            string klasorYolu = Console.ReadLine();

            // Klasör kontrolü
            if (!Directory.Exists(klasorYolu))
            {
                Console.WriteLine("Klasör bulunamadı.");
                return;
            }

            // Klasördeki .srt dosyalarını alıp string dizisine ekleyeceğim ama...
            string[] srtDosyalari = Directory.GetFiles(klasorYolu, "*.srt");

            // Önce klasörde hiç .srt dosyası olmaması kontrolünü yapıyorum.
            if (srtDosyalari.Length == 0)
            {
                Console.WriteLine("Klasörde .srt dosyası bulunamadı.");
                return;
            }

            // SRT dosyalarının sayısını veriyorum.
            Console.WriteLine($"\n- {srtDosyalari.Length} adet .srt dosyası bulundu. Temizleniyor...\n");


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
