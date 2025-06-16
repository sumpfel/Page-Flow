using Serilog.Debugging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookLib
{
    public class Library : IReviewable
    {
        public string Titel;
        public string Path { get; set; }
        public string Author;
        public string License;
        public string Blurb;
        public string Note;
        public int SumLikes;
        public string ImagePath;
        public int Likes { get; set; }
        public int DisLikes { get; set; }
        public int FakeLikes { get; set; } = 0;
        public List<string> Comments { get; set; } = new List<string> { };
        public List<string> Languages = new List<string> {"en"};
        public List<BookCollection> bookCollections = new List<BookCollection> { };
        public enum Type
        {
            Local=0,
            Downloaded=1,
            Server=2
        }
        public Type Local= Type.Local;

        public Library(string titel, string path, string author, string license, string blurb, string note,int sum_likes, int likes, int dis_likes, List<string> comments,Type type,List<string> languages)
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
            Languages= languages;
        }

        public string Serialize()
        {
            // Beispiel: ";" als Trennzeichen, evtl. bei Strings Escape einbauen wenn nötig
            string serializedBooks = $"{Titel}|{Author}|{License}|{Note}|{Path}";
            return serializedBooks;
        }

        public static class LibrarySaver
        {
            public static void Save(Library library, string path)
            {
                using (StreamWriter writer = new StreamWriter(path, append: false))
                {
                    writer.WriteLine(library.Serialize());
                }
            }
        }

        public void LoadBooks()
        {
            bookCollections.Clear();
            string Path_ = "books\\" + Path;
            //MessageBox.Show(Path_);
            if(Local==Type.Local || Local== Type.Downloaded){
                if (Directory.Exists(Path_))
                {
                    string[] SubDirs = Directory.GetDirectories(Path_);
                    foreach (string dir in SubDirs)
                    {
                        //MessageBox.Show(dir);
                        BookCollection bookCollection = new BookCollection();
                        bookCollection.Path = dir.Replace("books\\","");
                        string settingsFilePath = dir + "\\settings.csv";
                        string commentsFilePath = dir + "\\comments.csv";
                        string votesFilePath = dir + "\\votes.txt";
                        if (File.Exists(settingsFilePath))
                        {
                            //MessageBox.Show(settingsFilePath);
                            using(StreamReader sr = new StreamReader(settingsFilePath))
                            {
                                string str = sr.ReadToEnd().Trim();
                                string[] settings = str.Split(",");
                                bookCollection.Titel = settings[0];
                                bookCollection.Author = settings[1];
                                bookCollection.License = settings[2];
                                bookCollection.Blurb = settings[3];
                                bookCollection.Note = settings[4];
                                List<string> languages = settings[5].Split("%").ToList();
                                if (languages.Count > 0)
                                {
                                    languages.RemoveAt(languages.Count - 1);
                                }
                                bookCollection.Languages = languages;
                                bookCollections.Add(bookCollection);
                            }

                            if (File.Exists(dir + "\\thumbnail.jpg"))
                            {
                                bookCollection.ImagePath = dir + "\\thumbnail.jpg";
                            }
                            else if (File.Exists(dir + "\\thumbnail.png"))
                            {
                                bookCollection.ImagePath = dir + "\\thumbnail.png";
                            }
                            else { bookCollection.ImagePath = "FALSE"; }

                            if (File.Exists(commentsFilePath))
                            {
                                using (StreamReader sr = new StreamReader(commentsFilePath))
                                {
                                    while (!sr.EndOfStream)
                                    {
                                        string comment = sr.ReadLine();
                                        if (comment != null)
                                        {
                                            bookCollection.Comments.Add(comment.Replace("*","@@"));
                                        }
                                        
                                    }
                                }
                            }
                            if (File.Exists(votesFilePath))
                            {
                                using (StreamReader sr = new StreamReader(votesFilePath))
                                {
                                    string Ratings = sr.ReadToEnd().Trim();
                                    string[] votes = Ratings.Split(",");
                                    bookCollection.SumLikes = Convert.ToInt32(votes[0]);
                                    bookCollection.Likes = Convert.ToInt32(votes[1]);
                                    bookCollection.DisLikes = Convert.ToInt32(votes[2]);
                                }
                            }
                            bookCollection.LoadBooks();
                        }

                    }
                }
            }
        }
    }
}
