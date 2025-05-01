using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pathfinding
{
    [Serializable]
    public class PathfindingCell
    {
        public Vector2Int Coordinates { get; private set; }
        public int FCost { get; set; } = int.MaxValue;
        public int GCost { get; set; } = int.MaxValue;
        public int HCost { get; set; } = int.MaxValue;
        public Vector2Int Connection { get; set; }
        public bool IsBlocked { get; set; }

        public PathfindingCell(Vector2Int coordinates)
        {
            Coordinates = coordinates;
        }

        public void ResetData()
        {
            GCost = int.MaxValue;
            HCost = int.MaxValue;
            FCost = int.MaxValue;
            Connection = default;
        }
    }
    
    public class PathfindingManager : MonoBehaviour
    {
        #region Singleton
        private static PathfindingManager _instance;
        public static PathfindingManager Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<PathfindingManager>();
                return _instance;
            }
        }
        #endregion
        
        [SerializeField] private Vector2Int gridSize = new(20, 20);
        [SerializeField] private Vector2Int cellSize = Vector2Int.one;
        [SerializeField] private Vector2 gridOffset;
        
        /// <summary>
        /// The dictionary that stores the pathfinding cells.
        /// </summary>
        private Dictionary<Vector2Int, PathfindingCell> _cells = new();

        /// <summary>
        /// Indicates whether the grid has been generated.
        /// </summary>
        private bool GridGenerated { get; set; }

        /// <summary>
        /// Generates the grid of pathfinding cells when the script is started.
        /// </summary>
        private void Start()
        {
            GenerateGrid();
        }

        /// <summary>
        /// Generates the grid of pathfinding cells.
        /// </summary>
        private void GenerateGrid()
        {
            _cells.Clear();
            
            for (var x = gridSize.x / -2; x < gridSize.x / 2; x++) 
            for (var y = gridSize.y / -2; y < gridSize.y / 2; y++)
            {
                var coordinates = new Vector2Int(x, y);
                _cells.TryAdd(coordinates, new PathfindingCell(coordinates));
            }
            
            GridGenerated = true;
        }

        /// <summary>
        /// Gets the pathfinding cell at the specified world position.
        /// </summary>
        /// <param name="worldPosition">The world position to get the cell for.</param>
        /// <returns>If the cell exists, returns the cell; otherwise, creates a new cell and returns it.</returns>
        private PathfindingCell GetCell(Vector3 worldPosition)
        {
            var coordinates = Vector2Int.FloorToInt(worldPosition);
            if (_cells.TryGetValue(coordinates, out var cell)) return cell;
            
            cell = new PathfindingCell(coordinates);
            _cells.Add(coordinates, cell);
            return cell;
        }

        private HashSet<Vector2Int> _cellsToSearch = new();
        private HashSet<Vector2Int> _searchedCells = new();
        private HashSet<Vector2Int> _lastFoundPath = new();

        /// <summary>
        /// Finds a path from the start position to the target position.
        /// </summary>
        /// <returns>List of <c>Vector2Int</c> representing the path from start to target.</returns>
        public List<Vector2Int> FindPath(Vector3 start, Vector3 target)
        {
            var startConverted = Vector2Int.FloorToInt(start);
            var targetConverted = Vector2Int.FloorToInt(target);
            
            return _cells[targetConverted].IsBlocked
                ? new List<Vector2Int>()
                : FindPathInGrid(startConverted, targetConverted);
        }

        /// <summary>
        /// Finds a path from the start position to the nearest target position in the list of targets.
        /// </summary>
        /// <param name="start">The starting position.</param>
        /// <param name="targets">List of target positions to search for the nearest one.</param>
        /// <returns>List of <c>Vector2Int</c> representing the path from start to target.</returns>
        public List<Vector2Int> FindPath(Vector3 start, List<Vector2Int> targets)
        {
            var startConverted = Vector2Int.FloorToInt(start);

            var nearestDistance = int.MaxValue;
            Vector2Int? nearest = null;
            
            foreach (var target in targets)
            {
                var targetConverted = Vector2Int.FloorToInt(target);
                if (_cells[targetConverted].IsBlocked) continue;

                var distance = GetDistance(startConverted, targetConverted);
                if (distance >= nearestDistance) continue;
                nearestDistance = distance;
                nearest = targetConverted;
            }
            
            return nearest == null
                ? new List<Vector2Int>()
                : FindPathInGrid(startConverted, nearest.Value);
        }
        
        /// <summary>
        /// Finds a path in the grid from the start coordinates to the target coordinates.
        /// </summary>
        /// <returns>List of <c>Vector2Int</c> representing the path from start to target.</returns>
        private List<Vector2Int> FindPathInGrid(Vector2Int startCoordinates, Vector2Int targetCoordinates)
        {
            foreach (var cellToSearchCoord in _cellsToSearch) _cells[cellToSearchCoord].ResetData();
            foreach (var searchedCellCoord in _searchedCells) _cells[searchedCellCoord].ResetData();
            
            _cellsToSearch.Clear();
            _cellsToSearch.Add(startCoordinates);
            
            _searchedCells.Clear();

            var startCell = _cells[startCoordinates];
            startCell.GCost = 0;
            startCell.HCost = GetDistance(startCoordinates, targetCoordinates);
            startCell.FCost = GetDistance(startCoordinates, targetCoordinates);

            // Search
            while (_cellsToSearch.Count > 0)
            {
                var cellToSearchCoordinates = _cellsToSearch.First();

                foreach (var cellCoordinates in _cellsToSearch)
                {
                    var cell = _cells[cellCoordinates];

                    if (cell.FCost < _cells[cellToSearchCoordinates].FCost ||
                        (cell.GCost == _cells[cellToSearchCoordinates].FCost && cell.HCost == _cells[cellToSearchCoordinates].HCost))
                        cellToSearchCoordinates = cellCoordinates;
                }

                _cellsToSearch.Remove(cellToSearchCoordinates);
                _searchedCells.Add(cellToSearchCoordinates);
                
                var finalPath = new List<Vector2Int>();

                // Finish searching when reached to target
                if (cellToSearchCoordinates == targetCoordinates)
                {
                    var pathCell = _cells[targetCoordinates];

                    while (pathCell.Coordinates != startCoordinates)
                    {
                        finalPath.Insert(0, pathCell.Coordinates);
                        pathCell = _cells[pathCell.Connection];
                    }
                    
                    finalPath.Insert(0, startCoordinates);
                    
                    _lastFoundPath = finalPath.ToHashSet();
                    return finalPath;
                }
                
                // Search neighbors
                SearchNeighbors(cellToSearchCoordinates, targetCoordinates);
            }

            _lastFoundPath = new HashSet<Vector2Int>();
            return new List<Vector2Int>();
        }

        /// <summary>
        /// Searches the neighbors of the given cell coordinates and updates their costs and connections.
        /// </summary>
        /// <param name="cellCoordinates">The coordinates of the cell to search neighbors for.</param>
        /// <param name="targetCoordinates">The coordinates of the target cell.</param>
        private void SearchNeighbors(Vector2Int cellCoordinates, Vector2Int targetCoordinates)
        {
            for (var x = cellCoordinates.x - cellSize.x; x <= cellSize.x + cellCoordinates.x ; x += cellSize.x)
            for (var y = cellCoordinates.y - cellSize.y; y <= cellSize.y + cellCoordinates.y ; y += cellSize.y)
            {
                var neighborCoordinates = new Vector2Int(x, y);

                if (!_cells.TryGetValue(neighborCoordinates, out var neighborCell) || _searchedCells.Contains(neighborCoordinates) || neighborCell.IsBlocked) continue;
                
                var gCostToNeighbor = _cells[cellCoordinates].GCost + GetDistance(neighborCoordinates, targetCoordinates);
                if (gCostToNeighbor >= neighborCell.GCost) continue;
                
                neighborCell.Connection = cellCoordinates;
                neighborCell.GCost = gCostToNeighbor;
                neighborCell.HCost = GetDistance(neighborCoordinates, targetCoordinates);
                neighborCell.FCost = neighborCell.GCost + neighborCell.HCost;

                _cellsToSearch.Add(neighborCoordinates);
            }
        }

        /// <summary>
        /// Calculates the distance between cell a and cell b using the Manhattan distance formula.
        /// </summary>
        /// <returns>The distance between the two cells.</returns>
        private int GetDistance(Vector2Int a, Vector2Int b)
        {
            var distance = new Vector2Int(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));
            
            var lowest = Mathf.Min(distance.x, distance.y);
            var highest = Mathf.Max(distance.x, distance.y);
            
            var delta = highest - lowest;

            return lowest * 14 + delta * 10;
        }

        /// <summary>
        /// Gets the pathfinding cell at the specified world position and sets it as blocked.
        /// </summary>
        public void RegisterObstacle(Vector3 worldPosition)
        {
            GetCell(worldPosition).IsBlocked = true;
        }

        /// <summary>
        /// Unregisters the pathfinding cell at the specified world position, making it walkable again.
        /// </summary>
        public void UnregisterObstacle(Vector3 worldPosition)
        {
            GetCell(worldPosition).IsBlocked = false;   
        }

        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying && !GridGenerated) GenerateGrid();
            

            var walkableColor = new Color(0, 0, 1, 0.02f);
            var pathColor = new Color(1, 1, 0, 0.1f);
            var unwalkableColor = new Color(1, 0, 0, 0.1f);
            
            foreach (var kvp in _cells)
            {
                if (kvp.Value.IsBlocked)
                {
                    Gizmos.color = unwalkableColor;
                    Gizmos.DrawCube(kvp.Key + (Vector2) transform.position + gridOffset, (Vector2) cellSize);
                }
                
                else if (_lastFoundPath.Contains(kvp.Key))
                {
                    Gizmos.color = pathColor;
                    Gizmos.DrawCube(kvp.Key + (Vector2) transform.position + gridOffset, (Vector2) cellSize);
                }
                
                else
                {
                    Gizmos.color = walkableColor;
                    Gizmos.DrawWireCube(kvp.Key + (Vector2) transform.position + gridOffset, (Vector2) cellSize);
                }
            }
            
            if (!Application.isPlaying) GridGenerated = false;
        }
    }
}
