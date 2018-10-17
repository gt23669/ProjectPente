using ProjectPente.Models;
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

        public Tuple<int, int> BoardCenter { get; private set; }

        GameController game;
        int row;
        int col;

        //Constructor: Initializes windows
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
            gameOver.window = this;
        }
        //Returns user to Main Menu screen.
        internal void MainMenu()
        {
            titleScreen.Visibility = Visibility.Visible;
            nameSelect.Visibility = Visibility.Hidden;
            gameBoard.Visibility = Visibility.Hidden;
            gameOver.Visibility = Visibility.Hidden;
        }
        //Takes User to Name Select Screen
        internal void StartGame()
        {
            titleScreen.Visibility = Visibility.Hidden;
            nameSelect.Visibility = Visibility.Visible;
        }
        //Generates game with parameter and takes user to game screen
        internal void Go()
        {
            gameOver.Visibility = Visibility.Hidden;
            gameBoard.ugPenteBoard.Children.Clear();
            int size = (int) nameSelect.sGrid.Value;
            gameBoard.ugPenteBoard.Rows = size;
            gameBoard.ugPenteBoard.Columns = size;
            row = size;
            col = size;
            string player1 = nameSelect.tbxPlayer1Name.Text;
            string player2 = nameSelect.tbxPlayer2Name.Text;
            BoardCenter = new Tuple<int, int>((row - 1) / 2, (col - 1) / 2);
            game = new GameController(player1, player2, Mode.PVP, BoardCenter, this);
            ImageBrush imageStandard = new ImageBrush();
            imageStandard.ImageSource = new BitmapImage(new Uri($"Resources//PenteBoardBackground.png", UriKind.Relative));
            ImageBrush imageCenter = new ImageBrush();
            imageCenter.ImageSource = new BitmapImage(new Uri($"Resources//PenteBoardBackgroundCenter.png", UriKind.Relative));
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    Tile t = new Tile(j, i);
                    t.Game = game;
                    Rectangle rectangle = new Rectangle()
                    {
                        Fill = imageStandard
                    };

                    if (i == BoardCenter.Item1 && j == BoardCenter.Item2)
                    {
                        rectangle.Fill = imageCenter;
                    }


                    rectangle.MouseDown += t.PlacePiece;
                    

                    t.rectangle = rectangle;

                    gameBoard.ugPenteBoard.Children.Add(t.rectangle);
                }
            };
            nameSelect.Visibility = Visibility.Hidden;
            gameBoard.Visibility = Visibility.Visible;
        }

        internal void GameOver(Player currentPlayer)
        {
            gameBoard.Visibility = Visibility.Hidden;
            gameOver.Visibility = Visibility.Visible;
            gameOver.lb_winner.Content = $"{currentPlayer.Name} wins!";
        }
    }
}
