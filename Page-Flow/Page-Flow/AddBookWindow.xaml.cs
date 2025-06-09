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
    /// Interaction logic for AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {
        List<string> librarys = new List<string>();
        public AddBookWindow()
        {
            InitializeComponent();

            string[] SubDirs = Directory.GetDirectories("books");
            int x = 0;
            foreach (string dir in SubDirs)
            {
                if (File.Exists(dir+"\\settings.csv"))
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = dir.Replace("books\\", "");
                    item.Tag = dir.Replace("books\\","");

                    ComboBoxLibrary.Items.Add(item);
                    librarys.Add(dir);
                    if (!File.Exists(dir + "\\downloaded.txt")){
                        ComboBoxLibrary.SelectedIndex = x;
                    }
                    x += 1;
                }
            }
        }

        private bool IsFilledOut()
        {
            return true;
        }

        private void ButtonDeny_Click(object sender, RoutedEventArgs e)
        {
            DialogResult=false;
        }

        private void ButtonAccept_Click(object sender, RoutedEventArgs e)
        {
            if (IsFilledOut())
            {
                string path = librarys[ComboBoxLibrary.SelectedIndex]+ "\\" + TextBoxTitle.Text.Trim().Replace(" ", "_");
                Directory.CreateDirectory(path);
                using (StreamWriter sw = new StreamWriter(path + "\\settings.csv"))
                {
                    sw.WriteLine($"{TextBoxTitle.Text},{TextBoxAuthor.Text},{TextBoxLicense.Text},{TextBoxBlurb.Text},{TextBoxNote.Text},");
                }
            }
        }
    }
}
