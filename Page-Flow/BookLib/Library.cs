using Serilog.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BookLib
{
    public class Library
    {
        public string Titel;
        public string Path;
        public string Author;
        public string License;
        public string Blurb;
        public string Note;
        public int Likes;
        public string[] Comments;

        public Library(string titel, string path, string author, string license, string blurb, string note,int likes,string[] comments)
        {
            Titel = titel;
            Path = path;
            Author = author;
            License = license;
            Blurb = blurb;
            Note = note;
            Likes = likes;
            Comments= comments;
        }
    }
}
