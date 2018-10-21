using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPente.Models
{
    public class Tria
    {
        public Tuple<int, int> StartingPoint { get; set; }
        public Tuple<int, int> Direction{ get; set; }

        public override bool Equals(object obj)
        {
            Tria t = (Tria)obj;

            return t.StartingPoint == this.StartingPoint && t.Direction == this.Direction;
        }
    }

    
}
