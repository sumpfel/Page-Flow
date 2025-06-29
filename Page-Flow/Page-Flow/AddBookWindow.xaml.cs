﻿using BookLib;
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
    /// Interaction logic for AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {
        List<string> librarys = new List<string>();
        List<AddLanguageControl1> addLanguageControls = new List<AddLanguageControl1>() {};
        string ThumbnailPath;
        public AddBookWindow()
        {
            InitializeComponent();
            ComboboxLibrarysUpdate();
            AddLanguageControl();
            
        }

        public AddBookWindow(BookCollection bookCollection)
        {
            InitializeComponent();
            ComboboxLibrarysUpdateSelected(bookCollection.Path.Split(new char[] {'/','\\' })[0]);

            TextBoxTitle.Text = bookCollection.Titel;
            TextBoxAuthor.Text = bookCollection.Author;
            TextBoxLicense.Text = bookCollection.License;
            TextBoxBlurb.Text = bookCollection.Blurb;
            TextBoxNote.Text = bookCollection.Note;

            if (!string.IsNullOrEmpty(bookCollection.ImagePath) && File.Exists(bookCollection.ImagePath))
            {
                ThumbnailPath = bookCollection.ImagePath;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(System.IO.Path.GetFullPath(ThumbnailPath), UriKind.Absolute);
                bitmap.EndInit();
                ImageThumbnail.Source = bitmap;

                foreach(string language in bookCollection.Languages)
                {
                    AddLanguageControl1 addLanguageControl = new AddLanguageControl1();
                    addLanguageControls.Add(addLanguageControl);
                    LanguageStackPanel.Children.Insert(LanguageStackPanel.Children.Count - 1, addLanguageControl);

                    addLanguageControl.ComboBoxLanguage.SelectedIndex = Array.IndexOf(Translate.Languages_og, language);
                    addLanguageControl.TextBoxPath.Text = $"books\\{bookCollection.Path}\\{bookCollection.Path.Split(new char[] { '/', '\\' })[1]}{language}\\1.txt";
                }

            }

        }

        private void AddLanguageControl()
        {
            AddLanguageControl1 addLanguageControl = new AddLanguageControl1();
            addLanguageControls.Add(addLanguageControl);
            LanguageStackPanel.Children.Insert(LanguageStackPanel.Children.Count-1, addLanguageControl);
        }
        private void RemoveLanguageControl()
        {
            if (addLanguageControls.Count > 0)
            {
                addLanguageControls.RemoveAt(addLanguageControls.Count-1);
                LanguageStackPanel.Children.RemoveAt(LanguageStackPanel.Children.Count - 2);
            }
            
        }

        private void ComboboxLibrarysUpdate()
        {
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

        private void ComboboxLibrarysUpdateSelected(string selected_library)
        {
            string[] SubDirs = Directory.GetDirectories("books");
            int x = 0;
            foreach (string dir in SubDirs)
            {
                if (File.Exists(dir + "\\settings.csv"))
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = dir.Replace("books\\", "");
                    item.Tag = dir.Replace("books\\", "");

                    ComboBoxLibrary.Items.Add(item);
                    librarys.Add(dir);
                    if (dir.Replace("books\\", "")==selected_library)
                    {
                        ComboBoxLibrary.SelectedIndex = x;
                    }
                    x += 1;
                }
            }
        }

        private bool IsFilledOut()
        {
            if (TextBoxTitle.Text.Replace(" ", "").Length>2)
            {
                if (ComboBoxLibrary.SelectedIndex>=0)
                {
                    if (TextBoxAuthor.Text.Replace(" ", "").Length > 2)
                    {
                        if (TextBoxLicense.Text.Trim().Length > 0)
                        {
                            if(addLanguageControls.Count > 0)
                            {
                                foreach(AddLanguageControl1 addLanguageControl in addLanguageControls)
                                {

                                    if (addLanguageControl.ButtonGenerate.IsChecked == true)
                                    {
                                        if (addLanguageControl.ComboBoxLanguage.SelectedIndex >= 0)
                                        {

                                        }
                                        else { MessageBox.Show("Select Language(s) first."); return false; }
                                    }
                                    else
                                    {
                                        if(addLanguageControl.TextBoxPath.Text.Trim().Length > 0)
                                        {
                                            if (File.Exists(addLanguageControl.TextBoxPath.Text.Trim()) && System.IO.Path.GetExtension(addLanguageControl.TextBoxPath.Text) == ".txt")
                                            {
                                                if (addLanguageControl.ComboBoxLanguage.SelectedIndex >= 0)
                                                {

                                                }
                                                else { MessageBox.Show("Select Language(s) first."); return false; }
                                            }
                                            else { MessageBox.Show("Path for file(s) doesn't exist."); return false; }
                                        }
                                        else { MessageBox.Show("Add a path"); return false; }
                                    }
                                    
                                }
                                return true;
                            }
                            else { MessageBox.Show("Add a Language first."); }
                        }
                        else { MessageBox.Show("Select a License first."); }
                    }
                    else{MessageBox.Show("Author hast to be at least 2 characters without spaces."); }
                }
                else{MessageBox.Show("Select a Library first"); }
            }
            else{MessageBox.Show("Title hast to be at least 2 characters without spaces."); }
            return false;
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
                string Languages = "";
                
                foreach (AddLanguageControl1 LanguageControl in addLanguageControls)
                {
                    string Path2 = path+ "\\" + TextBoxTitle.Text.Trim().Replace(" ", "_")+ Translate.Languages_og[LanguageControl.ComboBoxLanguage.SelectedIndex];
                    Directory.CreateDirectory(Path2);
                    Languages += Translate.Languages_og[LanguageControl.ComboBoxLanguage.SelectedIndex]+"%";

                    if (LanguageControl.ButtonGenerate.IsChecked == true)
                    {
                        string existing_path = "no path";
                        foreach(AddLanguageControl1 LanguageControl2 in addLanguageControls)
                        {
                            if (!string.IsNullOrEmpty(LanguageControl2.TextBoxPath.Text))
                            {
                                existing_path = LanguageControl2.TextBoxPath.Text;
                            }
                        }
                        if (!File.Exists(existing_path))
                        {
                            continue;
                        }
                        string text = "";
                        using (StreamReader sr = new StreamReader(existing_path))
                        {
                            text = sr.ReadToEnd();
                        }
                        text = Translate.TranslateText(text, Translate.Languages_target[LanguageControl.ComboBoxLanguage.SelectedIndex]);
                        using (StreamWriter sw = new StreamWriter(Path2 + "\\1.txt"))
                        {
                            sw.Write(text);
                        }
                    }
                    else
                    {
                        File.Copy(LanguageControl.TextBoxPath.Text, Path2+"\\1.txt");
                    }
                    
                }
                using (StreamWriter sw = new StreamWriter(path + "\\settings.csv"))
                {
                    sw.WriteLine($"{TextBoxTitle.Text},{TextBoxAuthor.Text},{TextBoxLicense.Text},{TextBoxBlurb.Text},{TextBoxNote.Text},{Languages}");
                }
                if (!string.IsNullOrEmpty(ThumbnailPath))
                {

                    File.Copy(ThumbnailPath, path + "\\thumbnail" + System.IO.Path.GetExtension(ThumbnailPath), true);
                }
                DialogResult = true;
            }
        }

        private void AddLanguageButton_Click(object sender, RoutedEventArgs e)
        {
            AddLanguageControl();
        }

        private void RemoveLanguageButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveLanguageControl();
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
