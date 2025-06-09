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
    /// Interaction logic for TranslationPopUp.xaml
    /// </summary>
    public partial class TranslationPopUp : Window
    {
        public TranslationPopUp(string text)
        {
            InitializeComponent();
            Text.Text = text;
        }
    }
}
