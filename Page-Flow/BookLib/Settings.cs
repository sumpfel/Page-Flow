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
        private static string Api_Key { get; set; } = "6ab2d2c4-fffd-4216-a2f4-96f615bcbb53:fx";
        private static string FirstLanguage { get; set; } = "EN-US";

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


                    using (StreamReader sr = new StreamReader("encoded_api_key.pff"))//pff page flow file //TODO encode api key
                    {
                        Api_Key = sr.ReadToEnd();
                    }
                    if (!(Api_Key == ""))
                    {
                        return Api_Key;
                    }
                    else
                    {
                        MessageBox.Show("Select Deep L API key.");
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
            using (StreamWriter sw = new StreamWriter($"encoded_api_key.pff")) //TODO decode API key only if encoded to
            {
                sw.Write(Api_Key);
            }
        }


        public static string GetFirstLanguage()
        {
            if (!(FirstLanguage == ""))
            {
                return FirstLanguage;
            }
            else
            {
                try
                {


                    using (StreamReader sr = new StreamReader("FirstLanguage.pff"))//pff page flow file //TODO encode api key
                    {
                        FirstLanguage = sr.ReadToEnd();
                    }
                    if (!(FirstLanguage == ""))
                    {
                        return FirstLanguage;
                    }
                    else
                    {
                        MessageBox.Show("Select first language.");
                    }
                }
                catch (Exception ex)
                {
                    //TODO
                }
            }
            return "error no api key";
        }

        public static void SetFirstLanguage(string language)
        {
            FirstLanguage = language;
            using (StreamWriter sw = new StreamWriter($"FirstLanguage.pff")) //TODO decode API key only if encoded to
            {
                sw.Write(FirstLanguage);
            }
        }

    }
}
