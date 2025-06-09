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
                BookText.Text=sr.ReadToEnd();
            }
            

        }
    }
}
