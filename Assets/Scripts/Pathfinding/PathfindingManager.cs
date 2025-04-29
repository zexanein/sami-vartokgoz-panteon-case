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
        
        public Dictionary<Vector2Int, PathfindingCell> cells = new();

        private bool GridGenerated { get; set; }

        private void Start()
        {
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            cells.Clear();
            
            for (var x = gridSize.x / -2; x < gridSize.x / 2; x++) 
            for (var y = gridSize.y / -2; y < gridSize.y / 2; y++)
            {
                var coordinates = new Vector2Int(x, y);
                cells.TryAdd(coordinates, new PathfindingCell(coordinates));
            }
            
            GridGenerated = true;
        }

        public PathfindingCell GetCell(Vector3 worldPosition)
        {
            var coordinates = Vector2Int.FloorToInt(worldPosition);
            if (cells.TryGetValue(coordinates, out var cell)) return cell;
            
            cell = new PathfindingCell(coordinates);
            cells.Add(coordinates, cell);
            return cell;
        }

        private HashSet<Vector2Int> _cellsToSearch = new();
        private HashSet<Vector2Int> _searchedCells = new();
        private HashSet<Vector2Int> _lastFoundPath = new();

        public List<Vector2Int> FindPath(Vector3 from, Vector3 to)
        {
            var fromConverted = Vector2Int.FloorToInt(from);
            var toConverted = Vector2Int.FloorToInt(to);
            return FindPathInGrid(fromConverted, toConverted);
        }
        
        private List<Vector2Int> FindPathInGrid(Vector2Int startCoordinates, Vector2Int targetCoordinates)
        {
            foreach (var cellToSearchCoord in _cellsToSearch) cells[cellToSearchCoord].ResetData();
            foreach (var searchedCellCoord in _searchedCells) cells[searchedCellCoord].ResetData();
            
            _cellsToSearch.Clear();
            _cellsToSearch.Add(startCoordinates);
            
            _searchedCells.Clear();

            var startCell = cells[startCoordinates];
            startCell.GCost = 0;
            startCell.HCost = GetDistance(startCoordinates, targetCoordinates);
            startCell.FCost = GetDistance(startCoordinates, targetCoordinates);

            // Search
            while (_cellsToSearch.Count > 0)
            {
                var cellToSearchCoordinates = _cellsToSearch.First();

                foreach (var cellCoordinates in _cellsToSearch)
                {
                    var cell = cells[cellCoordinates];

                    if (cell.FCost < cells[cellToSearchCoordinates].FCost ||
                        (cell.GCost == cells[cellToSearchCoordinates].FCost && cell.HCost == cells[cellToSearchCoordinates].HCost))
                        cellToSearchCoordinates = cellCoordinates;
                }

                _cellsToSearch.Remove(cellToSearchCoordinates);
                _searchedCells.Add(cellToSearchCoordinates);
                
                var finalPath = new List<Vector2Int>();

                // Finish searching when reached to target
                if (cellToSearchCoordinates == targetCoordinates)
                {
                    var pathCell = cells[targetCoordinates];

                    while (pathCell.Coordinates != startCoordinates)
                    {
                        finalPath.Insert(0, pathCell.Coordinates);
                        pathCell = cells[pathCell.Connection];
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

        private void SearchNeighbors(Vector2Int cellCoordinates, Vector2Int targetCoordinates)
        {
            for (var x = cellCoordinates.x - cellSize.x; x <= cellSize.x + cellCoordinates.x ; x += cellSize.x)
            for (var y = cellCoordinates.y - cellSize.y; y <= cellSize.y + cellCoordinates.y ; y += cellSize.y)
            {
                var neighborCoordinates = new Vector2Int(x, y);

                if (!cells.TryGetValue(neighborCoordinates, out var neighborCell) || _searchedCells.Contains(neighborCoordinates) || neighborCell.IsBlocked) continue;
                
                var gCostToNeighbor = cells[cellCoordinates].GCost + GetDistance(neighborCoordinates, targetCoordinates);
                if (gCostToNeighbor >= neighborCell.GCost) continue;
                
                neighborCell.Connection = cellCoordinates;
                neighborCell.GCost = gCostToNeighbor;
                neighborCell.HCost = GetDistance(neighborCoordinates, targetCoordinates);
                neighborCell.FCost = neighborCell.GCost + neighborCell.HCost;

                _cellsToSearch.Add(neighborCoordinates);
            }
        }

        private int GetDistance(Vector2Int a, Vector2Int b)
        {
            var distance = new Vector2Int(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));
            
            var lowest = Mathf.Min(distance.x, distance.y);
            var highest = Mathf.Max(distance.x, distance.y);
            
            var delta = highest - lowest;

            return lowest * 14 + delta * 10;
        }

        public void RegisterObstacle(Vector3 worldPosition)
        {
            GetCell(worldPosition).IsBlocked = true;
        }

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
            
            foreach (var kvp in cells)
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
