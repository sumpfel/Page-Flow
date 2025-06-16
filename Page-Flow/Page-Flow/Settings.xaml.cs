using BookLib;
using HTTPClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        bool IsApplying = false;
        bool PauseAnimation = false;
        public Settings(HttpControler client)
        {
            InitializeComponent();
            Client = client;
            Translate.UpdateCombobox(ComboBoxLanguage);

            int language = Array.IndexOf(Translate.Languages_target, SettingsValues.GetFirstLanguage());

            if (language >=0)
            {
                ComboBoxLanguage.SelectedIndex = language;
            }

            if (Client.GetUserName() != null)
            {
                LabelUser.Content = "[ " + Client.GetUserName() + " ]";
            }
            TextBoxIP.Text = Client.Address;
            TextBoxPort.Text = Client.Port;

            TextSizeSlider.Value = SettingsValues.ReadTextSize;
            ToggleButtonScroll.IsChecked = !SettingsValues.DoScrollPage;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Loggin window = new Loggin(Client);

            window.ShowDialog();

            if (window.DialogResult == true)
            {

            }
            if (Client.GetUserName() != null)
            {
                LabelUser.Content = "[ " + Client.GetUserName() + " ]";
            }
            else
            {
                LabelUser.Content = "[no user logged in]";
            }
        }

        private async void Button_Click_Apply(object sender, RoutedEventArgs e)
        {
            if (IsApplying)
            {
                return;
            }
            IsApplying = true;

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;

            var animation = Button_Animation("please wait...",token);

            SettingsValues.SetFirstLanguage(Translate.Languages_target[ComboBoxLanguage.SelectedIndex]);

            Client.Address = TextBoxIP.Text;
            Client.Port = TextBoxPort.Text;

            if (await Client.CheckConnection()==false)
            {
                PauseAnimation=true;
                if (MessageBox.Show("Adress and or Port are wrong or server can't be reached. Continue without server?", "can't connect to server", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    DialogResult = true;
                }
                IsApplying = false;
                PauseAnimation=false;

                ApplyButton.Content = "apply";

                return;

            }

            SettingsValues.SetAPIKey(TextBoxAPIKey.Text);
            //TODO:Check if API key is working if not -> messagebox

            SettingsValues.ReadTextSize = Convert.ToInt32(TextSizeSlider.Value);
            
            SettingsValues.DoScrollPage = !(ToggleButtonScroll.IsChecked.Value);

            await Task.Delay(2000);//einfach damit i mine coole animation net umsunsch gmacht hon

            if (!SettingsValues.Save("settings/general.csv"))
            {
                MessageBox.Show("error: Could not save General settings to file. You may enter the settings again after restart.");
            }

            if (!Client.Save("settings/client.csv"))
            {
                MessageBox.Show("error: Could not save Server settings. You may enter the settings again after restart.");
            }

            DialogResult = true;
        }

        private async Task Button_Animation(string text, CancellationToken token)
        {
            while (true)
            {
                for (int i = 1; i <= text.Length; i++)
                {
                    ApplyButton.Content = text.Substring(0, i);
                    await Task.Delay(100);
                    while (PauseAnimation)
                    {
                        await Task.Delay(500);
                    }
                    if (!IsApplying) { return; }
                }

                for (int i = text.Length - 1; i >= 0; i--)
                {
                    ApplyButton.Content = text.Substring(0, i);
                    await Task.Delay(100);
                    while (PauseAnimation)
                    {
                        await Task.Delay(500);
                    }
                    if (!IsApplying) { return; }
                }
            }
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow window = new AboutWindow();

            window.ShowDialog();
        }
    }
}
