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
    /// Interaction logic for TextInputWindow.xaml
    /// </summary>
    public partial class TextInputWindow : Window
    {
        public TextInputWindow(string question)
        {
            InitializeComponent();
            QuestionLabel.Content = question;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(InputTextBox.Text))
            {
                DialogResult=true;
            }
            else { MessageBox.Show("You can't leave it blank"); }
        }

        private void Deny_Click(object sender, RoutedEventArgs e)
        {
            DialogResult=false;
        }
    }
}
