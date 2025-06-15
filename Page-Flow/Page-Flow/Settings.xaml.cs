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
            ComboBoxLanguage.SelectedIndex = 0;

            if (Client.GetUserName() != null)
            {
                LabelUser.Content = "[ " + Client.GetUserName() + " ]";
            }
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

            if (await Client.IsConnected()==false)
            {
                MessageBox.Show("Debug");
                PauseAnimation=true;
                if (MessageBox.Show("Adress and or Port are wrong or server can't be reached. Continue without server?", "can't connect to server", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    DialogResult = true;
                }
                bool bruh = await Client.CheckUser("c#user", "1234");
                MessageBox.Show(bruh.ToString());
                IsApplying = false;
                PauseAnimation=false;

                ApplyButton.Content = "apply";

                return;

            }
            else { MessageBox.Show("bruh"); }

            //TODO:Check Client Conection show message if wrong adress and or port

            SettingsValues.SetAPIKey(TextBoxAPIKey.Text);
            //TODO:Check if API key is working if not -> messagebox

            SettingsValues.ReadTextSize = Convert.ToInt32(TextSizeSlider.Value);

            DialogResult = true;
        }

        private async Task Button_Animation(string text, CancellationToken token)
        {
            while (true)
            {
                for (int i = 1; i <= text.Length; i++)
                {
                    ApplyButton.Content = text.Substring(0, i);
                    await Task.Delay(400);
                    while (PauseAnimation)
                    {
                        await Task.Delay(500);
                    }
                    if (!IsApplying) { return; }
                }

                for (int i = text.Length - 1; i >= 0; i--)
                {
                    ApplyButton.Content = text.Substring(0, i);
                    await Task.Delay(400);
                    while (PauseAnimation)
                    {
                        await Task.Delay(500);
                    }
                    if (!IsApplying) { return; }
                }
            }
        }
    }
}
