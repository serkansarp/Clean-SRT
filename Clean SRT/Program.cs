using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/*
CLEAN SRT
Bazı cihazlar tarafından işlenmeden direkt ekrana yazdırılan <i>, <b>, {\an8}, <font color="red"> ve benzeri
etiketleri SRT dosyasından temizleyip sade bir dosyaya dönüştürür. Amaç, altyazıyla izlenen videolarda çıkan
ve can sıkan etiketlerden kurtulmak, daha rahat bir izleme deneyimi sunmaktır. Olabildiğince sade kodlanmıştır.
Windows-1254 tipi dosyaları okur ve yazar. Etiketleri temizleme dışında dosyada herhangi bir değişiklik yapmaz.
Zaman damgalarını, satır numaralarını ve altyazı metinlerini olduğu gibi bırakır.

Başlangıçta el ile dosya yolunu girmeniz gerekir.

Dosyalarınızın yedeğini almayı unutmayın. Bu program, dosyalarınızı geri dönüşü olmayan şekilde değiştirebilir.

Serkan SARP, 2025
*/

namespace Clean_SRT
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("CLEAN SRT - Serkan Sarp - 2025\n");

            // Dosya yolu girişi
            Console.WriteLine("- Lütfen temizlemek istediğiniz SRT dosyasının tam yolunu girin:");
            
            string dosyaYolu = Console.ReadLine();
                                              
            // Dosyanın varlığının kontrolü
            if (!System.IO.File.Exists(dosyaYolu))
            {
                Console.WriteLine("Dosya bulunamadı.");
                return;
            }

            // Dosya uzantısı kontrolü
            if (Path.GetExtension(dosyaYolu).ToLower() != ".srt")
            {
                Console.WriteLine("Dosya uzantısı .srt değil.");
                return;
            }

            // Dosya büyüklüğü kontrolü (512 kb'tan büyük dosyaları kabul etmiyorum.)
            FileInfo fi = new FileInfo(dosyaYolu);
            if (fi.Length / 8 > 512)    // Byte olarak alıp kb'a çevirip kontrol ediyorum.
            {
                Console.WriteLine("Dosya boyutu 512 kb sınırının üzerinde.");
                return;
            }

            // Temizlik öncesi bilgilendirme
            Console.WriteLine("- Dosya bulundu, uzantı uygun ve 512 kb altında.");
            Console.WriteLine("- Her ihtimale karşı dosyanızın yedeğini almanız önerilir.");


            // Dosyayı okuyor - SRT dosyaları genellikle Windows-1254 kodlamasıyla yazıldığı için bu kodlamayı kullanıyorum.
            string srt = File.ReadAllText(dosyaYolu, Encoding.GetEncoding("windows-1254"));

            // Tek satırda tüm silmek istediğim etiketleri | ile birleştirerek bulup kaç adet olduğu bilgisini değişkene atıyorum.
            // Zira kaç etiket bulunduğunun bilgisini daha sonra temizlenenler olarak vereceğim.
            MatchCollection etiketler = Regex.Matches(srt, @"(<[^>]+>|\{[^}]+\})");
            int etiketSayisi = etiketler.Count;

            // Temizleme işlemi
            string temizlenmis = Regex.Replace(srt, @"(<[^>]+>|\{[^}]+\})", "");

            // Dosyaya yazma işlemi             
            File.WriteAllText(dosyaYolu, temizlenmis, Encoding.GetEncoding("windows-1254"));


            Console.WriteLine("- " + etiketSayisi + " adet etiket temizlenerek Srt dosyanız sadeleştirildi.");
            Console.WriteLine("- Lütfen programı sonlandırmak için bir tuşa basın.");

            Console.ReadKey();

        }
    }
}
