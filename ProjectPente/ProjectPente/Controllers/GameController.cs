using ProjectPente.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjectPente
{
    public class GameController
    {
        public MainWindow window { get; set; }
        public Player player1;
        public Player player2;
        private readonly string BLACKSTONE = "BlackStone";
        private readonly string WHITESTONE = "WhiteStone";
        public Player CurrentPlayer { get; set; }
        public Tuple<int, int> CenterSpace { get; private set; }
        public Tile CurrentPiece { get; private set; }
        public List<Tile> AvailableTiles = new List<Tile>();
        private List<Tile> BlackPieces;
        private List<Tile> WhitePieces;
        private List<Tile> CurrentPieces;
        Dictionary<Rectangle, Tile> TileLookup = new Dictionary<Rectangle, Tile>();
        List<Tile> AllTiles = new List<Tile>();
        Mode Mode;
        private int turnCount;
        Random r1 = new Random();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name1">1st Players Name</param>
        /// <param name="name2">2nd Players Name</param>
        /// <param name="mode">Game Mode Selected</param>
        /// <param name="center">Position of Center Tile</param>
        /// <param name="window">Reference to Main Window</param>
        public GameController(string name1, string name2, Mode mode, MainWindow window)
        {
            player1 = new Player(name1, false, BLACKSTONE);
            bool IsComputerOpponent = mode == Mode.PVC ? true : false;
            player2 = new Player(name2, IsComputerOpponent, WHITESTONE);
            CurrentPlayer = player1;
            turnCount = 1;
            Mode = mode;
            BlackPieces = new List<Tile>();
            WhitePieces = new List<Tile>();
            this.window = window;
        }

        //Generates all the game board tiles and adds them to a list.
        public void GenerateTiles(int size)
        {
            ImageBrush imageStandard = new ImageBrush();
            imageStandard.ImageSource = new BitmapImage(new Uri($"Resources//PenteBoardBackground.png", UriKind.Relative));
            ImageBrush imageCenter = new ImageBrush();
            imageCenter.ImageSource = new BitmapImage(new Uri($"Resources//PenteBoardBackgroundCenter.png", UriKind.Relative));
            CenterSpace = new Tuple<int, int>((size - 1) / 2, (size - 1) / 2);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Tile t = new Tile();
                    t.Rectangle = new Rectangle();
                    t.Position = new Tuple<int, int>(i, j);
                    t.Rectangle.Fill = imageStandard;
                    t.Rectangle.MouseDown += SelectTileEvent;
                    TileLookup.Add(t.Rectangle, t);
                    AllTiles.Add(t);
                    AvailableTiles.Add(t);
                    if (i == CenterSpace.Item1 && j == CenterSpace.Item2)
                    {
                        t.Rectangle.Fill = imageCenter;
                    }
                }
            }
        }
        //Returns all tiles to the view.
        public List<Tile> GetTiles()
        {
            return AllTiles;
        }



        //Event handler for tile clicks
        private void SelectTileEvent(object sender, MouseButtonEventArgs e)
        {
            SelectTile(sender);

        }
        //Attempts to place a oiece on a tile, returns false if invalid move.
        private bool SelectTile(object sender)
        {
            Rectangle r = (Rectangle)sender;
            Tile tile = TileLookup[r];
            if (ValidMove(tile))
            {
                tile.PlacePiece(CurrentPlayer.StoneColor);
                SetCurrentPiece(tile);
                RunChecks();
                AvailableTiles.Remove(tile);
                turnCount++;
                window.UpdateView(CurrentPlayer.Name, CurrentPlayer.Alerts);
                ChangePlayer();
                return true;
            }
            return false;
        }



        #region Turn Flow Logic
        //Sets reference to the piece just placed for win and capture checks and adds it to a list of same colored pieces placed.
        internal void SetCurrentPiece(Tile tile)
        {
            CurrentPiece = tile;
            if(tile.PieceColor == Piece.EMPTY)
            {
                throw new ArgumentException("Tile cannot have color of Piece.Empty");
            }
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
        internal void RunChecks()
        {
            TryCapture(CurrentPiece);
            VerifyTrias();
            CheckForTria();
            CheckForTessera();
            CheckForWin();
        }
        //Toggles the who the current player is.
        public void ChangePlayer()
        {
            CurrentPlayer.Alerts = "";
            CurrentPlayer = CurrentPlayer == player1 ? player2 : player1;
            window.turnTime = 21;
            if (Mode == Mode.PVC)
            {
                if (CurrentPlayer.IsComputer)
                {
                    foreach (Tile tile in AvailableTiles)
                    {
                        tile.Rectangle.MouseDown -= SelectTileEvent;
                    }
                }
                else if (!CurrentPlayer.IsComputer)
                {
                    foreach (Tile tile in AvailableTiles)
                    {
                        tile.Rectangle.MouseDown += SelectTileEvent;
                    }
                }
            }
        }
        //Handles the logic for the computer's turn if there is a computer opponent.
        internal void ComputerTurn()
        {
            if (turnCount == 1)
            {
                foreach (Tile tile in AvailableTiles)
                {
                    if (tile.Position.Equals(CenterSpace))
                    {
                        SelectTile(tile.Rectangle);
                        return;
                    }
                }
            }
            
            
            foreach (Tile item in WhitePieces)
            {
                for (int c = 0; c < 9; c++)
                {
                    int i = item.Position.Item1 + r1.Next(-1, 2);
                    int j = item.Position.Item2 + r1.Next(-1, 2);
                    foreach (Tile tile in AvailableTiles)
                    {
                        if (tile.Position.Item1 == i && tile.Position.Item2 == j && SelectTile(tile.Rectangle))
                        {
                            return;
                        }
                    }
                }
            }

            Random random = new Random();
            int index = random.Next(0, AvailableTiles.Count);
            SelectTile(AvailableTiles[index].Rectangle);
        }
        #endregion


        #region Checks
        //Performs a capture based on last piece placed if possible.
        private void TryCapture(Tile currentPiece)
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
        //Checks to see if a previously placed tria has been invalidated by the opponent.
        private void VerifyTrias()
        {
            Player player = CurrentPlayer == player1 ? player2 : player1;
            List<Tria> NotValid = new List<Tria>();
            foreach (Tria tria in player.Trias) 
            {
                Tuple<int, int> position1 = new Tuple<int, int>(tria.StartingPoint.Item1 - tria.Direction.Item1, tria.StartingPoint.Item2 - tria.Direction.Item2);
                Tuple<int, int> position2 = new Tuple<int, int>(tria.StartingPoint.Item1 + (3 * tria.Direction.Item1), tria.StartingPoint.Item2 + (3 *tria.Direction.Item2));
                Tile t1 = GetPieceAtPosition(position1, AllTiles);
                Tile t2 = GetPieceAtPosition(position2, AllTiles);

                bool condition1 = (t1 == null || t1.PieceColor == Piece.EMPTY);
                bool condition2 = (t2 == null || t2.PieceColor == Piece.EMPTY);


                if(!condition1 || !condition2)
                {
                    NotValid.Add(tria);
                }

            }

            foreach (Tria tria in NotValid)
            {
                player.Trias.Remove(tria);
            }


        }
        //Checks for any Trias
        private void CheckForTria()
        {
            foreach (Tile tile in CurrentPieces)
            {
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 1; j++)
                    {
                        if (i != 0 || j != 0)
                        {
                            Tuple<int, int> position = new Tuple<int, int>(tile.Position.Item1 + i, tile.Position.Item2 + j); 
                            int ConsecutivePieces = 1;


                            while (GetPieceAtPosition(position, CurrentPieces) != null)
                            {
                                position = new Tuple<int, int>(position.Item1 + i, position.Item2 + j);
                                ConsecutivePieces++;
                            }
                            if (ConsecutivePieces == 3)
                            {
                                Tuple<int, int> position1 = new Tuple<int, int>(tile.Position.Item1 - i, tile.Position.Item2 - j);
                                Tuple<int, int> position2 = new Tuple<int, int>(tile.Position.Item1 + (3*i), tile.Position.Item2 + (3*j));
                                Tile t1 = GetPieceAtPosition(position1, AllTiles);
                                Tile t2 = GetPieceAtPosition(position2, AllTiles);
                                bool condition1 = (t1 != null && (!t1.IsTaken || t1.PieceColor == CurrentPiece.PieceColor));
                                bool condition2 = (t2 != null && (!t2.IsTaken || t2.PieceColor == CurrentPiece.PieceColor));


                                if (condition1 && condition2)
                                {
                                    Tria tria = new Tria()
                                    {
                                        StartingPoint = tile.Position,
                                        Direction = new Tuple<int, int>(i, j)
                                    };
                                    
                                    CurrentPlayer.Alerts = $"{CurrentPlayer.Name} has a Tria!";
                                    foreach (Tria t in CurrentPlayer.Trias)
                                    {
                                        if (t.Equals(tria))
                                        {
                                            tria = null;
                                        }
                                    }

                                    if (tria != null)
                                    {
                                        CurrentPlayer.Trias.Add(tria);
                                    }
                                }

                            }
                            if(ConsecutivePieces == 5)
                            {
                                window.GameOver(CurrentPlayer);
                            }
                        }
                    }

                }
            }
        }
        //Checks for any Tesseras
        private void CheckForTessera()
        {
            Piece color = CurrentPiece.PieceColor;
            Tessera tessera = null;
            foreach (Tria t in CurrentPlayer.Trias)
            {
                Tuple<int, int> position1 = new Tuple<int, int>(t.StartingPoint.Item1 - t.Direction.Item1, t.StartingPoint.Item2 - t.Direction.Item2);
                Tuple<int, int> position2 = new Tuple<int, int>(t.StartingPoint.Item1 + (3 * t.Direction.Item1), t.StartingPoint.Item2 + (3 * t.Direction.Item2));
                Tuple<int, int> position3 = new Tuple<int, int>(t.StartingPoint.Item1 - (2 * t.Direction.Item1), t.StartingPoint.Item2 - (2 * t.Direction.Item2));
                Tuple<int, int> position4 = new Tuple<int, int>(t.StartingPoint.Item1 + (4 * t.Direction.Item1), t.StartingPoint.Item2 + (4 * t.Direction.Item2));
                Tile t1 = GetPieceAtPosition(position1, CurrentPieces);
                Tile t2 = GetPieceAtPosition(position2, CurrentPieces);
                Tile t3 = GetPieceAtPosition(position3, CurrentPieces);
                Tile t4 = GetPieceAtPosition(position4, CurrentPieces);
            

                if (t1 != null && t1.PieceColor == color && t3 == null && t4 == null)
                {
                    tessera = new Tessera()
                    {
                        StartingPoint = position1,
                        Direction = t.Direction

                    };
                    
                } else if(t2 != null && t2.PieceColor == color && t3 == null && t4 == null)
                {
                    tessera = new Tessera()
                    {
                        StartingPoint = position2,
                        Direction = new Tuple<int, int>(-t.Direction.Item1, -t.Direction.Item2)
                    };
                    
                }

                foreach (Tessera tess in CurrentPlayer.Tesseras)
                {
                    if (tess.Equals(tessera))
                    {
                        tessera = null;
                    }
                }

                if (tessera != null)
                {
                    CurrentPlayer.Alerts = $"{CurrentPlayer.Name} has a Tessera!";
                    CurrentPlayer.Tesseras.Add(tessera);
                }


            }
        }
        //Checks win conditions
        private void CheckForWin()
        {
            if (CurrentPlayer.Captures >= 5)
            {
                window.GameOver(CurrentPlayer);
            }

            Piece color = CurrentPiece.PieceColor;

            foreach (Tessera t in CurrentPlayer.Tesseras)
            {
                Tuple<int, int> position1 = new Tuple<int, int>(t.StartingPoint.Item1 - t.Direction.Item1, t.StartingPoint.Item2 - t.Direction.Item2);
                Tuple<int, int> position2 = new Tuple<int, int>(t.StartingPoint.Item1 + (4 * t.Direction.Item1), t.StartingPoint.Item2 + ( 4 * t.Direction.Item2));
                Tile t1 = GetPieceAtPosition(position1, CurrentPieces);
                Tile t2 = GetPieceAtPosition(position2, CurrentPieces);

                bool condition1 = (t1 != null && t1.PieceColor == color);
                bool condition2 = (t2 != null && t2.PieceColor == color);
                if (condition1 || condition2)
                {
                    window.GameOver(CurrentPlayer);
                    return;
                }
            }
        }
        #endregion


        #region Helper methods
        //Checks if a move is valid per tournament rules
        internal bool ValidMove(Tile tile)
        {
            if (tile.IsTaken)
            {
                return false;
            }
            if (turnCount == 1 && tile.Position.Equals(CenterSpace))
            {
                return true;
            }
            else if (turnCount == 3 && OutsideCenter(tile.Position, CenterSpace))
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
        public bool OutsideCenter(Tuple<int, int> position, Tuple<int, int> centerSpace)
        {
            if (Math.Abs(position.Item1 - centerSpace.Item1) > 2 || Math.Abs(position.Item2 - centerSpace.Item2) > 2)
            {
                return true;
            }

            return false;
        }
        //Returns a piece at a given position.
        private Tile GetPieceAtPosition(Tuple<int, int> position, List<Tile> pieces)
        {
            foreach (Tile tile in pieces)
            {
                if (tile.Position.Equals(position))
                {
                    return tile;
                }
            }

            return null;
        }
        #endregion



        
    }
}
