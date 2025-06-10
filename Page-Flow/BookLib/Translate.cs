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
using System.Windows.Controls;

namespace BookLib
{

    public class Translate
    {
        static public string[] Languages_user = new string[] { "EN-US", "EN-GB", "German", "Japanese", "French", "Italian", "Portuguese (Brazilian)", "Portuguese", "Korean", "Chinese Simplified", "Chinese Traditional" };
        static public string[] Languages_target = new string[] { "EN-US", "EN-GB", "DE", "JA", "FR", "IT", "PT-BR", "PT-PT", "KO", "ZH-HANS", "ZH-HANT" };
        static public string[] Languages_og = new string[] { "EN", "EN", "DE", "JA", "FR", "IT", "PT", "PT", "KO", "ZH", "ZH" };

        public static void UpdateCombobox(ComboBox ComboBoxLanguage)
        {
            ComboBoxLanguage.Items.Clear();
            foreach (string language in Translate.Languages_user)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = language;
                ComboBoxLanguage.Items.Add(comboBoxItem);
            }
        }

        public  static string TranslateText(string Text,string TranslateLanguage)
        {
            string authKey = SettingsValues.GetAPIKey();
            
            try
            {
                var translator = new Translator(authKey);

                var translatedText = Task.Run(() => translator.TranslateTextAsync(
                    text: Text,
                    sourceLanguageCode: null,
                    targetLanguageCode: TranslateLanguage
                )).Result;

                return translatedText.Text;
            }
            catch(Exception ex)
            {
                return "failed translation"+ex;
            }

        }
        
    }
}
