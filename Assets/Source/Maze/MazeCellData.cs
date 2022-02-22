namespace Source.Maze
{
    public sealed class MazeCellData
    {
        public MazeCellData(int x, int y) => (X, Y) = (x, y);

        public int X { get; }
        public int Y { get; }

        public bool WallLeftEnabled { get; set; } = true;
        public bool WallBottomEnabled { get; set; } = true;
        public bool FloorEnabled { get; set; } = true;

        public bool Visited { get; set; }
        
        public int Distance { get; set; }
    }
}