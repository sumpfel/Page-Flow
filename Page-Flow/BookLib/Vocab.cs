using DeepL.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookLib
{
    public class Vocab
    {

        public static void AddVocab(int language1, string word1, int language2, string word2, string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    using(StreamWriter sw = new StreamWriter(path))
                    {
                        foreach(string language in Translate.Languages_user)
                        {
                            sw.Write(language+",");
                        }
                        sw.WriteLine();
                    }
                }
                using (StreamWriter sw = new StreamWriter(path,true))
                {
                    for(int i=0; i<Translate.Languages_target.Count(); i++)
                    {
                        if (language1 == i)
                        {
                            sw.Write(word1 + ",");
                        }
                        else if (language2 == i)
                        {
                            sw.Write(word2 + ",");
                        }
                        else
                        {
                            sw.Write(",");
                        }
                    }
                    sw.WriteLine();
                }
                
            }catch (Exception ex)
            {
                //TODO:log to file
            }
            
        }

        public static void DownloadVocab(string path,List<int> languages, string target_path)
        {
            languages.Add(Array.IndexOf(Translate.Languages_target, SettingsValues.GetFirstLanguage()));
            languages=languages.Distinct().ToList();
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    using(StreamWriter sw = new StreamWriter(target_path))
                    {
                        while (!sr.EndOfStream)
                        {
                            string[] vocabs = sr.ReadLine().Split(",");
                            foreach (int i in languages)
                            {
                                sw.Write(vocabs[i] + ",");
                            }
                            sw.WriteLine();
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                //TODO:log to file
            }
        }


    }
}
