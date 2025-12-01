using System.Globalization;
using System.IO;
using System.Text;

namespace PraceSTextovymiSoubory
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*/ // <-- stačí když jednu ze dvou * smažete a celá studijní část se zakomentuje (a naopak)
            #region Studijní část
            // 1. OTEVŘENÍ SOUBORU
            // 1.1 Adresa k souboru (File PATH):
            //  - RELATIVNÍ vzhledem k EXE souboru (ve složce ...\Váš projekt\bin\Debug\netX.Y\)
            //      - př. "vstupni_soubor.txt" 
            //      - př. @"vstupy\1.txt" -> použijeme zavináč, abychom nemuseli zdvojovat zpětná lomítka (vedou obvykle na vyjádření speciálních znaků)
            //      - př. @"..\vstupy\1.txt" -> jdeme o složku zpět a v ní hledáme složku "vstupy" se souborem 1.txt
            //  - ABSOLUTNÍ adresa: 
            //      - př. @"C:\Users\mysak\Documents\03_TEACHING\02_SEMINAR_Z_PROGRAMOVANI\25_26\PrgSem2\vstup.txt"

            // 1.2 Blok using
            //  - soubor otevíráme vždy v rámci bloku "using", který zajišťuje uzavření souboru a to vždy (i kdyby program v průběhu spadl)
            //  - syntaktická zkratka za try - finally blok (bez catch!) - ve finally se uzavírá soubor a odemyká se pro další používání
            
            // 2. ČTENÍ ZE SOUBORU
            // 2.1 File
            //  - vhodný pro menší až střední soubory, které můžeme přečíst celé najednou (vejdou se celé do paměti)
            //  - neumí číst po částech
            //  - nemusí být v using bloku (uzavření si hlídá sám)
            string text = File.ReadAllText("data.txt");
            string[] lines = File.ReadAllLines("data.txt");

            // 2.2 StreamReader 
            //  - při volání konstuktoru specifikujeme adresu k souboru
            //  - vhodný pro větší soubory a čtení/zápis po částech

            using (StreamReader sr = new StreamReader(@"vstupy\0_vstup.txt"))
            {
                //  - nabízí funkce pro čtení souboru (pokračuje od místa, kde se skončilo číst):
                char prvniZnak = (char)sr.Read(); // přečteme 1 znak, vrací int (-1, když konec souboru)
                string zbytekPrvniRadky = sr.ReadLine(); // od prvního znaku dočte zbytek až do oddělovače řádku
                string zbytekSouboru = sr.ReadToEnd(); // přečte zbytek souboru od msíta, kde se skončilo se čtením

            }

            // 3. ZÁPIS DO SOUBORU
            // 3.1 File
            //  - rozlišujeme mezi WRITE (přepsání) a APPEND (připsání)
            File.WriteAllText("vystup.txt", "Ahoj světe!\nToto je nový soubor."); // \n - oddělovač řádků
            File.AppendAllText("vystup.txt", "Přidáváme další řádku.\n");
            string[] radky = { "První řádek", "Druhý řádek", "Třetí řádek" };
            File.WriteAllLines("vystup.txt", radky); // PŘEPÍŠE několik řádků souborů
            string[] dalsiRadky = { "Čtvrtý řádek", "Pátý řádek" };
            File.AppendAllLines("vystup.txt", dalsiRadky);

            // 3.2 StreamWriter
            using (StreamWriter sw = new StreamWriter("vystup.txt", false)) // false -> PŘEPISOVÁNÍ, true -> PŘIPISOVÁNÍ (append)
            {
                // Upozornění: pokud soubor s takovým jménem neexistoval, vytvoří ho

                sw.Write("Ahoj"); // píše se tam, kde se skončilo
                sw.WriteLine("Toto je nová řádka"); // přidá se oddělení řádku
            }

            // 4. PROBLÉMY
            //  - při práci se soubory může dojít k různým výjimkám -> používáme try-catch bloky
            try
            {
                using (StreamReader sr = new StreamReader(@"vstupy\0_vstup.txt"))
                {
                    // čtení
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Soubor nebyl nalezen: " + ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                // Nemáme práva k souboru nebo složce.
                // Např. zápis do chráněného systému(C:\Windows...) nebo čtení souboru, který je uzamčen pro čtení.
                Console.WriteLine("Nemáš oprávnění: " + ex.Message);
            }
            catch (IOException ex)
            {
                // obecná I/O (input/output) chyba (např. soubor je uzamčen jiným procesem)
                Console.WriteLine("Chyba vstupu/výstupu: " + ex.Message);
            }
            catch
            {
                Console.WriteLine("Nastala nějaká chyba");
            }
            #endregion
            /**/

            #region Praktická část
            // Následující úkoly vyřešte pomocí programování. Metodu "kouknu a vidim" si nechte pro kontrolu.
            // Výsledek si nechte vypsat do konzole a uveďte odpověď v komentáři.
            // Vyřešte vždy i všechny podotázky.

            // Složku vstupni_soubory si přesuňte do ...\bin\Debug\netX.Y\. 
            // Dále si ji otevřte ve VS Code a sledujte obsah souborů. 
            // Doporučení: pro důkladnější pátrání si stáhněte extension do VS Code Inspector Hex nebo Hex Editor.



            //
            using (StreamWriter sw = new StreamWriter(@"..\..\..\..\vstupni_soubory\2.txt"))
            {
                sw.WriteLine("Ahoj \tsvěte!\n");
            }
            // (10b) 1. Jaký je počet znaků v souboru 1.txt a jaký v 2.txt?
            // Zkontrolujte s VS Code a vysvětlete rozdíly.
            // Tip: Při Debugování uvidíte všchny čtené znaky.
            string loadedFile1;
            string loadedFile2;
            using (StreamReader sr1 = new StreamReader(@"..\..\..\..\vstupni_soubory\1.txt"))
            {
                loadedFile1 = sr1.ReadToEnd();
            }
            using (StreamReader sr2 = new StreamReader(@"..\..\..\..\vstupni_soubory\2.txt"))
            {
                loadedFile2 = sr2.ReadToEnd();
            }
            int fileSize1 = loadedFile1.ToCharArray().Length;
            int fileSize2 = loadedFile2.ToCharArray().Length;

            //Console.WriteLine(fileSize1); // 16
            //Console.WriteLine(fileSize2); // 15
            // (10b) 2. Jaký je počet znaků v souboru 1.txt, když pomineme bílé znaky?
            // Tip: Struktura Char má statickou funkci IsWhiteSpace().            
            int fileSizeClear1 = 0;
            foreach(char c in loadedFile1)
            {
                if (!char.IsWhiteSpace(c))
                {
                    fileSizeClear1++;
                }
            }
            //Console.WriteLine(fileSizeClear1); // 10
            using (StreamWriter sw = new StreamWriter(@"..\..\..\..\vstupni_soubory\4.txt"))
            {
                sw.WriteLine("1");
                sw.WriteLine("2");
                sw.WriteLine("3");
            }
            using (StreamWriter sw = new StreamWriter(@"..\..\..\..\vstupni_soubory\5.txt"))
            {
                sw.Write("1\n2\n3");
            }
            // (5b) 3. Jaké znaky (vypište jako integery) jsou použity pro oddělení řádků v souboru 3.txt?
            // Porovnejte s 4.txt a 5.txt.
            // Jakým znakům odpovídají v ASCII tabulce? https://www.ascii-code.com/
            // Zde se stačí podívat do VS Code a napsat sem odpověď, není potřeba nic programovat.
            // 3.txt - 13 a 10
            // 4.txt - 13 a 10
            // 5.txt - 10


            // (10b) 4. Kolik slov má soubor 6.txt?
            // Za slovo teď považujme neprázdnou souvislou posloupnost nebílých znaků oddělené bílými.
            // Tip: Split defaultně odděluje na základě libovolných bílých znaků, ale je tam jeden háček.. jaký?
            // V souboru je vidět 52 slov.
            string loadedFile6;
            using (StreamReader sr4 = new StreamReader(@"..\..\..\..\vstupni_soubory\6.txt"))
            {
                loadedFile6 = sr4.ReadToEnd();
            }
            string[] splitFile6 = loadedFile6.Split();
            //Console.WriteLine(splitFile6.Length); // 63?
            // (15b) 5. Zapište do souboru 7.txt slovo "řeřicha". Povedlo se? 
            // Vypište obsah souboru do konzole. V čem je u konzole problém a jak ho spravit?
            // Jaké kódování používá C#? Kolik bytů na znak? UTF-16 - 2 byty na znak
            using (StreamWriter sw7 = new StreamWriter(@"..\..\..\..\vstupni_soubory\7.txt", true))
            {
                sw7.WriteLine("řěřicha");
            }
            using(StreamReader sr7 = new StreamReader(@"..\..\..\..\vstupni_soubory\7.txt"))
            {
                Console.WriteLine(sr7.ReadToEnd());
            }

            // (25b) 6. Vypište četnosti jednotlivých slov v souboru 8.txt do souboru 9.txt ve formátu slovo:četnost na samostatný řádek.
            // Tentokrát však slova nejprve očištěte od diakritiky a všechna písmena berte jako malá (tak je i ukládejte do slovníku).
            // Tip: Využijte slovník: Dictionary<string, int> slova = new Dictionary<string, int>();
            Dictionary<string, int> wordAmount = new Dictionary<string, int>();
            using (StreamReader sr8 = new StreamReader(@"..\..\..\..\vstupni_soubory\8.txt"))
            {
                string loadedFile8Line = sr8.ReadLine();
                while (loadedFile8Line != null)
                {
                    string[] words = loadedFile8Line.Split();
                    foreach (string word in words)
                    {
                        string editedWord = word.Trim(new char[] { '?','.', '!', ','});
                        editedWord = editedWord.ToLower();
                        editedWord = RemoveDiacritics(editedWord);
                        if (wordAmount.ContainsKey(editedWord))
                        {
                            wordAmount[editedWord]++;
                        }
                        else
                        {
                            wordAmount.Add(editedWord, 1);
                        }
                    }
                    loadedFile8Line = sr8.ReadLine();
                }
            }
            using (StreamWriter sw9 = new StreamWriter(@"..\..\..\..\vstupni_soubory\9.txt"))
            {
              foreach(string key in wordAmount.Keys)
                {
                    sw9.WriteLine($"{key}:{wordAmount[key]}");
                }
            }
            // (+15b) Bonus: Vypište četnosti jednotlivých znaků abecedy (malá a velká písmena) v souboru 8.txt do konzole.

            #endregion
        }

        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
