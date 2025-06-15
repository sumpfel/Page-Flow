using BookLib;
using DeepL.Model;
using HTTPClient;
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
                Grid.SetColumnSpan(LabelNoteAngabe, 4);
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
            DownloadLib();
            Library.Local = Library.Type.Downloaded;
            HideDownloadButton();
            ShowDeleteButton();
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
            ButtonDelete.Background = Brushes.LightGray;
            ButtonDelete.Foreground = Brushes.DarkGray;
            ButtonDelete.BorderBrush = Brushes.DarkGray;
            CanDelete = false;
        }

        private void ShowDeleteButton()
        {
            ButtonDelete.Background = Brushes.LightCoral;
            ButtonDelete.Foreground = Brushes.Crimson;
            ButtonDelete.BorderBrush = Brushes.Crimson;
            CanDelete = true;
        }

        private async void DownloadLib()
        {
            if (CanDownload)
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
            }
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
                Directory.Delete("books/" + Library.Path, true);
                CanDelete = false;
                LibraryDeleted?.Invoke(this, EventArgs.Empty);
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            AddLibrary addLibrary = new AddLibrary();
            addLibrary.ShowDialog();
            if (addLibrary.DialogResult == true)
            {

            }
        }
    }
}
