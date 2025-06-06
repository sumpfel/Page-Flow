﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for OverviewControl.xaml
    /// </summary>
    public partial class OverviewControl : UserControl
    {
        public OverviewControl(string title,string license,string author,string note,int likes,List<string> languages)
        {
            InitializeComponent();
            LabelTitle.Content = title;
            LabelLicenseAngabe.Content= license;
            LabelAuthorAngabe.Content = author;
            LabelNoteAngabe.Content = note;

            //TODO likes

            foreach(string language in languages)
            LabelLanguagesAngabe.Content += language+" ";
        }
    }
}
