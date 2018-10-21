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
        EMPTY,
        BLACK,
        WHITE
    }

    public class Tile
    {
        public Rectangle Rectangle { get; set; }
        public Tuple<int, int> Position { get; set; }
        public Piece PieceColor { get; set; }
        public bool IsTaken { get; set; }


        internal void PlacePiece(string color)
        {
                IsTaken = true;
                PieceColor = color == "BlackStone" ? Piece.BLACK : Piece.WHITE;
                ImageBrush image = new ImageBrush();
                image.ImageSource = new BitmapImage(new Uri($"Resources//{color}(Resize).Png", UriKind.Relative));
                Rectangle.Fill = image;

        }

        internal void ResetPiece()
        {
            IsTaken = false;
            PieceColor = Piece.EMPTY;
            ImageBrush image = new ImageBrush();
            image.ImageSource = new BitmapImage(new Uri($"Resources//PenteBoardBackground.png", UriKind.Relative));
            Rectangle.Fill = image;
        }
    }
}


