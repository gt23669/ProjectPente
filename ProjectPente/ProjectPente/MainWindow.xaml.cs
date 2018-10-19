using ProjectPente.Models;
using ProjectPente.PENTE_User_Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
        Timer timer;
        public int turnTime;
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
            if (timer != null)
            {
                timer.Stop();
            }
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
            string player1 = nameSelect.tbxPlayer1Name.Text;
            string player2 = nameSelect.tbxPlayer2Name.Text;
            Mode mode = nameSelect.chkBoxComputer.IsChecked == true ? Mode.PVC : Mode.PVP;
            game = new GameController(player1, player2, mode, this);
            int size = (int)nameSelect.sGrid.Value;

            TimerSetup();
            game.GenerateTiles(size);
            FillBoard(size);
            UpdateView(nameSelect.tbxPlayer1Name.Text, null);
            ShowGameBoard();
        }

        private void TimerSetup()
        {
            if (timer != null)
            {
                timer.Stop();
            }

            turnTime = 20;
            gameBoard.lbTimer.Content = $"{turnTime}s";

            timer = new Timer
            {
                Interval = 1000
            };

            timer.Elapsed += CountDown;
            timer.Start();
        }

        private void FillBoard(int size)
        {
            gameBoard.ugPenteBoard.Children.Clear();
            gameBoard.ugPenteBoard.Rows = size;
            gameBoard.ugPenteBoard.Columns = size;
            foreach (Tile tile in game.GetTiles())
            {
                gameBoard.ugPenteBoard.Children.Add(tile.Rectangle);
            }
        }

        private void ShowGameBoard()
        {
            gameOver.Visibility = Visibility.Hidden;
            nameSelect.Visibility = Visibility.Hidden;
            gameBoard.Visibility = Visibility.Visible;
        }

        //Displays current player's name and shows any messages.
        internal void UpdateView(string name, string alerts)
        {
            gameBoard.lbPlayerLabel.Content = $"{name}'s Turn";
            gameBoard.lbAlert.Content = alerts;
        }

        //Logic for turn timer. Counts down from 20 and switches players at 0.
        private void CountDown(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                Random random = new Random();
                //int offset = random.Next(0, 5);
                int offset = 0;
                turnTime--;
                gameBoard.lbTimer.Content = $"{turnTime}s";

                if (game.CurrentPlayer.IsComputer && turnTime < 20 - offset)
                {
                    game.ComputerTurn();
                }

                if (turnTime <= 0)
                {
                    game.Changeplayer();
                }
            });
        }

        //Displays the winner when the game ends.
        internal void GameOver(Player currentPlayer)
        {
            gameBoard.Visibility = Visibility.Hidden;
            gameOver.Visibility = Visibility.Visible;
            gameOver.lb_winner.Content = $"{currentPlayer.Name} wins!";
            timer.Stop();
        }
    }
}
