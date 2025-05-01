using System.Collections;
using System.Collections.Generic;
using GameElements;
using UnityEngine;

namespace Pathfinding
{
    /// <summary>
    /// Handles path following behavior for units, including movement execution, path visualization,
    /// and target detection
    /// </summary>
    [RequireComponent(typeof(Unit))]
    public class UnitPathFollower : MonoBehaviour
    {
        private Coroutine _movementCoroutine;
        
        /// <summary>
        /// The final destination coordinates of the current path
        /// </summary>
        public Vector2Int Destination { get; private set; }
        
        /// <summary>
        /// List of waypoints comprising the current active path
        /// </summary>
        private List<Vector2Int> ActivePath { get; set; }
        
        /// <summary>
        /// Visual indicators for the current path
        /// </summary>
        private List<GameObject> PathVisuals { get; } = new();
        private Unit _unit;
        
        /// <summary>
        /// Reference to the Unit component with lazy initialization
        /// </summary>
        private Unit UnitReference
        {
            get
            {
                if (_unit == null) _unit = GetComponent<Unit>();
                return _unit;
            }
        }

        /// <summary>
        /// Flag indicating whether the unit is currently following a path
        /// </summary>
        private bool FollowingPath { get; set; }

        /// <summary>
        /// Event invoked when the unit reaches its target game element
        /// </summary>
        public delegate void OnReachedGameElementHandler(GameElement element);
        public event OnReachedGameElementHandler OnReachedGameElement;

        /// <summary>
        /// Moves the unit to the specified position.
        /// </summary>
        /// <param name="targetPosition">The target position to move to.</param>
        public void MoveToPosition(Vector3 targetPosition)
        {
            var path = PathfindingManager.Instance.FindPath(UnitReference.Coordinates, targetPosition);
            PathFollowerManager.Instance.GetBestAvailablePath(path);
            if (path.Count <= 1) return;
            
            PathFollowerManager.Instance.RegisterUnitPathFollower(this);
        
            StartFollowPath(path);
        }

        /// <summary>
        /// Moves the unit to the specified element.
        /// </summary>
        /// <param name="targetElement">The target element to move to.</param>
        public void MoveToElement(GameElement targetElement)
        {
            var path = PathfindingManager.Instance.FindPath(UnitReference.Coordinates, targetElement.GetCoordinatesAround());
            PathFollowerManager.Instance.GetBestAvailablePath(path);
            if (path.Count <= 1) return;
            
            PathFollowerManager.Instance.RegisterUnitPathFollower(this);
            
            StartFollowPath(path);
        }

        /// <summary>
        /// Begins following the provided path
        /// </summary>
        /// <param name="path">List of coordinates representing the path</param>
        private void StartFollowPath(List<Vector2Int> path)
        {
            if (path.Count <= 1) return;
            if (FollowingPath) StopFollowPath();

            ActivePath = path;
            Destination = ActivePath[^1];
            FollowingPath = true;
        
            DrawPathVisuals();
            _movementCoroutine = StartCoroutine(FollowPathCoroutine());
        }
    
        private Collider2D[] _results = new Collider2D[8];

        /// <summary>
        /// Checks if a game element is within proximity of specified coordinates
        /// </summary>
        /// <param name="waypointCoordinates">Coordinates to check around</param>
        /// <param name="element">Element to look for</param>
        /// <param name="radiusInTiles">Radius in tiles to check around the coordinates</param>
        /// <returns>True if element is found nearby, false otherwise</returns>
        public bool IsElementNearby(Vector2Int waypointCoordinates, GameElement element, int radiusInTiles = 1)
        {
            _results = new Collider2D[8];
            Physics2D.OverlapCircleNonAlloc(waypointCoordinates + (Vector2) UnitReference.ParentTilemap.cellSize / 2, radiusInTiles, _results, LayerMask.GetMask("GameElements"));
            if (_results.Length <= 0) return false;
        
            foreach (var result in _results)
            {
                if (result == null) continue;
                result.TryGetComponent(out GameElement detectedElement);
                if (detectedElement == UnitReference) continue;
                if (detectedElement != element) continue;
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Aborts current path following operation
        /// </summary>
        private void StopFollowPath()
        {
            if (!FollowingPath) return;
        
            FollowingPath = false;
        
            if (_movementCoroutine != null) StopCoroutine(_movementCoroutine);
        
            _movementCoroutine = null;
            ClearPathVisuals();
        
            ActivePath = new List<Vector2Int>();
        
            PathFollowerManager.Instance.UnregisterUnitPathFollower(this);
        }

        /// <summary>
        /// Clears all active path visualization markers
        /// </summary>
        private void ClearPathVisuals()
        {
            PoolingManager.Instance.PathVisualPool.ReturnAll(PathVisuals);
            PathVisuals.Clear();
        }

        /// <summary>
        /// Spawns visual indicators objects to the cell positions of current path
        /// </summary>
        private void DrawPathVisuals()
        {
            ClearPathVisuals();
            if (ActivePath == null) return;
            foreach (var c in ActivePath) PathVisuals.Add(
                PoolingManager.Instance.PathVisualPool.GetFromPool(
                    position: GetOffsetAppliedCellPosition(c),
                    rotation: Quaternion.identity));
        }

        /// <summary>
        /// Adjusts cell position by adding half cell size offset for proper centering
        /// </summary>
        /// <param name="cellPosition">Raw cell coordinates</param>
        /// <returns>Adjusted world position</returns>
        private Vector3 GetOffsetAppliedCellPosition(Vector2Int cellPosition) =>
            (Vector3)(Vector2)cellPosition + UnitReference.ParentTilemap.cellSize / 2f;
    
        /// <summary>
        /// Coroutine that handles incremental movement along the path
        /// </summary>
        private IEnumerator FollowPathCoroutine()
        {
            for (var i = 0; i < ActivePath.Count; i++)
            {
                var targetPosition = GetOffsetAppliedCellPosition(ActivePath[i]);
                while (transform.position != targetPosition)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * UnitReference.UnitBlueprint.movementSpeed);
                    yield return null;
                }
            
                ReachedWaypoint(ActivePath[i], i);
            }
        
            ReachedDestination();
        }

        /// <summary>
        /// Handles logic when reaching to a waypoint
        /// </summary>
        /// <param name="waypointCoordinates">Coordinates of reached waypoint</param>
        /// <param name="waypointIndex">Index of waypoint in path</param>
        private void ReachedWaypoint(Vector2Int waypointCoordinates, int waypointIndex)
        {
            if (UnitReference.AttackTarget != null && IsElementNearby(waypointCoordinates, UnitReference.AttackTarget))
            {
                StopFollowPath();
                OnReachedGameElement?.Invoke(UnitReference.AttackTarget);
            }

            if (PathVisuals.Count > 0)
            {
                PoolingManager.Instance.PathVisualPool.Return(PathVisuals[0]);
                PathVisuals.RemoveAt(0);   
            }
        
            UnitReference.Coordinates = (Vector3Int) waypointCoordinates;
        }

        /// <summary>
        /// Handles logic when reaching the final destination
        /// </summary>
        private void ReachedDestination()
        {
            StopFollowPath();
        }
    }
}
