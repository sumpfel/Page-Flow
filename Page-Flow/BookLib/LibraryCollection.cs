using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace BookLib
{
    public class LibraryCollection
    {
        public List<Library> libraryList = new List<Library> { };

        public LibraryCollection() { }

        public void LoadFromPreview(string path)
        {
            try
            {
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
                    
                        bool already_in_list=false;

                        foreach (Library library in libraryList)
                        {
                            if (library.Titel == title)
                            {
                                already_in_list = true;
                                break;
                            }
                        }
                        if (already_in_list)
                        {
                            continue;
                        }

                        string author = settings[1];
                        string license = settings[2];
                        string blurb = settings[3];
                        string note = settings[4];
                        List<string> languages = settings[5].Split("%").ToList();
                        List<string> comments = values[5].Split(";").ToList();
                        if (comments.Count > 0)
                        {
                            comments.RemoveAt(comments.Count - 1);
                        }
                        if (languages.Count > 0)
                        {
                            languages.RemoveAt(languages.Count - 1);
                        }
                        libraryList.Add(new Library(title, path_, author, license, blurb, note,Convert.ToInt32(sumLikes),Convert.ToInt32(likes), Convert.ToInt32(disLikes), comments,Library.Type.Server,languages));
                    }
                }
            }catch (Exception ex)
            {
                //TODO:log
            }
            
        }

        public void LoadFromLocal()
        {
            string[] SubDirs = Directory.GetDirectories("books");
            foreach (string dir in SubDirs)
            {
                try
                {
                    Library Library;
                    using (StreamReader sr = new StreamReader(dir + "\\settings.csv"))
                    {
                        string[] settings = sr.ReadToEnd().Trim().Split("|");
                        string title = settings[0];
                        string author = settings[1];
                        string license = settings[2];
                        string blurb = settings[3];
                        string note = settings[4];
                        List<string> languages = settings[5].Split("%").ToList();
                        if (languages.Count > 0)
                        {
                            languages.RemoveAt(languages.Count - 1);
                        }
                        Library = new Library(title, dir.Replace("books\\", ""), author, license, blurb, note, 0, 0, 0, new List<string> { }, Library.Type.Local, languages);
                        libraryList.Add(Library);
                    }
                    if (File.Exists(dir + "\\thumbnail.jpg"))
                    {
                        Library.ImagePath = dir + "\\thumbnail.jpg";
                    }
                    else if (File.Exists(dir + "\\thumbnail.png"))
                    {
                        Library.ImagePath = dir + "\\thumbnail.png";
                    }
                    else { Library.ImagePath = "FALSE"; }

                    if (File.Exists(dir + "\\downloaded.txt"))
                    {
                        Library.Local = Library.Type.Downloaded;
                        string commentsFilePath = dir + "\\comments.csv";
                        string votesFilePath = dir + "\\votes.txt";
                        if (File.Exists(commentsFilePath))
                        {
                            using (StreamReader sr = new StreamReader(commentsFilePath))
                            {
                                while (!sr.EndOfStream)
                                {
                                    string comment = sr.ReadLine();
                                    if (comment != null)
                                    {
                                        Library.Comments.Add(comment.Replace("*", "@@"));
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
                                Library.SumLikes = Convert.ToInt32(votes[0]);
                                Library.Likes = Convert.ToInt32(votes[1]);
                                Library.DisLikes = Convert.ToInt32(votes[2]);
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    //TODO: Log
                }
                
            }
        }
    }
}
