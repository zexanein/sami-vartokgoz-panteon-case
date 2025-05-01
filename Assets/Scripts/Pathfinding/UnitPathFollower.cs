using System.Collections;
using System.Collections.Generic;
using GameElements;
using UnityEngine;

namespace Pathfinding
{
    [RequireComponent(typeof(Unit))]
    public class UnitPathFollower : MonoBehaviour
    {
        private Coroutine _movementCoroutine;
        public Vector2Int Destination { get; private set; }
        public List<Vector2Int> ActivePath { get; set; }
        private List<GameObject> PathVisuals { get; } = new();
        private Unit _unit;
        private Unit UnitReference
        {
            get
            {
                if (_unit == null) _unit = GetComponent<Unit>();
                return _unit;
            }
        }
        
        public Vector3Int Coordinates => UnitReference.Coordinates;

        private bool FollowingPath { get; set; }

        public delegate void OnReachedWaypointHandler(Vector2Int waypointCoordinates, int waypointIndex);
        public event OnReachedWaypointHandler OnReachedWaypoint;

        public delegate void OnReachedDestinationHandler();
        public event OnReachedDestinationHandler OnReachedDestination;

        public delegate void OnReachedGameElementHandler(GameElement element);
        public event OnReachedGameElementHandler OnReachedGameElement;

        public delegate void OnMovingStopHandler();
        public event OnMovingStopHandler OnMovingStop;

        public void MoveToPosition(Vector3 targetPosition)
        {
            var path = PathfindingManager.Instance.FindPath(Coordinates, targetPosition);
            PathFollowerManager.Instance.GetBestAvailablePath(path);
            if (path.Count <= 1) return;
            
            PathFollowerManager.Instance.RegisterUnitPathFollower(this);
        
            StartFollowPath(path);
        }

        public void MoveToElement(GameElement targetElement)
        {
            var path = PathfindingManager.Instance.FindPath(Coordinates, targetElement.GetCoordinatesAround());
            PathFollowerManager.Instance.GetBestAvailablePath(path);
            if (path.Count <= 1) return;
            
            PathFollowerManager.Instance.RegisterUnitPathFollower(this);
            
            StartFollowPath(path);
        }

        private void StartFollowPath(List<Vector2Int> path)
        {
            if (path.Count <= 1) return;
            if (FollowingPath) StopFollowPath();

            ActivePath = path;
            Destination = ActivePath[^1];
            FollowingPath = true;
        
            DrawPathVisual();
            _movementCoroutine = StartCoroutine(FollowPath());
        }
    
        private Collider2D[] _results = new Collider2D[8];

        public bool IsElementNearby(Vector2Int waypointCoordinates, GameElement element)
        {
            _results = new Collider2D[8];
            Physics2D.OverlapCircleNonAlloc(waypointCoordinates + (Vector2) UnitReference.ParentTilemap.cellSize / 2, 1f, _results, LayerMask.GetMask("GameElements"));
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
        
        private void StopFollowPath()
        {
            if (!FollowingPath) return;
        
            FollowingPath = false;
        
            if (_movementCoroutine != null) StopCoroutine(_movementCoroutine);
        
            _movementCoroutine = null;
            ClearPathVisuals();
        
            ActivePath = new List<Vector2Int>();
        
            PathFollowerManager.Instance.UnregisterUnitPathFollower(this);
            OnMovingStop?.Invoke();
        }

        private void ClearPathVisuals()
        {
            PoolingManager.Instance.PathVisualPool.ReturnAllToPool(PathVisuals);
            PathVisuals.Clear();
        }
        
        private void DrawPathVisual()
        {
            ClearPathVisuals();
            if (ActivePath == null) return;
            foreach (var c in ActivePath) PathVisuals.Add(
                PoolingManager.Instance.PathVisualPool.GetFromPool(
                    position: GetOffsetAppliedCellPosition(c),
                    rotation: Quaternion.identity));
        }

        private Vector3 GetOffsetAppliedCellPosition(Vector2Int cellPosition) =>
            (Vector3)(Vector2)cellPosition + UnitReference.ParentTilemap.cellSize / 2f;
    
        private IEnumerator FollowPath()
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

        private void ReachedWaypoint(Vector2Int waypointCoordinates, int waypointIndex)
        {
            if (UnitReference.AttackTarget != null && IsElementNearby(waypointCoordinates, UnitReference.AttackTarget))
            {
                StopFollowPath();
                OnReachedGameElement?.Invoke(UnitReference.AttackTarget);
            }

            if (PathVisuals.Count > 0)
            {
                PoolingManager.Instance.PathVisualPool.ReturnToPool(PathVisuals[0]);
                PathVisuals.RemoveAt(0);   
            }
        
            UnitReference.Coordinates = (Vector3Int) waypointCoordinates;
            OnReachedWaypoint?.Invoke(waypointCoordinates, waypointIndex);
        }

        private void ReachedDestination()
        {
            StopFollowPath();
            OnReachedDestination?.Invoke();
        }

        private void ReachedGameElement(GameElement element)
        {
            
        }
    }
}
