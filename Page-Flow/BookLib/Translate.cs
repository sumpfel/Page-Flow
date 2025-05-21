using DeepL;
using DeepL;
using DeepL.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;

namespace BookLib
{

    public class Translate
    {

        public  static string TranslateText(string Text,string OgLanguage,string TranslateLanguage)
        {
            string authKey = "6ab2d2c4-fffd-4216-a2f4-96f615bcbb53:fx";//

            try
            {
                using (StreamReader sr = new StreamReader("encoded_api_key.pff"))//pff page flow file //TODO encode api key
                    {
                        authKey = sr.ReadToEnd();
                    }
            }
            catch
            {
                using (StreamWriter sw = new StreamWriter($".tmp/log_{DateTime.Now.ToString("yyyy-MM-dd_HH")}.txt", true))
                {
                    sw.Write("\n\rerror: failed reading saved api key");
                }
            }

            if (authKey == "")
            {
                using (StreamWriter sw = new StreamWriter($".tmp/log_{DateTime.Now.ToString("yyyy-MM-dd_HH")}.txt", true))
                {
                    sw.Write("\n\rinfo: requestin api key from user");
                }
                authKey = "6ab2d2c4-fffd-4216-a2f4-96f615bcbb53:fx"; // TODO: pop up window for user to enter key

                using (StreamWriter sw = new StreamWriter($"encoded_api_key.pff")) //TODO decode API key only if encoded to
                {
                    sw.Write(authKey);
                }

            }
            
            try
            {
                MessageBox.Show("try");
                var translator = new Translator(authKey);

                using (StreamWriter sw = new StreamWriter($".tmp/log_{DateTime.Now.ToString("yyyy-MM-dd_HH")}.txt", true))
                {
                    sw.Write($"\n\rinfo: start translating text: \"\"\"{Text}\"\"\" \n\rOriginalLanguage: {OgLanguage}\n\rTargetLanguage: {TranslateLanguage}");
                }

                var translatedText = Task.Run(() => translator.TranslateTextAsync(
                    "私はchristofだ",
                    "ja", // Language code for Japanese
                    "en-GB"  // Language code for English
                )).Result;

                //var translatedText = await translator.TranslateTextAsync(
                //    Text,
                //    OgLanguage,
                //    TranslateLanguage);

                return translatedText.Text;
            }
            catch
            {
                MessageBox.Show("catch");
                using (StreamWriter sw = new StreamWriter($".tmp/log_{DateTime.Now.ToString("yyyy-MM-dd_HH")}.txt", true))
                {
                    sw.Write($"\n\rerror: failed translating text");
                }
                return "failed translation";
            }

        }
        
    }
}
