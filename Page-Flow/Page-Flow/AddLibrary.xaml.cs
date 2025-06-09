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
    /// Interaction logic for AddLibrary.xaml
    /// </summary>
    public partial class AddLibrary : Window
    {
        public AddLibrary()
        {
            InitializeComponent();
        }

        private void ButtonDeny_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void ButtonAccept_Click(object sender, RoutedEventArgs e)
        {
            if (IsFilledOut())
            {
                string path = "books\\" + TextBoxTitle.Text.Trim().Replace(" ", "_");
                Directory.CreateDirectory(path);
                using(StreamWriter sw = new StreamWriter(path + "\\settings.csv"))
                {
                    sw.WriteLine($"{TextBoxTitle.Text},{TextBoxAuthor.Text},{TextBoxLicense.Text},{TextBoxBlurb.Text},{TextBoxNote.Text},");
                }
            }
        }

        private bool IsFilledOut()
        {
            return true;
        }
    }
}
