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

        public string Alerts { get; set; }

        public string StoneColor { get; set; }

        public bool IsComputer { get; set; }

        public List<Tria> Trias { get; set; }

        public List<Tessera> Tesseras { get; set; } 


        public Player(string name, bool type, string color)
        {
            Name = name;
            Captures = 0;
            IsComputer = type;
            StoneColor = color;
            Trias = new List<Tria>();
            Tesseras = new List<Tessera>();
        }
    }
}
