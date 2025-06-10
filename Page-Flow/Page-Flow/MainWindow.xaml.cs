using BookLib;
using DeepL;
using DeepL.Model;
using HTTPClient;
using Serilog;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;
using System.Collections.Generic;


namespace Page_Flow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LibraryCollection LibraryCollection = new LibraryCollection();
        HttpControler Client;
        public MainWindow()
        {
            InitializeComponent();

            Log.Logger = new LoggerConfiguration().MinimumLevel.Verbose().
                WriteTo.Console().
                WriteTo.File($".tmp/log.txt", rollingInterval: RollingInterval.Hour).//$".tmp/log_{DateTime.Now.ToString("yyyy-MM-dd_HH")}.txt"
                CreateLogger();


            Log.Logger.Information("MainWindow started ...");
            Client = new HttpControler("127.0.0.1", "5000");
            //await Client.CheckUser("c#user", "1234");

            //string TranslatedBook = Translate.TranslateText("私はchristofだ", "ja", "");

            //MessageBox.Show($"translation: {TranslatedBook}");

            LibraryCollection.LoadFromLocal();
            LoadToView();
        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings window = new Settings(Client);

            window.ShowDialog();

            if (window.DialogResult == true)
            {

            }

        }

        private async void ButtonReload_Click(object sender, RoutedEventArgs e)
        {
            await Client.DownloadPreviewFile("books/preview.csv");
            await Client.DownloadAll("books/all.zip");
            LibraryCollection.LoadFromPreview("books/preview.csv");
            LoadToView();
        }

        private void LoadToView()
        {
            View.Children.Clear();
            foreach (Library library in LibraryCollection.libraryList)
            {
                OverviewControl control = new OverviewControl(library,Client);
                control.LibraryClicked += Control_LibraryClicked;
                control.LibraryDeleted += Control_LibraryDeleted;
                View.Children.Add(control);
            }

        }

        private void Control_LibraryDeleted(object? sender, EventArgs e)
        {
            LibraryCollection.libraryList.Clear();
            LibraryCollection.LoadFromLocal();
            LibraryCollection.LoadFromPreview("books/preview.csv");
            LoadToView();
        }

        private void Control_LibraryClicked(object? sender, EventArgs e)
        {
            if (sender is OverviewControl Control)
            {
                View.Children.Clear();
                Control.Library.LoadBooks();
                foreach (BookCollection bookCollection in Control.Library.bookCollections)
                {
                    OverviewBookControl overviewBookControl = new OverviewBookControl(bookCollection,Control.Client);
                    overviewBookControl.BookCollectionClicked += Control_BookCollectionClicked;
                    View.Children.Add(overviewBookControl);
                }

            }
        }

        private void Control_BookCollectionClicked(object? sender, EventArgs e)
        {
            if (sender is OverviewBookControl Control)
            {
                //MessageBox.Show(Control.BookCollection.Path);
                SelectLanguageWindow window = new SelectLanguageWindow(Control.BookCollection);

                window.ShowDialog();

                if (window.DialogResult == true)
                {
                    foreach (Book book in Control.BookCollection.Books)
                    {
                        if(book.Language==window.Language.ToUpper())
                        {
                            ScrollViewWindow scrollViewWindow = new ScrollViewWindow(book);
                            scrollViewWindow.ShowDialog();
                            if(scrollViewWindow.DialogResult == true)
                            {
                                
                            }
                        }
                    }
                    
                }
            }
        }

        private void ButtonUpload_Click(object sender, RoutedEventArgs e)
        {
            CreateBookOrLibrary window = new CreateBookOrLibrary();
            window.ShowDialog();
            if(window.DialogResult == true)
            {
                if (window.Create == 1)
                {
                    AddLibrary addLibrary = new AddLibrary();
                    addLibrary.ShowDialog();
                    if(addLibrary.DialogResult == true)
                    {

                    }
                }
                else if(window.Create == 2)
                {
                    AddBookWindow addBookWindow = new AddBookWindow();
                    addBookWindow.ShowDialog();
                    if (addBookWindow.DialogResult == true)
                    {

                    }
                }
            }
        }

        private void SearchForLibrary(string libraryTitle)
        {
            //Prompt: c# sort a LibraryCollection.libraryList for a search term matching Library.Title keep it simple
            LibraryCollection.libraryList = LibraryCollection.libraryList.OrderByDescending(library => library.Titel.Contains(libraryTitle, StringComparison.OrdinalIgnoreCase)).ToList();
            LoadToView();
        }

        private void TextBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchForLibrary(TextBoxSearch.Text.Trim());
            }
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SearchForLibrary(TextBoxSearch.Text.Trim());
        }
    }
}