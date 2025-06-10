using BookLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Page_Flow
{
    /// <summary>
    /// Interaction logic for ScrollViewWindow.xaml
    /// </summary>
    public partial class ScrollViewWindow : Window
    {
        Book Book;
        public ScrollViewWindow(Book book)
        {
            InitializeComponent();
            Book = book;

            LabelTitle.Content=Book.Title;

            using(StreamReader sr = new StreamReader(Book.Path +"\\"+ Book.Position[0]+".txt"))
            {
                AddClickableWords(sr.ReadToEnd());
            }
            

        }

        private void AddClickableWords(string text)
        {
            BookText.Text = "";
            string[] words = text.Split(' ');

            foreach (string word in words)
            {
                Run run = new Run(word);
                run.Cursor = Cursors.Hand;

                run.MouseLeftButtonDown += Run_MouseLeftButtonDown;
                BookText.Inlines.Add(run);
                BookText.Inlines.Add(new Run(" "));
            }
        }

        private void Run_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Run clickedRun)
            {
                Point relativePosition = Mouse.GetPosition(this);
                Point screenPosition = PointToScreen(relativePosition);
                string translation = Translate.TranslateText(clickedRun.Text, SettingsValues.GetFirstLanguage());
                MessageBox.Show(Book.Path);
                string path = System.IO.Path.GetDirectoryName(Book.Path) + "\\vocabs.csv";
                MessageBox.Show(path);
                Vocab.AddVocab(Array.IndexOf(Translate.Languages_og, Book.Language),clickedRun.Text, Array.IndexOf(Translate.Languages_target, SettingsValues.GetFirstLanguage()), translation, path);
                TranslationPopUp PopUp = new TranslationPopUp("tranlation:\n"+ translation);
                PopUp.Left = screenPosition.X/1.75;
                PopUp.Top = screenPosition.Y/1.75;
                PopUp.ShowDialog();
            }
        }

    }
}
