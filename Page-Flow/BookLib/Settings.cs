using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookLib
{
    public class SettingsValues
    {
        private static string Api_Key = "6ab2d2c4-fffd-4216-a2f4-96f615bcbb53:fx";
        private static string FirstLanguage = "EN-US";

        public static int ReadTextSize { get; set; } = 20;
        public static bool DoScrollPage { get; set; }=true;

        public static string GetAPIKey()
        {
            if(!(Api_Key == ""))
            {
                return Api_Key;
            }
            else
            {
                try
                {


                    using (StreamReader sr = new StreamReader("settings/api_key.key"))
                    {
                        Api_Key = sr.ReadToEnd();
                    }
                    if (!(Api_Key == ""))
                    {
                        return Api_Key;
                    }
                    else
                    {
                        MessageBox.Show("Set Deep L API key in settings.");
                    }
                }
                catch (Exception ex)
                {
                    //TODO
                }
            }
            return "error no api key";
        }
        public static void SetAPIKey(string key)
        {
            Api_Key = key;
            using (StreamWriter sw = new StreamWriter($"settings/api_key.key"))
            {
                sw.Write(Api_Key);
            }
        }


        public static string GetFirstLanguage()
        {
            if (string.IsNullOrWhiteSpace(FirstLanguage))
            {
                MessageBox.Show("Select first language.");
            }
            return FirstLanguage;
        }

        public static void SetFirstLanguage(string language)
        {
            FirstLanguage = language;
        }

        public static bool Save(string path)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.Write($"{FirstLanguage}|{ReadTextSize}|{DoScrollPage}");
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool Load(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string[] settings = sr.ReadToEnd().Split('|');
                    FirstLanguage = settings[0];
                    ReadTextSize = Convert.ToInt32(settings[1]);
                    DoScrollPage = Convert.ToBoolean(settings[2]);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

    }
}
