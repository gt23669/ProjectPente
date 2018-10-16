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
    class Tile : INotifyPropertyChanged
    {
        public Rectangle rectangle { get; set; }
        public int XPos { get; set; }
        public int YPos { get; set; }


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
            XPos = x;
            YPos = y;
        }
        public void PlacePiece(object sender, MouseButtonEventArgs e)
        {
            if (!Taken && Game.ValidMove())
            {
                string color = Game.CurrentPlayer.Name == Game.player1.Name ? "WhiteStone" : "BLackStone";
                Rectangle position = (Rectangle)sender;
                ImageBrush image = new ImageBrush();
                image.ImageSource = new BitmapImage(new Uri($"Resources//{color}(Resize).Png", UriKind.Relative));
                position.Fill = image;
                Game.TogglePlayer();
                Taken = true;
                
            }
        }



    }
}
