using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPente.Models
{
    public class Player
    {
        public int Captures { get; set; }
        public string Name { get; set; }

        public Player(string name)
        {
            Name = name;
            Captures = 0;
        }
    }
}
