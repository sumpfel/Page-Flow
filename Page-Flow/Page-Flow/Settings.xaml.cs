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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Loggin window = new Loggin(Client);

            window.ShowDialog();

            if (window.DialogResult == true)
            {

            }
        }
    }
}
