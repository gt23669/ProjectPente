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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TitleScreen titleScreen = new TitleScreen();
        NameSelectUC nameSelect = new NameSelectUC();
        GameBoard gameBoard = new GameBoard();
        GameOverUC gameOver = new GameOverUC();


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
            gameBoard.ugPenteBoard.Children.Clear();
            for (int i = 0; i < 361; i++)
            {
                Rectangle rectangle = new Rectangle()
                {
                    Stroke = Brushes.Black
                };

                rectangle.MouseDown += PlacePiece;

                gameBoard.ugPenteBoard.Children.Add(rectangle);
            };
            nameSelect.Visibility = Visibility.Hidden;
            gameBoard.Visibility = Visibility.Visible;
        }

        private void PlacePiece(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
}
