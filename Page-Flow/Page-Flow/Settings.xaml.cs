using BookLib;
using HTTPClient;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        HttpControler Client;
        public Settings(HttpControler client)
        {
            InitializeComponent();
            Client = client;
            Translate.UpdateCombobox(ComboBoxLanguage);
            ComboBoxLanguage.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Loggin window = new Loggin(Client);

            window.ShowDialog();

            if (window.DialogResult == true)
            {
                LabelUser.Content = "[ " + Client.GetUserName() + " ]";
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SettingsValues.SetFirstLanguage(Translate.Languages_target[ComboBoxLanguage.SelectedIndex]);

            Client.Address=TextBoxIP.Text;
            Client.Port=TextBoxPort.Text;
            //TODO:Check Client Conection show message if wrong adress and or port

            SettingsValues.SetAPIKey(TextBoxAPIKey.Text);
            //TODO:Check if API key is working if not -> messagebox

            if (ToggleButtonDarkmode.IsChecked == true) { SettingsValues.DoScrollPage=true; }

            ResourceDictionary dict = new ResourceDictionary();
            if (ToggleButtonDarkmode.IsChecked == true)
            {
                dict.Source = new Uri("DarkTheme.xaml", UriKind.Relative);
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }
            else if(ToggleButtonDarkmode.IsChecked == false)
            {
                dict.Source = new Uri("LightTheme.xaml", UriKind.Relative);
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }
            

        }
    }
}
