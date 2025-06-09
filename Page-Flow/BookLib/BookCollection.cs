using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLib
{
    public class BookCollection : IReviewable
    {
        //TODO all
        public List<string> Comments { get; set; } = new List<string>();
        public int SumLikes;
        public int Likes { get; set; }
        public int DisLikes { get; set; }
        public int FakeLikes { get; set; } = 0;
        public string Titel;
        public string Path { get; set; }
        public string Author;
        public string License;
        public string Blurb;
        public string Note;
        public List<string> Languages = new List<string> { "en", "de" };

        public string Path_ = "";

        private List<Book> Books = new List<Book> { };

        public BookCollection() { }


        public void Add_Language(string language,string filepath, bool generate)
        {
            if (!generate)
            {
                Book book = new Book(language,Path_);
                book.FormatBook(filepath);
                Books.Add(book);
            }
            else
            {
                Book book = new Book(language,Path_);
                book.TranslateBook($"{Books[0].Path_}/{Books[0].Language}/", $"{Books[0].Language}");
                Books.Add(book);
            }
            
        }

        public void Remove_Language(string language)
        {
            for (int i = Books.Count-1; i >= 0; i--)
            {
                if (Books[i].Language == language)
                {
                    Books.RemoveAt(i);
                }
            }
        }

        public void Save()
        {
            using (StreamWriter sw = new StreamWriter($"{Path_}/settings.txt"))
            {
                //TODO:save settings of book
            }
        }
    }
}
