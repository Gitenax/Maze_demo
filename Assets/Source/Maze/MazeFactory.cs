using System;
using Source.Interaction.Areas;
using UnityEngine;

namespace Source.Maze
{
    public sealed class MazeFactory : MonoBehaviour
    {
        private const float YAxisValue = 0f;
        private const int MinMazeWidth = 3;
        private const int MinMazeHeight = 3;

        [SerializeField] private MazeCell _cellPrefab;
        [SerializeField] private SafeArea _safeAreaPrefab;
        [SerializeField] private int _mazeWidth = 5;
        [SerializeField] private int _mazeHeight = 5;
        [SerializeField] private Vector3 _pathOffset = new Vector3(0.5f, 0.1f, 0.5f);

        public event Action OnMazeInitialized;
        
        public MazeObject Maze { get; } = new MazeObject();
        
        public SafeArea Finish { get; private set; }
        
        private void OnValidate()
        {
            _mazeWidth = Mathf.Max(MinMazeWidth, _mazeWidth);
            _mazeHeight = Mathf.Max(MinMazeHeight, _mazeHeight);
        }

        public void GenerateMaze()
        {
            if(!Maze.IsEmpty)
            {
                Maze.Clear();
                foreach (Transform child in transform)
                    Destroy(child.gameObject);
            }
            
            Maze.Generate(_mazeWidth, _mazeHeight, _pathOffset);
            InstantiateMazeWalls(Maze.Cells);
        }
        
        private void InstantiateMazeWalls(MazeCellData[,] cells)
        {
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (var y = 0; y < cells.GetLength(1); y++)
                {
                    MazeCell cell = Instantiate(_cellPrefab, new Vector3(x, YAxisValue, y), Quaternion.identity, transform);
 
                    if(!cells[x, y].WallLeftEnabled)
                        Destroy(cell.WallLeft.gameObject);
                
                    if(!cells[x, y].WallBottomEnabled)
                        Destroy(cell.WallBottom.gameObject);
                
                    if(!cells[x, y].FloorEnabled)
                        Destroy(cell.Foor.gameObject);
                }
            }
            
            Finish = Instantiate(_safeAreaPrefab, new Vector3(Maze.FinishCell.X, YAxisValue, Maze.FinishCell.Y), Quaternion.identity, transform);
            OnMazeInitialized?.Invoke();
        }
    }
}
