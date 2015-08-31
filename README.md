# MinesweeperWPF
A version of Minesweeper built in WPF using Visual Studio 2013 Professional

I created this version of Minesweeper because it doesn't come with Windows 10 and I miss the old Windows XP version.
I wanted to keep it simple and quick to play and as faithful to the original as possible.

I couldn't be bothered creating graphics so for the moment everything uses ASII chars.

F - flagged square (right click to flag)
? - Unsure flag (second right click)
Numbers - as ever, show the mine count in surrounding squares.
M - For mine.

I might add some graphics at some point.

I use the WPF Extended toolkit for the integerUpDown controls for configuring Rows, Columns and No. Mines.
Microsoft Practices PRISM framework used primarily for INotifyPropertyChangeed implementation BindableBase.
These are both nuget packages which will hopefully restore by themselves.

Feel free to use this as you want!
