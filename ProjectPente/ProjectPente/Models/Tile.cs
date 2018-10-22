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
    //The color of a piece on a tile.
    public enum Piece
    {
        EMPTY,
        BLACK,
        WHITE
    }

    public class Tile
    {
        //The rectangle of a tile that is sent to a view
        public Rectangle Rectangle { get; set; }
        //The position of the tile instance.
        public Tuple<int, int> Position { get; set; }
        //The color of the piece associated with the tile
        public Piece PieceColor { get; set; }
        //Denotes if a tile has been taken by a player
        public bool IsTaken { get; set; }

        //Sets the fill of the rectangle to reflect the piece placed there.
        internal void PlacePiece(string color)
        {
                IsTaken = true;
                PieceColor = color == "BlackStone" ? Piece.BLACK : Piece.WHITE;
                ImageBrush image = new ImageBrush();
                image.ImageSource = new BitmapImage(new Uri($"Resources//{color}(Resize).Png", UriKind.Relative));
                Rectangle.Fill = image;

        }
        //Resets tile properties to default when a piece is removed.
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


