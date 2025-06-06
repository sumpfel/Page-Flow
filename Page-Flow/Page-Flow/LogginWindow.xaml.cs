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
using HTTPClient;

namespace Page_Flow
{
    /// <summary>
    /// Interaction logic for Loggin.xaml
    /// </summary>
    public partial class LogginWindow : Window
    {
        HttpControler Client;
        public Loggin(HttpControler client)
        {
            InitializeComponent();
            Client = client;
            user_name.Text = Client.GetUserName();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if(await Client.CreateUser(user_name.Text.Trim().Replace(";", ""), password.Text.Trim().Replace(";","")))
            {
                DialogResult=true;
            }
            else
            {
                MessageBox.Show("Creating user Failed... Try diffrent user Name");
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (await Client.CheckUser(user_name.Text.Trim().Replace(";", ""), password.Text.Trim().Replace(";", "")))
            {
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Creating user Failed... Check user name and password");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Client.LogOut();
            DialogResult = true;
        }
    }
}
