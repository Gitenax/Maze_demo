using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Maze
{
    public sealed class MazeObject
    {
        private readonly MazeGenerator _mazeGenerator = new MazeGenerator();

        public event Action OnMazeGenerated;
        
        public int Width => _mazeGenerator.Width;
        public int Height => _mazeGenerator.Height;

        public MazeCellData[,] Cells { get; private set; }

        public MazePath ShorcutPath { get; private set; }
        
        public MazeCellData EntryCell => _mazeGenerator[0, 0];
        public MazeCellData FinishCell => _mazeGenerator.FinishCell;

        public bool IsEmpty => Cells == null || Cells.Length == 0;
        
        public void Generate(int width, int height, Vector3 offset)
        {
            Cells = _mazeGenerator.Generate(width, height);
            ShorcutPath = new MazePath(offset, Cells, FinishCell);
            OnMazeGenerated?.Invoke();
        }

        public void Clear()
        {
            if(Cells == null || Cells.Length == 0)
                return;
            
            Array.Clear(Cells, 0, Cells.GetLength(0) * Cells.GetLength(1));
        }
        
        private sealed class MazeGenerator
        {
            private MazeCellData[,] _mazeCells; 
            
            public int Width { get; private set;}
            public int Height { get; private set;}
            
            public MazeCellData EntryCell { get; private set; }
            public MazeCellData FinishCell { get; private set; }

            public MazeCellData this[int x, int y] => _mazeCells[x, y];

            public MazeCellData[,] Generate(int width, int height)
            {
                (Width, Height) = (width, height);
                _mazeCells = new MazeCellData[Width, Height];

                for (int x = 0; x < _mazeCells.GetLength(0); x++)
                {
                    for (var y = 0; y < _mazeCells.GetLength(1); y++)
                        _mazeCells[x, y] = new MazeCellData(x, y);
                }
                
                // Удаление последних вертикальных стен и пола
                for (var x = 0; x < _mazeCells.GetLength(0); x++)
                {
                    _mazeCells[x, Height - 1].WallLeftEnabled = false;
                    _mazeCells[x, Height - 1].FloorEnabled = false;
                }

                // Удаление крайних горизонтальных стен и пола
                for (var y = 0; y < _mazeCells.GetLength(1); y++)
                {
                    _mazeCells[Width - 1, y].WallBottomEnabled = false;
                    _mazeCells[Width - 1, y].FloorEnabled = false;
                }

                GenetateWallsWithBacktracking(_mazeCells);
                FinishCell = SetMazeExit(_mazeCells);

                return _mazeCells;
            }

            private static void RemoveWall(MazeCellData current, MazeCellData nextCell)
            {
                if (current.X == nextCell.X)
                {
                    if (current.Y > nextCell.Y) 
                        current.WallBottomEnabled = false;
                    else 
                        nextCell.WallBottomEnabled = false;
                }
                else
                {
                    if (current.X > nextCell.X) 
                        current.WallLeftEnabled  = false;
                    else 
                        nextCell.WallLeftEnabled = false;
                }
            }

            private void GenetateWallsWithBacktracking(MazeCellData[,] cells)
            {
                MazeCellData current = cells[0, 0];
                current.Visited = true;

                var stack = new Stack<MazeCellData>();

                do
                {
                    var unvisitedNeighbours = new List<MazeCellData>();
                    int x = current.X;
                    int y = current.Y;
              
                   if(x > 0 && !cells[x - 1, y].Visited)
                        unvisitedNeighbours.Add(cells[x - 1, y]);
                   
                   if(y > 0 && !cells[x, y - 1].Visited)
                        unvisitedNeighbours.Add(cells[x, y - 1]);
                   
                   if(x < Width - 2 && !cells[x + 1, y].Visited)
                        unvisitedNeighbours.Add(cells[x + 1, y]);
                   
                   if(y < Height - 2 && !cells[x, y + 1].Visited)
                        unvisitedNeighbours.Add(cells[x, y + 1]);
                   
                    if (unvisitedNeighbours.Count > 0)
                    {
                        MazeCellData nextCell = unvisitedNeighbours[UnityEngine.Random.Range(0, unvisitedNeighbours.Count)];
                        RemoveWall(current, nextCell);

                        nextCell.Visited = true;
                        stack.Push(nextCell);
                        nextCell.Distance = current.Distance + 1;
                        current = nextCell;
                    }
                    else
                    {
                        current = stack.Pop();
                    }
                    
                } while (stack.Count > 0);
            }

            private MazeCellData SetMazeExit(MazeCellData[,] mazeCells)
            {
                MazeCellData furtherCell = mazeCells[0, 0];
                
                for (int x = 0; x < mazeCells.GetLength(0); x++)
                {
                    if (mazeCells[x, Height - 2].Distance > furtherCell.Distance)
                        furtherCell = mazeCells[x, Height - 2];
                    
                    if(mazeCells[x, 0].Distance > furtherCell.Distance)
                        furtherCell = mazeCells[x, 0];
                }

                for (var y = 0; y < mazeCells.GetLength(1); y++)
                {
                    if (mazeCells[Width - 2, y].Distance > furtherCell.Distance)
                        furtherCell = mazeCells[Width - 2, y];
                    
                    if (mazeCells[0, y].Distance > furtherCell.Distance)
                        furtherCell = mazeCells[0, y];
                }

                int fx = furtherCell.X;
                
                if (fx == 0)
                {
                    furtherCell.WallLeftEnabled = false;
                }
                else
                {
                    int fy = furtherCell.Y;
                    
                    if (fy == 0)
                        furtherCell.WallBottomEnabled = false;
                    else if (fx == Width - 2)
                        mazeCells[fx + 1, fy].WallLeftEnabled = false;
                    else if (fy == Height - 2)
                        mazeCells[fx, fy + 1].WallBottomEnabled = false;
                }

                return furtherCell;
            }
        }
    }
}