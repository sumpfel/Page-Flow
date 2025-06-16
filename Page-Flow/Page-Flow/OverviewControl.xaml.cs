using BookLib;
using DeepL.Model;
using HTTPClient;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static BookLib.Library;

namespace Page_Flow
{
    /// <summary>
    /// Interaction logic for OverviewControl.xaml
    /// </summary>
    public partial class OverviewControl : UserControl
    {
        public Library Library;
        public HttpControler Client;
        public event EventHandler LibraryClicked;
        public event EventHandler LibraryDeleted;

        private bool CanDownload;
        private bool CanDelete;
        public OverviewControl(Library Library_, HttpControler client)
        {
            InitializeComponent();
            Library = Library_;
            Client = client;
            LabelTitle.Content = Library.Titel;
            LabelLicenseAngabe.Content = Library.License;
            LabelAuthorAngabe.Content = Library.Author;
            LabelNoteAngabe.Content = Library.Note;
            
            

            UpdateLikes();

            LabelLanguagesAngabe.Content = "";
            foreach (string language in Library.Languages)
            {
                LabelLanguagesAngabe.Content += language + " ";
            }

            if (Library.Local == Library.Type.Local)
            {
                ButtonDownload.Visibility = Visibility.Hidden;
                Grid.SetColumnSpan(LabelNoteAngabe, 3);
                CanDownload = false;
                CanDelete = true;
            }
            else if (Library.Local == Library.Type.Downloaded)
            {
                HideDownloadButton();
                CanDelete = true;
            }
            else
            {
                HideDeleteButton();
                CanDownload = true;
            }
            LoadThumbnail();
        }

        private void LoadThumbnail()
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            if (Library.ImagePath != null)
            {
                if (File.Exists(Library.ImagePath))
                {
                    bitmap.UriSource = new Uri(System.IO.Path.GetFullPath(Library.ImagePath), UriKind.RelativeOrAbsolute);
                    bitmap.EndInit();
                    ImageThumbnail.Source = bitmap;
                }
            }
            else if (Library.ImagePath == "FALSE")
            {
                bitmap.UriSource = new Uri(System.IO.Path.GetFullPath("resources\\default_book.png"), UriKind.RelativeOrAbsolute);
                bitmap.EndInit();
                ImageThumbnail.Source = bitmap;
            }
            else
            {
                bitmap.UriSource = new Uri(System.IO.Path.GetFullPath("resources\\not_found.jpg"), UriKind.RelativeOrAbsolute);
                bitmap.EndInit();
                ImageThumbnail.Source = bitmap;
            }
            
        }

        private void UpdateLikes()
        {
            LabelComments.Content = $"{Library.Comments.Count} 🗨";
            try
            {
                int likes;
                int disLikes;
                if (Library.FakeLikes < 0)
                {
                    likes = Convert.ToInt32(Convert.ToDouble(Library.Likes) / (Library.DisLikes + Library.Likes - Library.FakeLikes) * 100);
                    disLikes = Convert.ToInt32(Convert.ToDouble(Library.DisLikes - Library.FakeLikes) / (Library.DisLikes + Library.Likes - Library.FakeLikes) * 100);
                }
                else
                {
                    likes = Convert.ToInt32(Convert.ToDouble(Library.Likes + Library.FakeLikes) / (Library.DisLikes + Library.Likes + Library.FakeLikes) * 100);
                    disLikes = Convert.ToInt32(Convert.ToDouble(Library.DisLikes) / (Library.DisLikes + Library.Likes + Library.FakeLikes) * 100);
                }
                LabelLikes.Content = $"{likes}% 👍";
                LabelDisLikes.Content = $"{disLikes}% 👎";
            }
            catch
            {
                LabelLikes.Content = $"--- 👍";
                LabelDisLikes.Content = $"--- 👎";
            }
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch (Library.Local){
                case Library.Type.Server:
                    if(MessageBox.Show("You haven't downloaded this Library yet. Do you want to download it?", "Download?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        DownloadLib();
                        Library.Local = Library.Type.Downloaded;
                        LibraryClicked?.Invoke(this, EventArgs.Empty);
                    }                    
                    break;
                default:
                    LibraryClicked?.Invoke(this, EventArgs.Empty);
                    break;
            }
                
        }

        private void ButtonDownload_Click(object sender, RoutedEventArgs e)
        {
            if (CanDownload)
            {
                DownloadLib();
                Library.Local = Library.Type.Downloaded;
                HideDownloadButton();
                ShowDeleteButton();
            }
            
        }

        private void HideDownloadButton()
        {
            ButtonDownload.Background = Brushes.LightGray;
            ButtonDownload.Foreground = Brushes.DarkGray;
            ButtonDownload.BorderBrush = Brushes.DarkGray;
            CanDownload = false;
        }

        private void HideDeleteButton()
        {
            CanDelete = false;

            ButtonDelete.Background = Brushes.LightGray;
            ButtonDelete.Foreground = Brushes.DarkGray;
            ButtonDelete.BorderBrush = Brushes.DarkGray;
            
            ButtonExport.Background = Brushes.LightGray;
            ButtonExport.Foreground = Brushes.DarkGray;
            ButtonExport.BorderBrush = Brushes.DarkGray;

            ButtonEdit.Background = Brushes.LightGray;
            ButtonEdit.Foreground = Brushes.DarkGray;
            ButtonEdit.BorderBrush = Brushes.DarkGray;

        }

        private void ShowDeleteButton()
        {
            CanDelete = true;

            ButtonDelete.Background = Brushes.LightCoral;
            ButtonDelete.Foreground = Brushes.Crimson;
            ButtonDelete.BorderBrush = Brushes.Crimson;

            ButtonExport.Background = Brushes.LawnGreen;
            ButtonExport.Foreground = Brushes.ForestGreen;
            ButtonExport.BorderBrush = Brushes.ForestGreen;

            ButtonEdit.Background = Brushes.Gold;
            ButtonEdit.Foreground = Brushes.Goldenrod;
            ButtonEdit.BorderBrush = Brushes.Goldenrod;
        }

        private async void DownloadLib()
        {
            string path = "books/" + Library.Path+ ".zip";
            bool Succes= await Client.DownloadBook(Library.Path, path);
            if (Succes==false)
            {
                MessageBox.Show("Error 404: Failed downloading book.");
                return;
            }
            ZipFile.ExtractToDirectory(path, "books");
            File.Delete(path);


            if (File.Exists("books\\" + Library.Path + "\\thumbnail.jpg"))
            {
                Library.ImagePath = "books\\" + Library.Path + "\\thumbnail.jpg";
            }
            else if (File.Exists("books\\" + Library.Path + "\\thumbnail.png"))
            {
                Library.ImagePath = "books\\"+Library.Path + "\\thumbnail.png";
            }
            else { Library.ImagePath = "FALSE"; }

            LoadThumbnail();

        }
            

        private void Review_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ReviewWindow window = new ReviewWindow(Library,Client);

            window.ShowDialog();

            if (window.DialogResult == true)
            {
                UpdateLikes();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CanDelete)
            {
                ImageThumbnail.Source = null; 
                Directory.Delete("books/" + Library.Path, true);
                CanDelete = false;
                LibraryDeleted?.Invoke(this, EventArgs.Empty);
            }
        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            if (CanDelete)
            {
                string downloadsPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Title = "Export Library as Page Flow File.",
                    Filter = "PFF Files (*.PFF)|*.PFF|All Files (*.*)|*.*",
                    FileName = Library.Titel.Replace(" ", "_") + ".PFF",
                    InitialDirectory = downloadsPath
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        MessageBox.Show("books\\" + Library.Path);
                        ZipFile.CreateFromDirectory("books\\"+Library.Path, saveFileDialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        //TODO:log
                    }
                }
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {

            if (CanDelete)
            {
                var editWindow = new AddLibrary(Library);
                bool? result = editWindow.ShowDialog();

                if (result == true)
                {
                    // Nach dem Bearbeiten kann man hier z.B. die Library-Daten neu laden, falls AddLibrary die Dateien gespeichert hat
                    Library.Titel = editWindow.TextBoxTitle.Text.Trim();
                    Library.Author = editWindow.TextBoxAuthor.Text.Trim();
                    Library.License = editWindow.TextBoxLicense.Text.Trim();
                    Library.Blurb = editWindow.TextBoxBlurb.Text.Trim();
                    Library.Note = editWindow.TextBoxNote.Text.Trim();

                    // Wenn du möchtest, hier noch die Änderungen in der UI anzeigen
                    LabelTitle.Content = Library.Titel;
                    LabelAuthorAngabe.Content = Library.Author;
                    LabelLicenseAngabe.Content = Library.License;
                    LabelNoteAngabe.Content = Library.Note;

                    // Optional: Thumbnail neu laden, falls geändert
                    if (!string.IsNullOrEmpty(editWindow.ThumbnailPath) && File.Exists(editWindow.ThumbnailPath))
                    {
                        Library.ImagePath = editWindow.ThumbnailPath;
                        LoadThumbnail();
                    }

                    // Du könntest auch direkt die Daten in die settings.csv schreiben oder AddLibrary das machen lassen.
                }
            }

            // Speicherort definieren, z.B. settings.csv im Verzeichnis der Library
            string libraryPath = System.IO.Path.Combine("books", Library.Path, "settings.csv");

            // Speichern
            LibrarySaver.Save(Library, libraryPath);
        }
    }
}
