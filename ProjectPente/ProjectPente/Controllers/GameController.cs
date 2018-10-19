using ProjectPente.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPente
{
    public class GameController
    {
        public MainWindow window { get; set; }
        public Player player1;
        public Player player2;
        public Player CurrentPlayer { get; set; }
        public Tuple<int, int> CenterSpace { get; private set; }
        public Tile CurrentPiece { get; private set; }
        public List<Tile> AvailableTiles = new List<Tile>();
        private List<Tile> BlackPieces;
        private List<Tile> WhitePieces;
        private List<Tile> CurrentPieces;
        Mode Mode;
        private int turnCount;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name1">1st Players Name</param>
        /// <param name="name2">2nd Players Name</param>
        /// <param name="mode">Game Mode Selected</param>
        /// <param name="center">Position of Center Tile</param>
        /// <param name="window">Reference to Main Window</param>
        public GameController(string name1, string name2, Mode mode, Tuple<int, int> center, MainWindow window)
        {
            player1 = new Player(name1, false);
            bool IsComputerOpponent = mode == Mode.PVC ? true : false;
            player2 = new Player(name2, IsComputerOpponent);
            CurrentPlayer = player1;
            turnCount = 1;
            Mode = mode;
            CenterSpace = center;
            BlackPieces = new List<Tile>();
            WhitePieces = new List<Tile>();
            this.window = window;
        }

        //Checks if a move is valid per tournament rules
        internal bool ValidMove(Tile tile)
        {
            if (turnCount == 1 && tile.Position.Equals(CenterSpace))
            {
                return true;
            }
            else if (turnCount == 3 && outsideCenter(tile.Position, CenterSpace))
            {
                return true;
            }
            else if (turnCount != 1 && turnCount != 3)
            {
                return true;
            }

            return false;
        }

        //Helper method for ValidMove.
        private bool outsideCenter(Tuple<int, int> position, Tuple<int, int> centerSpace)
        {
            if (Math.Abs(position.Item1 - centerSpace.Item1) > 2 || Math.Abs(position.Item2 - centerSpace.Item2) > 2)
            {
                return true;
            }

            return false;
        }

        //Toggles the who the current player is.
        public void TogglePlayer()
        {
            string Alerts = CurrentPlayer.Alerts;
            CurrentPlayer = CurrentPlayer == player1 ? player2 : player1;
            turnCount++;
            window.turnTime = 21;
            if (Mode == Mode.PVC)
            {
                if (CurrentPlayer.IsComputer)
                {
                    foreach (Tile tile in AvailableTiles)
                    {
                        tile.rectangle.MouseDown -= tile.PlacePieceEvent;
                    }
                }
                else if (!CurrentPlayer.IsComputer)
                {
                    foreach (Tile tile in AvailableTiles)
                    {
                        tile.rectangle.MouseDown += tile.PlacePieceEvent;
                    }
                }
                window.UpdateView(CurrentPlayer.Name, Alerts);
            }
        }

        //Sets reference to the piece just placed for win and capture checks and adds it to a list of same colored pieces placed.
        internal void setCurrentPiece(Tile tile)
        {
            CurrentPiece = tile;
            if (tile.PieceColor == Piece.BLACK)
            {
                BlackPieces.Add(tile);
                CurrentPieces = BlackPieces;
            }
            else
            {
                WhitePieces.Add(tile);
                CurrentPieces = WhitePieces;
            }
            AvailableTiles.Remove(tile);
        }

        //Runs capture and win checks
        internal void runChecks()
        {
            tryCapture(CurrentPiece);
            checkWinConditions();
        }

        //Performs a capture based on last piece placed if possible.
        private void tryCapture(Tile currentPiece)
        {
            List<Tile> Capturing = new List<Tile>();
            List<Tile> Captured = new List<Tile>();

            switch (currentPiece.PieceColor)
            {
                case Piece.BLACK:
                    Capturing = BlackPieces;
                    Captured = WhitePieces;
                    break;
                case Piece.WHITE:
                    Capturing = WhitePieces;
                    Captured = BlackPieces;
                    break;
            }

            foreach (Tile tile in Capturing)
            {
                bool capture = false;
                Tile tile1 = null;
                Tile tile2 = null;
                Tuple<int, int> position1;
                Tuple<int, int> position2;

                if (currentPiece.Position.Item1 - tile.Position.Item1 == 3 && currentPiece.Position.Item2 - tile.Position.Item2 == 0)
                {
                    position1 = new Tuple<int, int>(currentPiece.Position.Item1 - 1, currentPiece.Position.Item2);
                    position2 = new Tuple<int, int>(currentPiece.Position.Item1 - 2, currentPiece.Position.Item2);
                    tile1 = GetPieceAtPosition(position1, Captured);
                    tile2 = GetPieceAtPosition(position2, Captured);
                    if (tile1 != null && tile2 != null)
                    {
                        capture = true;
                    }
                }
                else if (currentPiece.Position.Item1 - tile.Position.Item1 == -3 && currentPiece.Position.Item2 - tile.Position.Item2 == 0)
                {
                    position1 = new Tuple<int, int>(currentPiece.Position.Item1 + 1, currentPiece.Position.Item2);
                    position2 = new Tuple<int, int>(currentPiece.Position.Item1 + 2, currentPiece.Position.Item2);
                    tile1 = GetPieceAtPosition(position1, Captured);
                    tile2 = GetPieceAtPosition(position2, Captured);
                    if (tile1 != null && tile2 != null)
                    {
                        capture = true;
                    }
                }
                else if (currentPiece.Position.Item1 - tile.Position.Item1 == 0 && currentPiece.Position.Item2 - tile.Position.Item2 == 3)
                {
                    position1 = new Tuple<int, int>(currentPiece.Position.Item1, currentPiece.Position.Item2 - 1);
                    position2 = new Tuple<int, int>(currentPiece.Position.Item1, currentPiece.Position.Item2 - 2);
                    tile1 = GetPieceAtPosition(position1, Captured);
                    tile2 = GetPieceAtPosition(position2, Captured);
                    if (tile1 != null && tile2 != null)
                    {
                        capture = true;
                    }
                }
                else if (currentPiece.Position.Item1 - tile.Position.Item1 == 0 && currentPiece.Position.Item2 - tile.Position.Item2 == -3)
                {
                    position1 = new Tuple<int, int>(currentPiece.Position.Item1, currentPiece.Position.Item2 + 1);
                    position2 = new Tuple<int, int>(currentPiece.Position.Item1, currentPiece.Position.Item2 + 2);
                    tile1 = GetPieceAtPosition(position1, Captured);
                    tile2 = GetPieceAtPosition(position2, Captured);
                    if (tile1 != null && tile2 != null)
                    {
                        capture = true;
                    }
                }
                else if (currentPiece.Position.Item1 - tile.Position.Item1 == 3 && currentPiece.Position.Item2 - tile.Position.Item2 == 3)
                {
                    position1 = new Tuple<int, int>(currentPiece.Position.Item1 - 1, currentPiece.Position.Item2 - 1);
                    position2 = new Tuple<int, int>(currentPiece.Position.Item1 - 2, currentPiece.Position.Item2 - 2);
                    tile1 = GetPieceAtPosition(position1, Captured);
                    tile2 = GetPieceAtPosition(position2, Captured);
                    if (tile1 != null && tile2 != null)
                    {
                        capture = true;
                    }
                }
                else if (currentPiece.Position.Item1 - tile.Position.Item1 == -3 && currentPiece.Position.Item2 - tile.Position.Item2 == 3)
                {
                    position1 = new Tuple<int, int>(currentPiece.Position.Item1 + 1, currentPiece.Position.Item2 - 1);
                    position2 = new Tuple<int, int>(currentPiece.Position.Item1 + 2, currentPiece.Position.Item2 - 2);
                    tile1 = GetPieceAtPosition(position1, Captured);
                    tile2 = GetPieceAtPosition(position2, Captured);
                    if (tile1 != null && tile2 != null)
                    {
                        capture = true;
                    }
                }
                else if (currentPiece.Position.Item1 - tile.Position.Item1 == 3 && currentPiece.Position.Item2 - tile.Position.Item2 == -3)
                {
                    position1 = new Tuple<int, int>(currentPiece.Position.Item1 - 1, currentPiece.Position.Item2 + 1);
                    position2 = new Tuple<int, int>(currentPiece.Position.Item1 - 2, currentPiece.Position.Item2 + 2);
                    tile1 = GetPieceAtPosition(position1, Captured);
                    tile2 = GetPieceAtPosition(position2, Captured);
                    if (tile1 != null && tile2 != null)
                    {
                        capture = true;
                    }
                }
                else if (currentPiece.Position.Item1 - tile.Position.Item1 == -3 && currentPiece.Position.Item2 - tile.Position.Item2 == -3)
                {
                    position1 = new Tuple<int, int>(currentPiece.Position.Item1 + 1, currentPiece.Position.Item2 + 1);
                    position2 = new Tuple<int, int>(currentPiece.Position.Item1 + 2, currentPiece.Position.Item2 + 2);
                    tile1 = GetPieceAtPosition(position1, Captured);
                    tile2 = GetPieceAtPosition(position2, Captured);
                    if (tile1 != null && tile2 != null)
                    {
                        capture = true;
                    }
                }

                if (capture)
                {
                    Captured.Remove(tile1);
                    Captured.Remove(tile2);
                    tile1.ResetPiece();
                    tile2.ResetPiece();
                    CurrentPlayer.Captures++;
                    AvailableTiles.Add(tile1);
                    AvailableTiles.Add(tile2);
                }
            }

            switch (currentPiece.PieceColor)
            {
                case Piece.BLACK:
                    BlackPieces = Capturing;
                    WhitePieces = Captured;
                    break;
                case Piece.WHITE:
                    WhitePieces = Capturing;
                    BlackPieces = Captured;
                    break;
            }
        }

        //Handles the logic for the computer's turn if there is a computer opponent.
        internal void ComputerTurn()
        {
            Random r1 = new Random();
            Random r2 = new Random();
            foreach (Tile item in WhitePieces)
            {
                for (int c = 0; c < 9; c++)
                {
                    int i = item.Position.Item1 + r1.Next(-1, 2);
                    int j = item.Position.Item1 + r2.Next(-1, 2);
                    foreach (Tile tile in AvailableTiles)
                    {
                        if (tile.Position.Item1 == i && tile.Position.Item2 == j && tile.PlacePiece())
                        {
                            return;
                        }
                    }
                }
            }

            Random random = new Random();
            int index = random.Next(0, AvailableTiles.Count);
            AvailableTiles[index].PlacePiece();
        }

        //Returns a piece at a given position.
        private Tile GetPieceAtPosition(Tuple<int, int> position, List<Tile> captured)
        {
            foreach (Tile tile in captured)
            {
                if (tile.Position.Equals(position))
                {
                    return tile;
                }
            }

            return null;
        }

        //Checks win conditions
        private void checkWinConditions()
        {
            if (CurrentPlayer.Captures >= 5)
            {
                window.GameOver(CurrentPlayer);
            }
            foreach (Tile tile in CurrentPieces)
            {
                Tuple<int, int> position = new Tuple<int, int>(tile.Position.Item1 - 1, tile.Position.Item2 - 1);
                int ConsecutivePieces = 1;
                while (GetPieceAtPosition(position, CurrentPieces) != null)
                {
                    position = new Tuple<int, int>(position.Item1 - 1, position.Item2 - 1);
                    ConsecutivePieces++;
                }
                if (ConsecutivePieces >= 3)
                {
                    CurrentPlayer.Alerts = $"{CurrentPlayer.Name} has a Tria!";
                    if (ConsecutivePieces >= 4)
                    {
                        CurrentPlayer.Alerts = $"{CurrentPlayer.Name} has a Tessera!";
                        if (ConsecutivePieces >= 5)
                        {
                            window.GameOver(CurrentPlayer);
                            return;
                        }
                    }
                }
                else
                {
                    position = new Tuple<int, int>(tile.Position.Item1 - 1, tile.Position.Item2 + 1);
                    ConsecutivePieces = 1;
                }
                while (GetPieceAtPosition(position, CurrentPieces) != null)
                {
                    position = new Tuple<int, int>(position.Item1 - 1, position.Item2 + 1);
                    ConsecutivePieces++;
                }
                if (ConsecutivePieces >= 3)
                {
                    CurrentPlayer.Alerts = $"{CurrentPlayer.Name} has a Tria!";
                    if (ConsecutivePieces >= 4)
                    {
                        CurrentPlayer.Alerts = $"{CurrentPlayer.Name} has a Tessera!";
                        if (ConsecutivePieces >= 5)
                        {
                            window.GameOver(CurrentPlayer);
                            return;
                        }
                    }
                }
                else
                {
                    position = new Tuple<int, int>(tile.Position.Item1, tile.Position.Item2 - 1);
                    ConsecutivePieces = 1;
                }

                while (GetPieceAtPosition(position, CurrentPieces) != null)
                {
                    position = new Tuple<int, int>(position.Item1, position.Item2 - 1);
                    ConsecutivePieces++;
                }
                if (ConsecutivePieces >= 3)
                {
                    CurrentPlayer.Alerts = $"{CurrentPlayer.Name} has a Tria!";
                    if (ConsecutivePieces >= 4)
                    {
                        CurrentPlayer.Alerts = $"{CurrentPlayer.Name} has a Tessera!";
                        if (ConsecutivePieces >= 5)
                        {
                            window.GameOver(CurrentPlayer);
                            return;
                        }
                    }
                }
                else
                {
                    position = new Tuple<int, int>(tile.Position.Item1 - 1, tile.Position.Item2);
                    ConsecutivePieces = 1;
                }

                while (GetPieceAtPosition(position, CurrentPieces) != null)
                {
                    position = new Tuple<int, int>(position.Item1 - 1, position.Item2);
                    ConsecutivePieces++;
                }
                if (ConsecutivePieces >= 3)
                {
                    CurrentPlayer.Alerts = $"{CurrentPlayer.Name} has a Tria!";
                    if (ConsecutivePieces >= 4)
                    {
                        CurrentPlayer.Alerts = $"{CurrentPlayer.Name} has a Tessera!";
                        if (ConsecutivePieces >= 5)
                        {
                            window.GameOver(CurrentPlayer);
                            return;
                        }
                    }
                }
            }
        }
    }
}
