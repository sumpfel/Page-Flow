using BookLib;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for SelectLanguageWindow.xaml
    /// </summary>
    public partial class SelectLanguageWindow : Window
    {
        public BookCollection BookCollection;
        public string Language;
        public SelectLanguageWindow(BookCollection BookCollection_)
        {
            InitializeComponent();
            BookCollection = BookCollection_;
            UpdateLanguages();
        }

        private void UpdateLanguages()
        {
            View.Children.Clear();
            foreach (string language in BookCollection.Languages)
            {
                try
                {
                    TextBlock commentText = new TextBlock();
                    commentText.Text = language;
                    commentText.FontSize = 20;
                    commentText.Padding = new Thickness(10, 0, 0, 0);
                    commentText.TextWrapping = TextWrapping.Wrap;
                    commentText.MouseLeftButtonUp += LanguageClicked;

                    StackPanel commentsPanel = new StackPanel();
                    commentsPanel.Margin = new Thickness(5);
                    commentsPanel.Children.Add(commentText);
                    

                    Border border = new Border();
                    border.Child = commentsPanel;
                    border.BorderThickness = new Thickness(5);
                    border.CornerRadius = new CornerRadius(10);
                    border.Background = Brushes.CadetBlue;

                    View.Children.Add(border);
                }
                catch { }
            }

        }

        private void LanguageClicked(object sender, RoutedEventArgs e)
        {
            if (sender is TextBlock Text){
                Language=Text.Text;
                DialogResult=true;
            }
        }

    }
}
