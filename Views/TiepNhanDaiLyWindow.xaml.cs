﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using WpfAppTemplate.ViewModels;
using System.Windows;

namespace WpfAppTemplate.Views
{
    /// <summary>
    /// Interaction logic for TiepNhanDaiLyWindow.xaml
    /// </summary>
    public partial class TiepNhanDaiLyWindow : Window
    {
        public TiepNhanDaiLyWindow(TiepNhanDaiLyViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
