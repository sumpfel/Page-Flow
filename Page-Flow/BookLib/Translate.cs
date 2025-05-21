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

namespace BookLib
{

    public class Translate
    {

        public async static Task<string> TranslateText(string Text,string OgLanguage,string TranslateLanguage)
        {
            string authKey = "";

            try
            {
                using (StreamReader sr = new StreamReader("encoded_api_key.pff"))//pff page flow file //TODO encode api key
                    {
                        authKey = sr.ReadToEnd();
                    }
            }
            catch
            {
                using (StreamWriter sw = new StreamWriter($"log_{DateTime.Now.ToString("yyyy-MM-dd_HH")}.txt", true))
                {
                    sw.Write("\n\rerror: failed reading saved api key");
                }
            }

            if (authKey == "")
            {
                using (StreamWriter sw = new StreamWriter($"log_{DateTime.Now.ToString("yyyy-MM-dd_HH")}.txt", true))
                {
                    sw.Write("\n\rinfo: requestin api key from user");
                }
                authKey = ""; // TODO: pop up window for user to enter key

                using (StreamWriter sw = new StreamWriter($"encoded_api_key.pff")) //TODO decode API key only if encoded to
                {
                    sw.Write(authKey);
                }

            }
            
            try
            {
                var translator = new Translator(authKey);

                var translatedText = await translator.TranslateTextAsync(
                    Text,
                    OgLanguage,
                    TranslateLanguage);

                return translatedText.Text;
            }
            catch
            {
                using (StreamWriter sw = new StreamWriter($"log_{DateTime.Now.ToString("yyyy-MM-dd_HH")}.txt", true))
                {
                    sw.Write($"\n\rerror: failed translating text: \"\"\"{Text}\"\"\" \n\rOriginalLanguage: {OgLanguage}\n\rTargetLanguage: {TranslateLanguage}");
                }
                return "failed translation";
            }

        }
        
    }
}
