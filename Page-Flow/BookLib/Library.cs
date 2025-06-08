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
        public int SumLikes;
        public int Likes;
        public int DisLikes;
        public int FakeLikes=0;
        public List<string> Comments;
        public List<string> Languages = new List<string> {"en", "de"};
        public enum Type
        {
            Local=0,
            Downloaded=1,
            Server=2
        }
        public Type Local= Type.Local;

        public Library(string titel, string path, string author, string license, string blurb, string note,int sum_likes, int likes, int dis_likes, List<string> comments,Type type)
        {
            Titel = titel;
            Path = path;
            Author = author;
            License = license;
            Blurb = blurb;
            Note = note;
            SumLikes = sum_likes;
            Likes = likes;
            DisLikes = dis_likes;
            Comments= comments;
            Local= type;
        }
    }
}
