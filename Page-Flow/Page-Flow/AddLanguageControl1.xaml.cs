using BookLib;
using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Page_Flow
{
    /// <summary>
    /// Interaction logic for AddLanguageControl1.xaml
    /// </summary>
    public partial class AddLanguageControl1 : UserControl
    {
        public AddLanguageControl1()
        {
            InitializeComponent();
            Translate.UpdateCombobox(ComboBoxLanguage);
        }

        private void ButtonBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = "Select a Text File"
            };

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                TextBoxPath.Text = openFileDialog.FileName;
            }
        }
    }
}
