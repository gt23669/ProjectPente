using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ProjectPente.Models
{
    class Tile
    {
        public Rectangle rectangle { get; set; }
        public bool IsTaken { get; set; }

        public Tile()
        {
            Rectangle rectangle = new Rectangle()
            {
                Stroke = Brushes.Black,
                Fill = Brushes.White
            };
        }
    }
}
