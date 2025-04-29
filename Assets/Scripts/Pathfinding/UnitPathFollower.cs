using System.Collections;
using System.Collections.Generic;
using GameElements;
using PlacementSystem;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitPathFollower : MonoBehaviour
{
    private Coroutine _movementCoroutine;
    public Vector2Int Destination { get; private set; }
    private List<Vector2Int> ActivePath { get; set; }
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

    private bool FollowingPath { get; set; }

    public delegate void OnReachedWaypointHandler(Vector2Int waypointCoordinates, int waypointIndex);
    public OnReachedWaypointHandler OnReachedWaypoint;

    public delegate void OnReachedDestinationHandler();
    public OnReachedDestinationHandler OnReachedDestination;
    
    public void StartFollowPath(List<Vector2Int> path)
    {
        if (path.Count <= 1) return;
        if (FollowingPath) StopFollowPath();

        ActivePath = path;
        Destination = ActivePath[^1];
        FollowingPath = true;
        
        DrawPathVisual();
        _movementCoroutine = StartCoroutine(FollowPath());
    }

    private void StopFollowPath()
    {
        if (!FollowingPath) return;
        
        FollowingPath = false;
        
        if (_movementCoroutine != null)
            StopCoroutine(_movementCoroutine);
        
        _movementCoroutine = null;
        ClearPathVisuals();
        
        ActivePath = null;
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
        PoolingManager.Instance.PathVisualPool.ReturnToPool(PathVisuals[0]);
        PathVisuals.RemoveAt(0);
        
        UnitReference.Coordinates = (Vector3Int) waypointCoordinates;
        OnReachedWaypoint?.Invoke(waypointCoordinates, waypointIndex);
    }

    private void ReachedDestination()
    {
        StopFollowPath();
        OnReachedDestination?.Invoke();
    }
}
