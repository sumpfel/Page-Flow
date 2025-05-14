using DeepL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepL;

namespace BookLib
{



    public class Translate
    {
        async static Task TranslateText(string Text,string OgLanguage,string TranslateLanguage)
        {
            var authKey = "";
            var translator = new Translator(authKey);


            var translatedText = await translator.TranslateTextAsync(
                  Text,
                  OgLanguage,
                  TranslateLanguage);
            
        }
        
    }
}
