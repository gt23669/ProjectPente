using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPente.Models
{
    public class Player
    {
        //Number of Captures a player has.
        public int Captures { get; set; }
        //Player's Name.
        public string Name { get; set; }
        //Player's Alerts.
        public string Alerts { get; set; }
        //Player's piece color.
        public string StoneColor { get; set; }
        //Denotes if a player instance is controlled by computer.
        public bool IsComputer { get; set; }
        //A list of any Trias a player has.
        public List<Tria> Trias { get; set; }
        //A list of any Tesseras a player has.
        public List<Tessera> Tesseras { get; set; } 

        //Constructor
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
