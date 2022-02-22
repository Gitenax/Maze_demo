using Source.Maze;
using UnityEngine;

namespace Source.View
{
    [RequireComponent(typeof(LineRenderer))]
    public sealed class MazePathConstructor : MonoBehaviour
    {
        [SerializeField] private MazeFactory _mazeFactory;

        private LineRenderer _lineRenderer;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _mazeFactory.Maze.OnMazeGenerated += OnMazeGeneratedHandler;
        }

        private void OnDisable() => _mazeFactory.Maze.OnMazeGenerated -= OnMazeGeneratedHandler;

        private void OnDestroy() => _mazeFactory.Maze.OnMazeGenerated -= OnMazeGeneratedHandler;

        private void OnMazeGeneratedHandler() => DrawPath();

        private void DrawPath()
        {
            _lineRenderer.positionCount = _mazeFactory.Maze.ShorcutPath.Count;
            _lineRenderer.SetPositions(_mazeFactory.Maze.ShorcutPath);
        }
    }
}
