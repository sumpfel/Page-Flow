
using System.CodeDom.Compiler;
using System.IO;

namespace BookLib
{

    public class Book
    {
        

        static public string[] ChapterSplitter = new string[]{"Kapitel","Chapter"};

        public string Path = "";
        public string Title;
        public int[] Position = new int[2] { 1, 0 }; //Chapter Page

        public string Language;



        public Book(string title,string La,string Path_) {Title=title; Language = La; Path = Path_; }

        /*
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
        }*/

    }

}
