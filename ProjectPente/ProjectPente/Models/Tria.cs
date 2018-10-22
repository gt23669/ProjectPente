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
            if (t != null)
            {
                return t.StartingPoint == this.StartingPoint && t.Direction == this.Direction;
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -1926835897;
            hashCode = hashCode * -1521134295 + EqualityComparer<Tuple<int, int>>.Default.GetHashCode(StartingPoint);
            hashCode = hashCode * -1521134295 + EqualityComparer<Tuple<int, int>>.Default.GetHashCode(Direction);
            return hashCode;
        }
    }
}
