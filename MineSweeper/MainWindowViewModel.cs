using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;

namespace MineSweeper
{
    public class MainWindowViewModel : BindableBase
    {
        #region Private Members

        private List<Tile> _tiles;
        private bool _gameOver = true;
        private bool minesPlaced;
        private string _startButtonContent;
        
        private int _rows = 9;
        private int _cols = 9;                
        private int _mines = 10;
        private int _minesLeft;

        private readonly DispatcherTimer _timer;
        private int _gameTime;

        #endregion

        #region Contructor

        public MainWindowViewModel()
        {
            RestartCommand = new DelegateCommand(Restart);
            UncoverTileCommand = new DelegateCommand<Tile>(UncoverTile);
            MarkTileCommand = new DelegateCommand<Tile>(MarkTile);
            DoubleClickCommand = new DelegateCommand<Tile>(DoubleClick);
           
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += timer_Tick;
            InitialiseGameTiles();
            MinesLeft = Mines;
            StartButtonContent = "Start";
        }

        #endregion

        #region Private Methods

        private void Restart()
        {
            _timer.Stop();
           
            if (StartButtonContent == "Stop")
            {
                GameOver = true;
                StartButtonContent = "Start";             
            }
            else
            {
                Tiles = null;
                GameOver = false;
                minesPlaced = false;
                MinesLeft = Mines;
                InitialiseGameTiles();
                GameTime = 0;
                StartButtonContent = "Stop";    
            }            
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            GameTime += 1;
        }

        private void InitialiseGameTiles()
        {          
            var tiles = new List<Tile>();
            var index = 0;

            for (var r = 0; r < Rows; r++)
            {
                for (var c = 0; c < Columns; c++)
                {                    
                    var tile = new Tile
                    {
                        Index = index,
                        Row = r,
                        Col = c
                    };
                    tiles.Add(tile);
                    index += 1;
                }                
            }
            Tiles = tiles;            
        }

        private void PlaceMines()
        {
            var MaxRandom = (Rows*Columns) + 1;
            for (var i = 0; i < Mines; ++i)
            {
                var r = new Random();
                var success = false;
                do
                {
                    var nextValue = r.Next(0, MaxRandom);
                    var selectedTile = Tiles.FirstOrDefault(x => x.Index == nextValue);

                    if (selectedTile == null || selectedTile.Covered == false || selectedTile.IsMine)
                        continue;

                    selectedTile.IsMine = true;
                    success = true;

                } while (success == false);
            }
            minesPlaced = true;
            CalculateMineCounts();
        }

        private void CalculateMineCounts()
        {
            foreach (var tile in Tiles.Where(tile => tile.IsMine == false))
            {
                tile.MineCount = CalculateMineCount(tile);
            }
        }

        private int CalculateMineCount(Tile tile)
        {            
            return GetTilesToCheck(tile).Count(tileToCheck => tileToCheck.IsMine);
        }

        private void MarkTile(Tile tile)
        {
            tile.Mark();
            MinesLeft = Mines - Tiles.Count(x => x.Status == "F");              
        }

        private void DoubleClick(Tile tile)
        {
            if (tile.MineCount == 0)
                return;

            var foundMarkedTiles = FindMarkedTiles(tile);
            if (foundMarkedTiles != tile.MineCount) return;

            CheckSurroundingTiles(tile, true);
        }

        private int FindMarkedTiles(Tile tile)
        {
            return GetTilesToCheck(tile).Count(tileToCheck => tileToCheck.Status == "F");
        }

        private void UncoverTile(Tile tile)
        {
            if (_timer.IsEnabled == false)
                _timer.Start();

            if (tile.Status == "")
                tile.Covered = false;
            else
                return;

            if (minesPlaced == false)
                PlaceMines();

            if (tile.IsMine)
            {
                tile.SelectedMine = true;
                EndGame(false);
                return;
            }

            CheckSurroundingTiles(tile, false);            
        }

        private void CheckSurroundingTiles(Tile tile, bool checkMines)
        {
            var tilesToCheck = GetTilesToCheck(tile);
            foreach (var tileToCheck in tilesToCheck)
            {
                if (!tileToCheck.IsMine)
                    tileToCheck.Covered = false;

                if (tileToCheck.IsMine && tileToCheck.Status != "F" && checkMines)
                {
                    EndGame(false);
                    return;
                }

                if (!tileToCheck.IsMine && tileToCheck.MineCount == 0)
                    CheckSurroundingTiles(tileToCheck, checkMines);
            }
            CheckGameStatus();
        }

        private IEnumerable<Tile> GetTilesToCheck(Tile tile)
        {
            var tilesToCheck = new List<Tile>();

            var leftTopTile = Tiles.FirstOrDefault(x => x.Row == tile.Row - 1 && x.Col == tile.Col - 1);
            var aboveTile = Tiles.FirstOrDefault(x => x.Row == tile.Row - 1 && x.Col == tile.Col);
            var rightTopTile = Tiles.FirstOrDefault(x => x.Row == tile.Row - 1 && x.Col == tile.Col + 1);
            var leftTile = Tiles.FirstOrDefault(x => x.Row == tile.Row && x.Col == tile.Col - 1);
            var rightTile = Tiles.FirstOrDefault(x => x.Row == tile.Row && x.Col == tile.Col + 1);
            var belowLeftTile = Tiles.FirstOrDefault(x => x.Row == tile.Row + 1 && x.Col == tile.Col - 1);
            var belowTile = Tiles.FirstOrDefault(x => x.Row == tile.Row + 1 && x.Col == tile.Col);
            var belowRightTile = Tiles.FirstOrDefault(x => x.Row == tile.Row + 1 && x.Col == tile.Col + 1);

            if (leftTopTile != null && leftTopTile.Covered) tilesToCheck.Add(leftTopTile);
            if (aboveTile != null && aboveTile.Covered) tilesToCheck.Add(aboveTile);
            if (rightTopTile !=null && rightTopTile.Covered) tilesToCheck.Add(rightTopTile);
            if (leftTile != null && leftTile.Covered) tilesToCheck.Add(leftTile);
            if (rightTile != null && rightTile.Covered) tilesToCheck.Add(rightTile);
            if (belowLeftTile != null && belowLeftTile.Covered) tilesToCheck.Add(belowLeftTile);
            if (belowTile != null && belowTile.Covered) tilesToCheck.Add(belowTile);
            if (belowRightTile != null && belowRightTile.Covered) tilesToCheck.Add(belowRightTile);

            return tilesToCheck;
        }

        private void CheckGameStatus()
        {
            if (!GameOver && !Tiles.Any(x => x.IsMine==false && x.Covered))
                EndGame(true);

            if (!GameOver && Tiles.All(x => x.IsMine && x.Covered))
                EndGame(true);
        }

        private void EndGame(bool won)
        {
            _timer.Stop();

            if (won)
            {
                foreach (var tile in Tiles)
                {
                    if (tile.IsMine)
                        tile.Status = "F";
                    else
                        tile.Covered = false;
                }
                MessageBox.Show("Yay!");
            }
            else
            {
                foreach (var tile in Tiles)
                {
                    if (tile.IsMine == false && tile.Status == "F")
                    {
                        tile.Status = "X";
                        tile.Covered = true;
                    }
                    else
                        tile.Covered = false;
                }
                MessageBox.Show("Boom!");
            }

            GameOver = true;
            StartButtonContent = "Start";
        }

        #endregion

        #region Public Properties

        public DelegateCommand RestartCommand { get; private set; }
        public DelegateCommand<Tile> UncoverTileCommand { get; private set; }
        public DelegateCommand<Tile> MarkTileCommand { get; private set; }
        public DelegateCommand<Tile> DoubleClickCommand { get; private set; }

        public List<Tile> Tiles
        {
            get { return _tiles; }
            set { SetProperty(ref _tiles, value); }
        }

        public string StartButtonContent
        {
            get {  return _startButtonContent; }
            set { SetProperty(ref _startButtonContent, value); }
        }

        public bool GameOver
        {
            get { return _gameOver; }
            set { SetProperty(ref _gameOver, value); }
        }

        public int GameTime
        {
            get { return _gameTime; }
            set { SetProperty(ref _gameTime, value); }
        }

        public int Rows
        {
            get { return _rows; }
            set
            {
                SetProperty(ref _rows, value);
                OnPropertyChanged("MaxMines");
                InitialiseGameTiles();
            }
        }

        public int Columns
        {
            get { return _cols; }
            set
            {
                SetProperty(ref _cols, value);
                OnPropertyChanged("MaxMines");
                InitialiseGameTiles();
            }
        }

        public int Mines
        {
            get { return _mines; }
            set
            {
                if (value > MaxMines) value=MaxMines;

                SetProperty(ref _mines, value);
                InitialiseGameTiles();
                MinesLeft=Mines;
            }
        }

        public int MaxMines
        {
            get { return ((Rows*Columns)-10); }
        }

        public int MinesLeft
        {
            get { return _minesLeft; }
            set { SetProperty(ref _minesLeft, value); }
        }

        #endregion
    }
}
