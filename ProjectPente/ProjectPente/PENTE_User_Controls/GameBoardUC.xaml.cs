﻿using System;
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

namespace ProjectPente.PENTE_User_Controls
{
    /// <summary>
    /// Interaction logic for GameBoard.xaml
    /// </summary>
    public partial class GameBoard : UserControl
    {
        public MainWindow window;

        public GameBoard()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            window.CloseGame();
        }

        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            window.MainMenu();
        }

        private void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            window.Go();
        }
    }
}
