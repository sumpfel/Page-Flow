using BookLib;
using HTTPClient;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Page_Flow
{
    /// <summary>
    /// Interaction logic for OverviewBookControl.xaml
    /// </summary>
    public partial class OverviewBookControl : UserControl
    {
        public BookCollection BookCollection;
        public HttpControler Client;
        public event EventHandler BookCollectionClicked;
        public event EventHandler BookCollectionDeleted;
        public OverviewBookControl(BookCollection BookCollection_, HttpControler Client_)
        {
            InitializeComponent();
            BookCollection = BookCollection_;
            Client = Client_;

            LabelTitle.Content = BookCollection.Titel;
            LabelLicenseAngabe.Content = BookCollection.License;
            LabelAuthorAngabe.Content = BookCollection.Author;
            LabelNoteAngabe.Content = BookCollection.Note;
            

            UpdateLikes();

            LabelLanguagesAngabe.Content = "";
            foreach (string language in BookCollection.Languages)
                LabelLanguagesAngabe.Content += language + " ";
            LoadThumbnail();
        }

        private void LoadThumbnail()
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            if (BookCollection.ImagePath != null)
            {
                if (BookCollection.ImagePath == "FALSE")
                {
                    bitmap.UriSource = new Uri(System.IO.Path.GetFullPath("resources\\default_book.png"), UriKind.RelativeOrAbsolute);
                    bitmap.EndInit();
                    ImageThumbnail.Source = bitmap;
                }
                else if (File.Exists(BookCollection.ImagePath))
                {
                    bitmap.UriSource = new Uri(System.IO.Path.GetFullPath(BookCollection.ImagePath), UriKind.RelativeOrAbsolute);
                    bitmap.EndInit();
                    ImageThumbnail.Source = bitmap;
                }
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
            LabelComments.Content = $"{BookCollection.Comments.Count} 🗨";
            try
            {
                int likes;
                int disLikes;
                if (BookCollection.FakeLikes < 0)
                {
                    likes = Convert.ToInt32(Convert.ToDouble(BookCollection.Likes) / (BookCollection.DisLikes + BookCollection.Likes - BookCollection.FakeLikes) * 100);
                    disLikes = Convert.ToInt32(Convert.ToDouble(BookCollection.DisLikes - BookCollection.FakeLikes) / (BookCollection.DisLikes + BookCollection.Likes - BookCollection.FakeLikes) * 100);
                }
                else
                {
                    likes = Convert.ToInt32(Convert.ToDouble(BookCollection.Likes + BookCollection.FakeLikes) / (BookCollection.DisLikes + BookCollection.Likes + BookCollection.FakeLikes) * 100);
                    disLikes = Convert.ToInt32(Convert.ToDouble(BookCollection.DisLikes) / (BookCollection.DisLikes + BookCollection.Likes + BookCollection.FakeLikes) * 100);
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
            BookCollectionClicked?.Invoke(this, EventArgs.Empty);
        }

        private void Review_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ReviewWindow window = new ReviewWindow(BookCollection, Client);

            window.ShowDialog();

            if (window.DialogResult == true)
            {
                UpdateLikes();
            }
        }

        private void ButtonDownload_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists("books\\"+BookCollection.Path + "\\vocabs.csv"))
            {
                string downloadsPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Title = "Save Vocabulary File",
                    Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
                    FileName = BookCollection.Titel.Replace(" ","_")+"_vocabulary.csv",
                    InitialDirectory = downloadsPath
                };

                if(saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        List<int> indexes = new List<int>();
                        foreach (string str in BookCollection.Languages)
                        {
                            int index = Array.IndexOf(Translate.Languages_og, str.ToUpper());
                            if (index != -1)
                            {
                                indexes.Add(index);
                            }
                        }
                        Vocab.DownloadVocab("books\\" + BookCollection.Path + "\\vocabs.csv", indexes,saveFileDialog.FileName);
                    }catch (Exception ex)
                    {
                        //TODO:log
                    }
                }
            }
            

        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            ImageThumbnail.Source = null;
            Directory.Delete("books\\" + BookCollection.Path, true);
            BookCollectionDeleted?.Invoke(this, EventArgs.Empty);
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            AddBookWindow addBookWindow = new AddBookWindow(BookCollection);

            addBookWindow.ShowDialog();

            if(addBookWindow.DialogResult == true)
            {

            }
        }
    }
}
