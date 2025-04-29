using System.Collections.Generic;
using System.Linq;
using GameElements;
using Pathfinding;
using UnityEngine;

public class UnitMovementManager : MonoBehaviour
{
    #region Singleton
    private static UnitMovementManager _instance;
    public static UnitMovementManager Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<UnitMovementManager>();
            return _instance;
        }
    }
    #endregion

    private List<UnitPathFollower> ActivePathFollowers { get; } = new();

    public void RegisterUnitMovement(Unit unit, Vector3 target)
    {
        var path = PathfindingManager.Instance.FindPath(unit.Coordinates, target);
        
        //When two units going to same target, go possible further point
        while (path.Count > 1 && ActivePathFollowers.Any(f => f.Destination == path[^1]))
            path.RemoveAt(path.Count - 1);
        
        if (path.Count <= 1) return;
        
        if (unit.PathFollower == null) unit.PathFollower = unit.gameObject.AddComponent<UnitPathFollower>();
        var follower = unit.PathFollower;
        
        if (!ActivePathFollowers.Contains(follower)) ActivePathFollowers.Add(follower);
        
        follower.OnReachedDestination += () => UnregisterUnitMovement(follower);
        follower.StartFollowPath(path);
    }

    private void UnregisterUnitMovement(UnitPathFollower follower)
    {
        ActivePathFollowers.Remove(follower);
    }
}
