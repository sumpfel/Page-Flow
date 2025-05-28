
using System.CodeDom.Compiler;
using System.IO;

namespace BookLib
{

    public class Book
    {
        static public string[] Languages_user = new string[] { "EN-US", "EN-GB", "German", "Japanese", "French", "Italian", "Portuguese (Brazilian)", "Portuguese", "Korean", "Chinese Simplified", "Chinese Traditional" };
        static public string[] Languages_target = new string[] { "EN-US","EN-GB","DE", "JA", "FR", "IT", "PT-BR", "PT-PT", "KO", "ZH-HANS", "ZH-HANT" };
        static public string[] Languages_og = new string[] {     "EN",   "EN",   "DE", "JA", "FR", "IT", "PT",    "PT"   , "KO", "ZH"     , "ZH" };

        static public string[] ChapterSplitter = new string[]{"Kapitel","Chapter"};

        public string Path_ = "";

        public int[] Position = new int[2] { 0, 0 }; //Chapter Seite

        public string Language;

        public bool IsPictureBook = false; //nice to have proparbly will never get implemented



        public Book(string La,string Path) { Language = La; Path_ = Path; }

        public String Load()
        {
            string BookContent = "";
            using (StreamReader sr = new StreamReader($"{Path_}/{Language}/{Position[0]}.txt"))
            {
                BookContent += sr.ReadToEnd();
            }
            return BookContent;
        }

        public void FormatBook(string filePath, bool Chapters=true)
        {
            string BookContent = "";
            using (StreamReader sr = new StreamReader($"{filePath}"))
            {
                BookContent += sr.ReadToEnd();
            }

            if (Chapters)
            {
                string[] splittet = new string[0];
                foreach (string Splitter in ChapterSplitter)
                {
                    splittet = BookContent.Split(Splitter);
                    if (splittet.Length > 1) { break; }
                }
                int i = 0;
                foreach (string Chapter in splittet)
                {
                    i++;
                    using(StreamWriter sw = new StreamWriter($"{Path_}/{Language}/{i}.txt"))
                    {
                        sw.Write(Chapter);
                    }
                }
                return;
            }
            using (StreamWriter sw = new StreamWriter($"{Path_}/{Language}/1.txt"))
            {
                sw.Write(BookContent);
            }
        }

        public void TranslateBook(string OriginalBookPath, string LanguageOg)
        {
            string[] files = Directory.GetFiles(OriginalBookPath);
            List <string> filePaths = new List<string>();
            foreach (string File in files)
            {
                if (Path.GetExtension(File) == "txt")
                {
                    filePaths.Add(File);
                }
            }

            int x = 0;
            foreach (string File in filePaths)
            {
                string Translation;
                
                using(StreamReader sr = new StreamReader(File))
                {
                    Translation = sr.ReadToEnd();
                }

                string TranslatedChapter = Translate.TranslateText(Translation, LanguageOg, Language);

                using (StreamWriter sw = new StreamWriter($"{Path_}/{Language}/{x}.txt"))
                {
                    sw.Write(TranslatedChapter);
                }

                x++;
            }
        }

    }

}
