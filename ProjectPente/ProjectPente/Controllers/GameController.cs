using ProjectPente.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPente
{
    class GameController
    {
        public Player player1;
        public Player player2;
        public Player CurrentPlayer { get; set; }
        Mode Mode;
       
        
        public GameController(string name1, string name2, Mode mode)
        {
            player1 = new Player(name1);
            player2 = new Player(name2);
            CurrentPlayer = player1;
            Mode = mode;
        }

        internal bool ValidMove()
        {
            return true;    
        }

        internal void TogglePlayer()
        {
            CurrentPlayer = CurrentPlayer == player1 ? player2 : player1;
        }
    }
}
