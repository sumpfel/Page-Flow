
using System.CodeDom.Compiler;
using System.IO;

namespace BookLib
{
    public enum Languages
            {
                en,
                de,
                ja,
                fr,
                sp,
                ch,
            }

    public class Book
    {
        public string[] ChapterSplitter = new string[]{"Kapitel","Chapter"};

        public int[] Position = new int[2] { 0, 0 };

        public Int16 Language;

        public bool IsPictureBook = false; //nice to have proparbly will never get implemented

        public Book(Int16 La) { Language = La; }

        public String Load(string bookPath)
        {
            string BookContent = "";
            using (StreamReader sr = new StreamReader($"{bookPath}_{Language}"))
            {
                BookContent += sr.ReadToEnd();
            }
            return BookContent;
        }

        public void FormatBook(string filePath, string bookPath, bool Chapters)
        {
            string BookContent = "";
            using (StreamReader sr = new StreamReader($"{bookPath}_{Language}"))
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
                    using(StreamWriter sw = new StreamWriter($"{bookPath}_{Language}/{i}.txt"))
                    {
                        sw.Write(Chapter);
                    }
                }
                return;
            }
            using (StreamWriter sw = new StreamWriter($"{bookPath}_{Language}/1.txt"))
            {
                sw.Write(BookContent);
            }
        }

        public void TranslateBook(string OriginalBookPath, string TargetLanguage)
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

            foreach (string File in filePaths)
            {
                string Translation;
                
                using(StreamReader sr = new StreamReader(File))
                {
                    Translation = sr.ReadToEnd();
                }

                string TranslatedBook = Translate.TranslateText(Translation,((Languages)Language).ToString(), TargetLanguage);
                //TODO: safe to new files

            }
        }

    }

}
