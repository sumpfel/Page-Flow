using BookLib;
using HTTPClient;
using System;
using System.Collections.Generic;
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

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            AddBookWindow addBookWindow = new AddBookWindow();
            addBookWindow.ShowDialog();
            if (addBookWindow.DialogResult == true)
            {

            }
        }
    }
}
