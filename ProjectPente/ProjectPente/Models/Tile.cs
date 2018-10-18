using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjectPente.Models
{

    public enum Piece
    {
        BLACK,
        WHITE,
        EMPTY
    }

    public class Tile : INotifyPropertyChanged
    {
        public Rectangle rectangle { get; set; }
        public Tuple<int, int> Position { get; set; }
        public int MyProperty { get; set; }

        public Piece PieceColor { get; set; }

        private bool Taken;
        public bool IsTaken
        {
            get { return Taken; }
            set
            {
                Taken = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsTaken"));
            }
        }

        public GameController Game { get; internal set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Tile(int x, int y)
        {
            Position = new Tuple<int, int>(x, y);
        }
        public void PlacePiece(object sender, MouseButtonEventArgs e)
        {
            if (!Taken && Game.ValidMove(this))
            {
                string color = Game.CurrentPlayer.Name == Game.player1.Name ? "BlackStone" : "WhiteStone";
                PieceColor = color == "BlackStone" ? Piece.BLACK : Piece.WHITE;
                ImageBrush image = new ImageBrush();
                image.ImageSource = new BitmapImage(new Uri($"Resources//{color}(Resize).Png", UriKind.Relative));
                rectangle.Fill = image;
                Game.setCurrentPiece(this);
                Game.runChecks();
                Game.TogglePlayer();
                Taken = true;
            }
        }


        internal void ResetPiece()
        {
            IsTaken = false;
            PieceColor = Piece.EMPTY;
            ImageBrush image = new ImageBrush();
            image.ImageSource = new BitmapImage(new Uri($"Resources//PenteBoardBackground.png", UriKind.Relative));
            rectangle.Fill = image;
        }
    }
}
