using System.Collections.Generic;
using UnityEngine;

namespace Source.Maze
{
    public sealed class MazePath
    {
        public MazePath(Vector3 offset, MazeCellData[,] cells, MazeCellData finishCell)
        {
            Positions = CalculatePath(offset, cells, finishCell);
        }

        public static implicit operator List<Vector3>(MazePath mazePath) => mazePath.Positions;
        public static implicit operator Vector3[](MazePath mazePath) => mazePath.Positions.ToArray();

        public Vector3 this[int index] => Positions[index]; 

        public List<Vector3> Positions { get; }

        public int Count => Positions.Count;

        private static List<Vector3> CalculatePath(Vector3 offset, MazeCellData[,] cells, MazeCellData finishCell)
        {
            var currentPosition = new Vector2Int(finishCell.X, finishCell.Y);
            var positions = new List<Vector3>();

            while (currentPosition != Vector2Int.zero)
            {
                int x = currentPosition.x;
                int y = currentPosition.y;
                MazeCellData currentCell = cells[x, y];

                positions.Add(new Vector3(x + offset.x, offset.y, y + offset.z));

                if (x > 0 && !currentCell.WallLeftEnabled && cells[x - 1, y].Distance.Equals(currentCell.Distance - 1))
                    currentPosition.x--;
                else if (y > 0 && !currentCell.WallBottomEnabled && cells[x, y - 1].Distance.Equals(currentCell.Distance - 1))
                    currentPosition.y--;
                else if (x < cells.GetLength(0) - 1 && !cells[x + 1, y].WallLeftEnabled &&
                         cells[x + 1, y].Distance.Equals(currentCell.Distance - 1))
                    currentPosition.x++;
                else if (y < cells.GetLength(1) - 1 && !cells[x, y + 1].WallBottomEnabled &&
                         cells[x, y + 1].Distance.Equals(currentCell.Distance - 1))
                    currentPosition.y++;
            }
            
            positions.Add(new Vector3(offset.x, offset.y, offset.z));
            positions.Reverse();
            return positions;
        }
    }
}