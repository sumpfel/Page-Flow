﻿using BookLib;
using DeepL;
using DeepL.Model;
using Serilog;
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


namespace Page_Flow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Log.Logger = new LoggerConfiguration().MinimumLevel.Verbose().
                WriteTo.Console().
                WriteTo.File($".tmp/log.txt", rollingInterval: RollingInterval.Hour).//$".tmp/log_{DateTime.Now.ToString("yyyy-MM-dd_HH")}.txt"
                CreateLogger();


            Log.Logger.Information("MainWindow started ...");
            Log.Logger.Debug("hello world!");
            Log.Logger.Error("ahhhhhhhhhhhh");

            //string TranslatedBook = Translate.TranslateText("私はchristofだ", "ja", "");

            //MessageBox.Show($"translation: {TranslatedBook}");

        }

    }
}