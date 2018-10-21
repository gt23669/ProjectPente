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
    }
}
