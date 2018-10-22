using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPente.Models
{
    public class Tessera
    {
        public Tuple<int, int> StartingPoint { get; set; }
        public Tuple<int, int> Direction { get; set; }


        public override bool Equals(object obj)
        {
            Tessera tessera = (Tessera)obj;
            if (tessera != null)
            {
                return tessera.StartingPoint.Equals(this.StartingPoint) && tessera.Direction.Equals(this.Direction);
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
