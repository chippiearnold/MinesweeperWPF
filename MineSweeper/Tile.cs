using System;
using Microsoft.Practices.Prism.Mvvm;

namespace MineSweeper
{
    public class Tile : BindableBase
    {
        private bool _covered=true;
        private bool _isMine;
        private bool _selectedMine;
        private int _mineCount;
        private int _index;
        private int _row;
        private int _col;
        private string _status = String.Empty;

        public void Mark()
        {
            switch (Status)
            {
                case "F":
                    Status = "?";
                    break;
                case "?":
                    Status = String.Empty;
                    break;
                default:
                    Status = "F";
                    break;
            }
        }

        public int Index
        {
            get { return _index; }
            set { SetProperty(ref _index, value); }
        }

        public int MineCount
        {
            get { return _mineCount; }
            set
            {
                SetProperty(ref _mineCount, value);
                OnPropertyChanged("DisplayMineCount");
            }
        }

        public string DisplayMineCount
        {
            get { return MineCount == 0 ? string.Empty : MineCount.ToString(); }
        }

        public bool Covered
        {
            get { return _covered; }
            set { SetProperty(ref _covered, value); }
        }

        public bool IsMine
        {
            get { return _isMine; }
            set { SetProperty(ref _isMine, value); }
        }

        public bool SelectedMine
        {
            get { return _selectedMine; }
            set { SetProperty(ref _selectedMine, value); }
        }
     
        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        public int Row
        {
            get { return _row; }
            set { SetProperty(ref _row, value); }
        }

        public int Col
        {
            get { return _col; }
            set { SetProperty(ref _col, value); }
        }

        public string Location
        {
            get { return string.Format("{0},{1}", Row, Col); }
        }

        public static int CellSize { get { return 23; } }

        public static int MineSize { get { return 20; } }
    }
}
