using BookLib;
using Microsoft.Win32;
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
        public string ThumbnailPath;
        public AddLibrary()
        {
            InitializeComponent();
        }

        public AddLibrary(Library lib) : this()
        {
            TextBoxTitle.Text = lib.Titel;
            TextBoxAuthor.Text = lib.Author;
            TextBoxLicense.Text = lib.License;
            TextBoxBlurb.Text = lib.Blurb;
            TextBoxNote.Text = lib.Note;

            // Thumbnail laden (falls vorhanden)
            if (!string.IsNullOrEmpty(lib.ImagePath) && File.Exists(lib.ImagePath))
            {
                ThumbnailPath = lib.ImagePath;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(System.IO.Path.GetFullPath(ThumbnailPath), UriKind.Absolute);
                bitmap.EndInit();
                ImageThumbnail.Source = bitmap;
            }
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
                using (StreamWriter sw = new StreamWriter(path + "\\settings.csv"))
                {
                    sw.WriteLine($"{TextBoxTitle.Text},{TextBoxAuthor.Text},{TextBoxLicense.Text},{TextBoxBlurb.Text},{TextBoxNote.Text},");
                }
                if (!string.IsNullOrEmpty(ThumbnailPath))
                {
                    File.Copy(ThumbnailPath, path + "\\thumbnail" + System.IO.Path.GetExtension(ThumbnailPath), true);
                }

                DialogResult = true;
                Close();
            }
            else
            {
                // Bei ungültigen Eingaben passiert nichts, Meldungen kommen von IsFilledOut()
            }
        }

        private bool IsFilledOut()
        {
            if (TextBoxTitle.Text.Replace(" ", "").Length > 2)
            {
                if (TextBoxAuthor.Text.Replace(" ", "").Length > 2)
                {
                    if (TextBoxLicense.Text.Trim().Length > 0)
                    {
                        return true;
                    }
                    else { MessageBox.Show("Select a License first."); }
                }
                else { MessageBox.Show("Author hast to be at least 2 characters without spaces."); }
            }
            else { MessageBox.Show("Title hast to be at least 2 characters without spaces."); }
            return false;
        }

        private void ButtonThumbnail_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*",
                Title = "Select a Text File"
            };

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                if (File.Exists(openFileDialog.FileName) && (System.IO.Path.GetExtension(openFileDialog.FileName) == ".jpg" || System.IO.Path.GetExtension(openFileDialog.FileName) == ".png"))
                {
                    ThumbnailPath = openFileDialog.FileName;

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(System.IO.Path.GetFullPath(ThumbnailPath), UriKind.Absolute);
                    bitmap.EndInit();
                    ImageThumbnail.Source = bitmap;
                }
            }
        }
    }
}
