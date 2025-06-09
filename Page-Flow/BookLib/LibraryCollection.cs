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
                    string sumLikes = values[1];
                    string likes = values[2];
                    string disLikes = values[3];
                    string[] settings = values[4].Split(";");
                    string title = settings[0];
                    string author = settings[1];
                    string license = settings[2];
                    string blurb = settings[3];
                    string note = settings[4];
                    List<string> comments = values[5].Split(";").ToList();
                    if (comments.Count > 0)
                    {
                        comments.RemoveAt(comments.Count - 1);
                    }
                    libraryList.Add(new Library(title, path_, author, license, blurb, note,Convert.ToInt32(sumLikes),Convert.ToInt32(likes), Convert.ToInt32(disLikes), comments,Library.Type.Server));
                }
            }
        }
    }
}
