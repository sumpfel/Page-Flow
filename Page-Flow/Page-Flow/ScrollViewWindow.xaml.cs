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
using static System.Net.Mime.MediaTypeNames;

namespace Page_Flow
{
    /// <summary>
    /// Interaction logic for ScrollViewWindow.xaml
    /// </summary>
    public partial class ScrollViewWindow : Window
    {
        Book Book;
        int PageNumber;
        List<string> Pages = new List<string>();

        public ScrollViewWindow(Book book)
        {
            InitializeComponent();
            Book = book;

            LabelTitle.Content=Book.Title;

            BookText.FontSize = SettingsValues.ReadTextSize;

            string[] path = Book.Path.Split(new char[] { '/', '\\' });

            LabelPath.Content = $"Page Flow > Home > {path[1]} > {path[2]} > {book.Language} > Chapter {Book.Position[0]} > Page {Book.Position[1]}";

            using (StreamReader sr = new StreamReader(Book.Path +"\\"+ Book.Position[0]+".txt"))
            {
                if (SettingsValues.DoScrollPage)
                {
                    AddClickableWords(sr.ReadToEnd());
                }
                else
                {
                    GeneratePages(sr.ReadToEnd(), 780);//15 Sätze ungefähr * 10 wörter pro satz * 5 Buchstaben pro wort * 20/font size default size 20 //ohne font size weil des trash isch wenn user umstellt + klele weils immer 1 satz weniger nimmt
                    MyScrollViewer.Width = 600;
                    ChangePage(Book.Position[1]);
                }
                    
            }
            

        }

        private void GeneratePages(string text, int char_count)
        {
            string[] sentences = text.Split(new char[] { '?', '!', '.' }, StringSplitOptions.RemoveEmptyEntries);
            
            int x = 0;
            string page = "";
            foreach (string sentence in sentences)
            {

                if (x + sentence.Length >= char_count)
                {
                    Pages.Add(page);
                    page = "";
                    x=0;
                    continue;
                }
                x+=sentence.Length;
                page += sentence+" ";
            }
            Pages.Add(page);
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
                string path = System.IO.Path.GetDirectoryName(Book.Path) + "\\vocabs.csv";
                Vocab.AddVocab(Array.IndexOf(Translate.Languages_og, Book.Language),clickedRun.Text, Array.IndexOf(Translate.Languages_target, SettingsValues.GetFirstLanguage()), translation, path);
                TranslationPopUp PopUp = new TranslationPopUp("tranlation:\n"+ translation);
                PopUp.Left = screenPosition.X/1.75;
                PopUp.Top = screenPosition.Y/1.75;
                PopUp.ShowDialog();
            }
        }

        private void ChangePage(int page_number)
        {
            PageNumber = page_number;
            AddClickableWords(Pages[PageNumber]);
            PageCountLabel.Content = (PageNumber+1).ToString();
            PreviousButton.Visibility = Visibility.Visible;
            NextButton.Visibility = Visibility.Visible;
            if (PageNumber == 0)
            {
                PreviousButton.Visibility = Visibility.Hidden;
            }
            if (PageNumber+1==Pages.Count())
            {
                NextButton.Visibility = Visibility.Hidden;
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(PageNumber+1);
        }
        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(PageNumber-1);
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Book.Position[1] = PageNumber;
            DialogResult = false;
        }

        
    }
}
