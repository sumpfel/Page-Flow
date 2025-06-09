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
    /// Interaction logic for CreateBookOrLibrary.xaml
    /// </summary>
    public partial class CreateBookOrLibrary : Window
    {
        public int Create = 0;
        public CreateBookOrLibrary()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Create = 2;
            DialogResult=true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Create = 1;
            DialogResult = true;
        }
    }
}
