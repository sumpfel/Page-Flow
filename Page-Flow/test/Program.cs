using System;
using System.Transactions;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string TranslatedBook = Translate.TranslateText("私はchrisだ", "japanese", "english").Result;
            Console.WriteLine(TranslatedBook);
        }
    }
}