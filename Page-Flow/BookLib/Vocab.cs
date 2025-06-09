using DeepL.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLib
{
    internal class Vocab
    {
        public string Path_ = "";
        public string NativeLanguage = "English";

        //TODO for each language a file

        public Dictionary<string, Dictionary<string, string>> Vocabulary = new Dictionary<string, Dictionary<string, string>>{ };

        public void AddVocab(string language, string word)
        {
            word = word.Replace(":","");
            Vocabulary[language].Add(word, Translate.TranslateText(word, NativeLanguage));
        }

        public void save()
        {
            using(StreamWriter sw = new StreamWriter(Path_))
            {
                foreach (var language in Vocabulary.Keys)
                {
                    sw.WriteLine(language);
                    foreach (var word in Vocabulary[language])
                    {
                        sw.WriteLine($"{language}:{word.Key}:{word.Value}");
                    }
                    
                }
            }
        }

        public void load()
        {
            try
            {
                using (StreamReader sr = new StreamReader(Path_))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] vocabs = sr.ReadLine().Split(":");
                        Dictionary <string, string> vocabs_dict = new Dictionary<string, string>();
                        for (int i = 1; i < vocabs.Length; i+=2)
                        {
                            vocabs_dict.Add(vocabs[i], vocabs[i+1]);
                        }

                        Vocabulary.Add(vocabs[0], vocabs_dict);
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
