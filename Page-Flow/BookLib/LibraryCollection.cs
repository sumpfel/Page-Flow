using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BookLib
{
    public class LibraryCollection
    {
        public List<Library> libraryList = new List<Library> { };

        public LibraryCollection() { }

        public void LoadFromPreview(string path)
        {
            libraryList.Clear();
            using(StreamReader sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] values = line.Split(",");
                    string path_ = values[0];
                    string likes = values[1];
                    string[] settings = values[2].Split(";");
                    string title = settings[0];
                    string author = settings[1];
                    string license = settings[2];
                    string blurb = settings[3];
                    string note = settings[4];
                    string[] comments = values[3].Split(";");
                    libraryList.Add(new Library(title, path, author, license, blurb, note,Convert.ToInt32(likes),comments));
                }
            }
        }
    }
}
