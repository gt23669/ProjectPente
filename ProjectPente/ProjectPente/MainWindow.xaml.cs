﻿using ProjectPente.Models;
using ProjectPente.PENTE_User_Controls;
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

namespace ProjectPente
{
     public enum Mode
    {
        PVP,
        PVC
    }



    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TitleScreen titleScreen = new TitleScreen();
        NameSelectUC nameSelect = new NameSelectUC();
        GameBoard gameBoard = new GameBoard();
        GameOverUC gameOver = new GameOverUC();
        GameController game;
        int row;
        int col;

        public MainWindow()
        {
            InitializeComponent();
            mainWindow.Children.Add(titleScreen);
            mainWindow.Children.Add(nameSelect);
            mainWindow.Children.Add(gameBoard);
            mainWindow.Children.Add(gameOver);
            nameSelect.Visibility = Visibility.Hidden;
            gameBoard.Visibility = Visibility.Hidden;
            gameOver.Visibility = Visibility.Hidden;
            titleScreen.window = this;
            nameSelect.window = this;
            gameBoard.window = this;
        }

        internal void MainMenu()
        {
            titleScreen.Visibility = Visibility.Visible;
            nameSelect.Visibility = Visibility.Hidden;
            gameBoard.Visibility = Visibility.Hidden;
            gameOver.Visibility = Visibility.Hidden;
        }

        internal void StartGame()
        {
            titleScreen.Visibility = Visibility.Hidden;
            nameSelect.Visibility = Visibility.Visible;
        }

        internal void Go()
        {
            game = new GameController(nameSelect.tbxPlayer1Name.Text, nameSelect.tbxPlayer2Name.Text, Mode.PVP);
            gameBoard.ugPenteBoard.Children.Clear();
            row = gameBoard.ugPenteBoard.Rows;
            col = gameBoard.ugPenteBoard.Columns;
            for (int i = 0; i < row * col ; i++)
            {
                Tile t = new Tile();

                t.rectangle.MouseDown += PlacePiece;

                gameBoard.ugPenteBoard.Children.Add(t.rectangle);
            };
            nameSelect.Visibility = Visibility.Hidden;
            gameBoard.Visibility = Visibility.Visible;
        }

        private void PlacePiece(object sender, MouseButtonEventArgs e)
        {
            string color = game.CurrentPlayer.Name == game.player1.Name ? "WhiteStone" : "BLackStone"; 
            Rectangle position = (Rectangle)sender;
            ImageBrush image = new ImageBrush();
            image.ImageSource = new BitmapImage(new Uri($"Resources//{color}(Resize).Png", UriKind.Relative));
            position.Fill = image;
            if (game.ValidMove())
            {
                game.TogglePlayer();
            }
        }
    }
}
