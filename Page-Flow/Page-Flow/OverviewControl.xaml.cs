using BookLib;
using HTTPClient;
using System;
using System.Collections.Generic;
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
using System.IO.Compression;

namespace Page_Flow
{
    /// <summary>
    /// Interaction logic for OverviewControl.xaml
    /// </summary>
    public partial class OverviewControl : UserControl
    {
        Library Library;
        HttpControler Client;
        public OverviewControl(Library Library_, HttpControler client)
        {
            InitializeComponent();
            Library = Library_;
            Client = client;
            LabelTitle.Content = Library_.Titel;
            LabelLicenseAngabe.Content = Library_.License;
            LabelAuthorAngabe.Content = Library_.Author;
            LabelNoteAngabe.Content = Library_.Note;

            LabelLikes.Content = $"{Convert.ToInt32(Convert.ToDouble(Library_.Likes) / (Library_.DisLikes + Library_.Likes) * 100)}% 👍";
            LabelDisLikes.Content = $"{Convert.ToInt32(Convert.ToDouble(Library_.DisLikes) / (Library_.DisLikes + Library_.Likes) * 100)}% 👎";


            foreach (string language in Library_.Languages)
                LabelLanguagesAngabe.Content += language + " ";
            Client = client;
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch (Library.Local){
                case Library.Type.Server:
                    if(MessageBox.Show("You haven't downloaded this Library yet. Do you want to download it?", "Download?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        DownloadLib();
                    }
                    else
                    {
                        break;
                    }
                        break;
                default:
                    
                    break;
            }
                
        }

        private void ButtonDownload_Click(object sender, RoutedEventArgs e)
        {
            DownloadLib();
        }

        private async void DownloadLib()
        {
            MessageBox.Show(Library.Path);
            string path = "books/" + Library.Path+ ".zip";
            bool Succes= await Client.DownloadBook(Library.Path, path);
            if (Succes==false)
            {
                MessageBox.Show("Error 404: Failed downloading book.");
                return;
            }
            ZipFile.ExtractToDirectory(path, "books");
        }
    }
}
