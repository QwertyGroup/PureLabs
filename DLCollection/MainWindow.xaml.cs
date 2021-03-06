﻿using DLCollection.Face;
using DLCollection.Street;
using DLCollection.MSvsCV;
using DLCollection.Demo;

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
using System.Diagnostics;

namespace DLCollection
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Topmost = true;
            Loaded += (s, e) => OnLoaded();
        }

        private void OnLoaded()
        {
            Topmost = false;
        }

        private void GrdActionBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void CmdExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void FaceBtn_Click(object sender, RoutedEventArgs e)
        {
            var wind = new FaceMain() { Topmost = true };
            wind.Show();
            Close();
        }

        private void DemoBtn_Click(object sender, RoutedEventArgs e)
        {
            var wind = new DemoMain() { Topmost = true };
            wind.Show();
            Close();
        }

        private void MSvsCVBtn_Click(object sender, RoutedEventArgs e)
        {
            var wind = new MSvsCVMain() { Topmost = true };
            wind.Show();
            Close();
        }
    }
}
